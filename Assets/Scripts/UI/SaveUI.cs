using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour
{
    [SerializeField] Button saveButton;
    [SerializeField] Button cancelButton;
    [SerializeField] private TMP_InputField mapNameInputField;
    [SerializeField] private GameObject AlertPanel;

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

        if (!GameObject.Find("PlayerPosIndicator(Clone)") || !GameObject.Find("PlayerPosIndicator(Clone)"))
        {
            AlertPanel.SetActive(true);
            return;
        }

        if (String.IsNullOrEmpty(mapName))
            return;
        MapDataManager.Instance.SaveMapData(mapName);
        gameObject.SetActive(false);
    }
}