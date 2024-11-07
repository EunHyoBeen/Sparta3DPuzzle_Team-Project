
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayCustomGameUI : MonoBehaviour
{
    [SerializeField] private Transform mapDataLoadButtonContainer;
    [SerializeField] private Button quitButton;
    
    private void Start()
    {
        MapEditor.Instance.generator.OnGenerateMap += HideUI;
        quitButton.onClick.AddListener(OnQuit);
        SetMapDataLoadButton();
    }


    private void OnQuit()
    {
        FadeManager.Instance.LoadScene("StartScene");
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void SetMapDataLoadButton()
    {
        string mapDataDirectoryPath = Application.dataPath + "/MapData";

        if (!Directory.Exists(mapDataDirectoryPath))
        {
            Directory.CreateDirectory(mapDataDirectoryPath);
        }

        string[] files = Directory.GetFiles(mapDataDirectoryPath, "*.json"); 

        foreach (var filePath in files)
        {
            string mapDataName = Path.GetFileNameWithoutExtension(filePath); 
            Debug.Log(mapDataName);

            GameObject mapDataLoadButton = ObjectPool.Instance.GetObject();
            mapDataLoadButton.GetComponent<MapDataLoadButton>().Setting(mapDataName);
            mapDataLoadButton.transform.SetParent(mapDataLoadButtonContainer.transform);
        }
    }


}
 