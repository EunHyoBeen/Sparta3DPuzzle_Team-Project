using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    public ItemData itemData;
    public Action addItem;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<PlayerController>();
        CharacterManager.Instance.Player = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
