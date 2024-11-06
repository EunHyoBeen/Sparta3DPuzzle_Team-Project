﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour
{
    [SerializeField] Button saveButton;
    [SerializeField] Button cancelButton;
    [SerializeField] private TMP_InputField mapNameInputField;


    private string mapName;
    
    private void Start()
    {
        InitSaveUIButton();
    }

    private void InitSaveUIButton()
    {
        cancelButton.onClick.AddListener(OnCancelButton);
        saveButton.onClick.AddListener(OnSaveButton);
    }

    
    private void OnCancelButton()
    {
        gameObject.SetActive(false);
    }
    
    private void OnSaveButton()
    {
        mapName = mapNameInputField.text;
        
        if(String.IsNullOrEmpty(mapName))
            return;
        
        MapDataManager.Instance.SaveMapData(mapName);  
        gameObject.SetActive(false);
    }

}