using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int _width;
    private int _height;
    private MapType _type;
    private float tileSize;
    private GameObject mapContainer;
    public void InitData(int width, int height, MapType type , GameObject container)
    {
        _width = width;
        _height = height;
        _type = type;
        mapContainer = container;
    }
    
    
    [ContextMenu("맵삭제")]
    void ClearAllElement()
    {
        foreach (Transform child in mapContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void GenerateDefaultMap()
    {
         
        GameObject defaultTerrain = Resources.Load<GameObject>($"Map/{_type.ToString()}/terrain");

        
        switch (_type)
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
        
        
        for (int i = 0; i < _height ; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                Vector3 terrainPos = new Vector3(tileSize * j, 0, tileSize * i);
                Instantiate(defaultTerrain, terrainPos, Quaternion.identity, mapContainer.transform);
            }
        }
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