using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneUI : MonoBehaviour
{
    [SerializeField] private Button  customModeStartButton;  
    [SerializeField] private Button  storyModeStartButton;  
    [SerializeField] private Button  settingButton;  
    [SerializeField] private Button  exitButton;  
    
    
    private void Start()
    {
        customModeStartButton.onClick.AddListener(OnCustomModeStart);    
        storyModeStartButton.onClick.AddListener(OnStoryModeStart);    
        settingButton.onClick.AddListener(OnSetting);    
        exitButton.onClick.AddListener(OnExit);    
    }

    private void OnCustomModeStart()
    {
        
    }


    private void OnStoryModeStart()
    {
        
    }

    private void OnSetting()
    {
        
    }

    private void OnExit()
    {
        Application.Quit();
    }
}
