using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public Equipment equip;  


    public ItemData itemData;
    public Action addItem;

    public event Action<bool> onPuzzleEvent;
    public Transform dropPosition;

    // Start is called before the first frame update
    void Awake()
    {
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
        controller = GetComponent<PlayerController>();
        CharacterManager.Instance.Player = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPuzzle(bool inPuzzleView)
    {
        onPuzzleEvent?.Invoke(inPuzzleView);
    }
}
