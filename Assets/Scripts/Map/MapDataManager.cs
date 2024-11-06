using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;




public class MapDataManager : DestroySingleton<MapDataManager>
{ 
    private GameObject mapContainer;
    private  List<MapElement> mapData = new List<MapElement>();
    private string filePath;

    

    private void Start()
    {
        mapContainer = MapEditor.Instance.mapContainer != null ? MapEditor.Instance.mapContainer :new GameObject("MapContainer");
    }


     public void SaveMapData(string mapName)
    {
        if(mapContainer == null)
            return;
 
        filePath = Application.dataPath  +  $"/MapData/{mapName}.json";
        
        mapData.Clear();
        for (int i = 0; i <mapContainer.transform.childCount; i++)
        {
            var child = mapContainer.transform.GetChild(i);

            if (child.TryGetComponent<PuzzleControllerBase>(out PuzzleControllerBase type))
            {
                MapElement newMapData = new MapElement(
                    child.name,
                    child.position,
                    child.rotation,
                    child.localScale,
                    type.CurrentPuzzleType
                 );
                mapData.Add(newMapData);
            }

            else
            {
                MapElement newMapData = new MapElement(
                    child.name,
                    child.position,
                    child.rotation,
                    child.localScale
                );
                mapData.Add(newMapData);
            }
      
        }

        string jsonData = JsonUtility.ToJson(new MapElementContainer(mapData), true);
        File.WriteAllText(filePath, jsonData);
        Debug.Log($"맵 데이터가 저장 됐습니다: '{mapName}' 경로: {filePath}");
    }

     public List<MapElement> LoadMapData(string mapDataName)
    {
        filePath = Path.Combine(Application.dataPath, $"MapData/{mapDataName}.json");
        
        if (!File.Exists(filePath))
        {
            Debug.LogError($"해당 Data를 발견하지 못했습니다.: {filePath}");
            return new List<MapElement>();
        }
        string jsonData = File.ReadAllText(filePath);

        MapElementContainer loadedElementContainer = JsonUtility.FromJson<MapElementContainer>(jsonData);
        Debug.Log($"로딩된 데이터: {filePath}");
        return loadedElementContainer != null ? loadedElementContainer.elements : new List<MapElement>();
    }

      public void CreateMap(string mapDataName)
    {
        MapEditor.Instance.generator.GenerateByMapData(LoadMapData(mapDataName));
    }
}


[System.Serializable]
public class MapElementContainer
{
    public List<MapElement> elements;

    public MapElementContainer(List<MapElement> elements)
    {
        this.elements = elements;
    }
}