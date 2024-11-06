using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapElement
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
    public PuzzleType type; 

    public MapElement(string name, Vector3 position, Quaternion rotation, Vector3 localScale, PuzzleType type = 0)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.localScale = localScale;
        this.type = type;
    }
}
