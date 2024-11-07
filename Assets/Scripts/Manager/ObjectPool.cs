 using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : DestroySingleton<ObjectPool>
{
    
    [SerializeField]private GameObject prefab;
    [SerializeField] private int poolSize;
    
    private List<GameObject> pool = new List<GameObject>();
    
    private void Start()
    {   
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab,transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        
        return CreateNewGameObject();
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private GameObject CreateNewGameObject()
    {
        GameObject newObj = Instantiate(prefab);
        pool.Add(newObj);
        return newObj;
    }
    
}
