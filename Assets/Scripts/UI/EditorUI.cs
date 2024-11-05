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

public class EditorUI : MonoBehaviour
{
    [SerializeField] private Transform elementButtonContainer;
    [SerializeField] private Button[] elementTypeButtons;


    private Button curButton;
    private List<GameObject> activeElementResourceButtons = new List<GameObject>();
    private MapType mapType;


    private void Start()
    {
        gameObject.SetActive(false);
        MapEditor.Instance.generator.OnGenerateDefaultMap += Activate;
        mapType = MapEditor.Instance.generator.Type;
        SetElementButtonByType(3);
        InitElementTypeButton();
    }
 
    
 
    private void InitElementTypeButton()
    {
        for (int i = 0; i < elementTypeButtons.Length; i++)
        {
            int index = i; 
            elementTypeButtons[i].onClick.AddListener(() => SetElementButtonByType(index));
        }
        
    }

    

    private void Activate()
    {
        gameObject.SetActive(true);
    }


    public void SetElementButtonByType(int type)
    {

         curButton = elementTypeButtons[type];
        
        if (activeElementResourceButtons.Count != 0)
            ReturnButtonsToPool();

        ElementType elementType = (ElementType)type;
        string defaultResourcePath = $"Map/{mapType}/{elementType.ToString()}";
        string defaultIconPath = $"UI/{elementType.ToString()}";

        GameObject[] resourcePrefabs = Resources.LoadAll<GameObject>(defaultResourcePath);

        foreach (var obj in resourcePrefabs)
        {
            string resourcePath = $"{defaultResourcePath}/{obj.name}";
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