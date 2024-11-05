using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Key> keys = new List<Key>();

    public bool HasKey(Key key)
    {
        return keys.Contains(key);
    }
}
