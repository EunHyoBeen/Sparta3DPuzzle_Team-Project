using System;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    Space,
    Dungeon,
    Maze
}

public class MapGenerator : MonoBehaviour
{
    public MapType Type { get; private set;}
    private float tileSize;
    private GameObject mapContainer; 

    public event Action OnGenerateDefaultMap;
    

    [ContextMenu("맵삭제")]
    void ClearAllElement()
    {
        foreach (Transform child in mapContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void GenerateDefaultMap(MapType type,int width,int height)
    {
        GameObject defaultTerrain = Resources.Load<GameObject>($"Map/{type.ToString()}/terrain");
        mapContainer = new GameObject("Map Container");
        
        switch (Type)
        {
            case MapType.Space:
                tileSize = 2f;
                break;
            case MapType.Dungeon:
                tileSize = 4f;
                break;
            case MapType.Maze:
                //maze 프리펩을 못 찾음 
                break;
        }


        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 terrainPos = new Vector3(tileSize * j, 0, tileSize * i);
                Instantiate(defaultTerrain, terrainPos, Quaternion.identity, mapContainer.transform);
            }
        }
        
        OnGenerateDefaultMap?.Invoke();
    }

    public void GenerateByMapData(List<MapElement> mapData)
    {
        ClearAllElement();
        foreach (var element in mapData)
        {
            GameObject prefab = Resources.Load<GameObject>($"Map/{element.name}");
            GameObject obj = Instantiate(prefab, mapContainer.transform);
            obj.transform.position = element.position;
            obj.transform.rotation = element.rotation;
            obj.transform.localScale = element.localScale;
        }
    }
}