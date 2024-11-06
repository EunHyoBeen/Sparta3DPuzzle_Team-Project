using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayCustomGameUI : MonoBehaviour
{
    [SerializeField] private Transform mapDataLoadButtonContainer;
    
    private void Start()
    {
        MapEditor.Instance.generator.OnGenerateMap += HideUI;
        SetMapDataLoadButton();
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void SetMapDataLoadButton()
    {
        string mapDataDirectoryPath = Application.dataPath + "/MapData";

        // 폴더가 없을 경우 생성
        if (!Directory.Exists(mapDataDirectoryPath))
        {
            Directory.CreateDirectory(mapDataDirectoryPath);
        }

        // 폴더에서 파일 이름 가져오기
        string[] files = Directory.GetFiles(mapDataDirectoryPath, "*.json"); // JSON 파일만 가져오기
        Debug.Log($"파일 개수: {files.Length}");

        foreach (var filePath in files)
        {
            string mapDataName = Path.GetFileNameWithoutExtension(filePath); // 파일 이름만 추출
            Debug.Log(mapDataName);

            GameObject mapDataLoadButton = ObjectPool.Instance.GetObject();
            mapDataLoadButton.GetComponent<MapDataLoadButton>().Setting(mapDataName);
            mapDataLoadButton.transform.SetParent(mapDataLoadButtonContainer.transform);
        }
    }


}
 