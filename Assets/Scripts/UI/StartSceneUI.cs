using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneUI : MonoBehaviour
{
    [Header("UI Componenets")] [SerializeField]
    private Button customModeStartButton;

    [SerializeField] private Button storyModeStartButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject customModePanel;
    [SerializeField] private GameObject storyModePanel;


    private void Start()
    {
        customModeStartButton.onClick.AddListener(OnCustomModeStart);
        storyModeStartButton.onClick.AddListener(OnStoryModeStart);
        settingButton.onClick.AddListener(OnSetting);
        quitButton.onClick.AddListener(OnQuit);
    }

    private void OnCustomModeStart()
    {
        storyModePanel.SetActive(false);
        customModePanel.SetActive(true);
    }


    private void OnStoryModeStart()
    {
        storyModePanel.SetActive(true);
        customModePanel.SetActive(false);
    }

    private void OnSetting()
    {
    }

    private void OnQuit()
    {
        Application.Quit();
    }


    public void OnContinueButton()
    {
        DataManager.Instance.LoadTopScene();
    }

    public void OnReStartButton()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void OnEditMapButton()
    {
        SceneManager.LoadScene("EditMapScene");
    }
    
    public void OnPlayCustomMapButton()
    {
        SceneManager.LoadScene("PlayCustomGameScene");
    }

}