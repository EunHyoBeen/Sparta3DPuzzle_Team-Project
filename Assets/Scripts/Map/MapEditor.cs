using System;
using UnityEngine;

public enum MapType
{
    Space,
    Dungeon,
    Maze
}

 
public class MapEditor : Singleton<MapEditor>
{
    
    //MapEditor Fields
    private float tileSize;
    private GameObject container;
    
    //MapEditor Components 
    public MapEditInputController controller;
    public MapGenerator generator;
    public MapDataManager dataManager;
    
    
    
    
    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<MapEditInputController>();
        generator = GetComponent<MapGenerator>();
        dataManager = GetComponent<MapDataManager>();
    }

    private void Start()
    {
        dataManager.Init(container);
    }
    
    
}

