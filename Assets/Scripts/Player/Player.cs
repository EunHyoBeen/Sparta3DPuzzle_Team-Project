using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        CharacterManager.Instance.Player = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
