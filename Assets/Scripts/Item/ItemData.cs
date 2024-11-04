using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Readable,
    Useable
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

}