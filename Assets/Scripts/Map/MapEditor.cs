using System;
using UnityEngine;

public enum MapType
{
    Space,
    Dungeon,
    Maze
}

 
public class MapEditor : MonoBehaviour
{
    
    //MapEditor Fields
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private MapType type = MapType.Space;
    private float tileSize;
    private GameObject container;
    
    //MapEditor Components 
    public MapEditInputController controller;
    public MapGenerator generator;
    public MapDataManager dataManager;
    
    
    
    private void Awake()
    {
        container = new GameObject("Map Container");
        controller = GetComponent<MapEditInputController>();
        generator = GetComponent<MapGenerator>();
        dataManager = GetComponent<MapDataManager>();
        
    }

    private void Start()
    {
        generator.InitData(width, height, type,container);
        dataManager.Init(container);
        generator.GenerateDefaultMap();
        
            
        
    }
    
    
}

