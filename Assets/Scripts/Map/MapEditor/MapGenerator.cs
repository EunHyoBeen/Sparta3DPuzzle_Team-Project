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
    public MapType Type { get; private set; }
    private float tileSize;

    private void Start()
    {
    }

    public event Action OnGenerateMap;


    [ContextMenu("맵삭제")]
    void ClearAllElement()
    {
        foreach (Transform child in MapEditor.Instance.mapContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void GenerateDefaultMap(MapType type, int width, int height)
    {
        GameObject defaultTerrain = Resources.Load<GameObject>($"Map/{type.ToString()}/terrain");

        switch (Type)
        {
            case MapType.Space:
                tileSize = 2f;
                break;
            case MapType.Dungeon:
                tileSize = 4f;
                break;
            case MapType.Maze:
                break;
        }


        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 terrainPos = new Vector3(tileSize * j, 0, tileSize * i);
                Instantiate(defaultTerrain, terrainPos, Quaternion.identity, MapEditor.Instance.mapContainer.transform);
            }
        }

        OnGenerateMap?.Invoke();
    }

    public void GenerateByMapData(List<MapElement> mapData)
    {
        GameObject loadedMap = new GameObject("lOADED MAP");
        GameObject prefab;
        foreach (var element in mapData)
        {
            string elementName = RemoveCloneFormat(element);

                if (elementName == "PlayerPosIndicator")
                {
                    prefab = Resources.Load<GameObject>($"Map/{Type}/Player");
                }

            else
            {
                prefab = Resources.Load<GameObject>($"Map/{Type}/{elementName}");
            }


            GameObject obj = Instantiate(prefab, loadedMap.transform);
            obj.transform.position = element.position;
            obj.transform.rotation = element.rotation;
            obj.transform.localScale = element.localScale;
            if (obj.TryGetComponent<PuzzleControllerBase>(out PuzzleControllerBase type))
            {
                type.SetCurrentPuzzleType(element.type);
            }

        }
        
        OnGenerateMap?.Invoke();
    }

    
    private string RemoveCloneFormat(MapElement element)
    {
        string elementName = element.name;

        int cloneIndex = elementName.IndexOf("(Clone)");

        if (cloneIndex >= 0)
        {
            elementName = elementName.Substring(0, cloneIndex);
        }

        return elementName;
    }
}