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
    public MapDataManager dataManager;
    public Builder builder;

    public GameObject mapContainer ;
    
    
    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<MapEditInputController>();
        generator = GetComponent<MapGenerator>();
        dataManager = GetComponent<MapDataManager>();
        builder = GetComponent<Builder>();
        
        mapContainer = GameObject.Find("MapContainer");

        if (mapContainer == null)
        {
            mapContainer = new GameObject("MapContainer");
        }
    }

    private void Start()
    {
        
    }
    
    
}

