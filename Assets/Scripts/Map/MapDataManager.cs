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
        Debug.Log($"Map data saved for scene '{mapName}' at path: {filePath}");
    }

    
    public List<MapElement> LoadMapData(string filename)
    {
        filePath = Path.Combine(Application.dataPath, $"MapData/{filename}.json");
        
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found at path: {filePath}");
            return new List<MapElement>();
        }
        string jsonData = File.ReadAllText(filePath);

        SerializableMapData loadedData = JsonUtility.FromJson<SerializableMapData>(jsonData);
        Debug.Log($"Loaded map data from {filePath}");
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