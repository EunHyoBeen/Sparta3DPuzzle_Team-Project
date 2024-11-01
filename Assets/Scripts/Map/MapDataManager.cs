using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;




public class MapDataManager : Singleton<MapDataManager>
{
    public GameObject saveMap;
    private readonly List<MapElement> mapData = new List<MapElement>();
    private string filePath;

    protected override void Awake()
    {
        base.Awake();
    }


    [ContextMenu("맵데이터 저장")]
    public void SaveMapData()
    {
        string mapName = "";
        if (mapName == "")
            mapName = SceneManager.GetActiveScene().name;

        //TODO 빌드 전에 persistentDataPath로 변경
        filePath = Application.dataPath  + $"/MapData/MapData_{mapName}.json";
        
        mapData.Clear();
        for (int i = 0; i < saveMap.transform.childCount; i++)
        {
            var child = saveMap.transform.GetChild(i);
            MapElement newMapData = new MapElement(
                child.name,
                child.position,
                child.rotation,
                child.localScale
            );
            mapData.Add(newMapData);
        }

        string jsonData = JsonUtility.ToJson(new SerializableMapData(mapData), true);
        File.WriteAllText(filePath, jsonData);
        Debug.Log($"맵 데이터가 저장 됐습니다: '{mapName}' 경로: {filePath}");
    }

    
    public List<MapElement> LoadMapData(string filename)
    {
        filePath = Path.Combine(Application.dataPath, $"MapData/{filename}.json");
        
        if (!File.Exists(filePath))
        {
            Debug.LogError($"해당 Data를 발견하지 못했습니다.: {filePath}");
            return new List<MapElement>();
        }
        string jsonData = File.ReadAllText(filePath);

        SerializableMapData loadedData = JsonUtility.FromJson<SerializableMapData>(jsonData);
        Debug.Log($"로딩된 데이터: {filePath}");
        return loadedData != null ? loadedData.elements : new List<MapElement>();
    }
}
[System.Serializable]
public class SerializableMapData
{
    public List<MapElement> elements;

    public SerializableMapData(List<MapElement> elements)
    {
        this.elements = elements;
    }
}