using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapDataLoadButton : MonoBehaviour
{
    private string _mapDataName;
    [SerializeField]private TMP_Text _text;
    
    public void Setting(string mapDataName )
    {
        _mapDataName = mapDataName;
        _text.text = mapDataName;
    }

    public void StartLoadedMapData()
    {
        MapDataManager.Instance.CreateMap(_mapDataName);
        Transform currentParent = gameObject.transform.parent;

        while (currentParent != null)
        {
            currentParent.gameObject.SetActive(false);
            currentParent = currentParent.parent;
        }
    }
}
