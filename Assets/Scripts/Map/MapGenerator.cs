using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //TODO 맵이름 설정 할 수 있게
    [ContextMenu("맵 생성기")]
    public void Generate(string name)
    {
         List<MapElement> loadedMapData = MapDataManager.Instance.LoadMapData(name);
        foreach (var element in loadedMapData)
        {
            GameObject prefab = Resources.Load<GameObject>($"Map/{element.name}");
            GameObject obj = Instantiate(prefab,transform.parent);
            obj.transform.position = element.position;
            obj.transform.rotation = element.rotation;
            obj.transform.localScale = element.localScale;
        }

    }
}