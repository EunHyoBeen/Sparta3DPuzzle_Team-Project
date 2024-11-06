using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ElementType
{
    Terrain ,
    Prop,
    Puzzle,
    Trap
}

//UI를 처리해 주는 부분과
//보여주는 부분을 분리 해주는 게 좋음. 



public class EditorUI : MonoBehaviour
{
    [SerializeField] private Transform elementButtonContainer;
    [SerializeField] private Button[] elementTypeButtons;
    [SerializeField] private Button playerPosButton;
    [SerializeField] private Button endPointButton;
    private Button curButton;
    private List<GameObject> activeElementResourceButtons = new List<GameObject>();
    private MapType mapType;


    private void Start()
    {
        gameObject.SetActive(false);
        MapEditor.Instance.generator.OnGenerateDefaultMap += Activate;
        mapType = MapEditor.Instance.generator.Type;
        SetElementButtonByType(0);
        InitButton();
    }
    
 
    private void InitButton()
    {
        for (int i = 0; i < elementTypeButtons.Length; i++)
        {
            int index = i; 
            elementTypeButtons[i].onClick.AddListener(() => SetElementButtonByType(index));
        }
        
        
        playerPosButton.onClick.AddListener(SetPlayerSpawnPosElement);
        endPointButton.onClick.AddListener(SetEndPointElement);
    }

    

    private void Activate()
    {
        gameObject.SetActive(true);
    }


    public void SetPlayerSpawnPosElement()
    {
        playerPosButton.interactable = false;
        MapEditor.Instance.builder.CreateBuildElement($"Map/{MapEditor.Instance.generator.Type}/PlayerPosIndicator");
    }

    public void SetEndPointElement()
    {
        endPointButton.interactable = false;
        MapEditor.Instance.builder.CreateBuildElement($"Map/{MapEditor.Instance.generator.Type}/CustomGameEndPoint");
    }

    
    public void SetElementButtonByType(int type)
    {

         curButton = elementTypeButtons[type];
        
        if (activeElementResourceButtons.Count != 0)
            ReturnButtonsToPool();

        ElementType elementType = (ElementType)type;
        string defaultResourcePath = $"Map/{mapType}";
        string defaultIconPath = $"UI/{mapType}/{elementType.ToString()}";

        Sprite[] iconFiles  = Resources.LoadAll<Sprite>(defaultIconPath);

        foreach (var obj in iconFiles)
        {
            string resourcePath = $"{defaultResourcePath}/{obj.name}";
            Debug.Log(resourcePath);
            string iconPath = $"{defaultIconPath}/{obj.name}";
            GameObject resourceButton = ObjectPool.Instance.GetObject();
            resourceButton.GetComponent<ResourceLoadButton>().Setting(resourcePath,iconPath);
            resourceButton.transform.SetParent(elementButtonContainer.transform);
            activeElementResourceButtons.Add(resourceButton);
        }
    }


    public void ReturnButtonsToPool()
    {
        foreach (var button in activeElementResourceButtons)
        {
            ObjectPool.Instance.ReturnObject(button);
        }

        activeElementResourceButtons.Clear();
    }
}