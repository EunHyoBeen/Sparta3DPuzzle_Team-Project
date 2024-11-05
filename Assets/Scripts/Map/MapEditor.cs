using System;
using UnityEngine;



 
public class MapEditor : DestroySingleton<MapEditor>
{
    
    //MapEditor Fields
    private float tileSize;
    private GameObject container;
    
    //MapEditor Components 
    public MapEditInputController controller;
    public MapGenerator generator;
    public Builder builder;

    public GameObject mapContainer ;
    
    
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("초기화");
        controller = GetComponent<MapEditInputController>();
        generator = GetComponent<MapGenerator>();
        builder = GetComponent<Builder>();
        
        mapContainer = GameObject.Find("MapContainer");

        if (mapContainer == null)
        {
            mapContainer = new GameObject("MapContainer");
        }
    }
    
    
}

