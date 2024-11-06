using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionPoint : EndPoint
{
    [SerializeField] private string stageName;
    private int nextSceneIndex; 

    private void Start()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
           Debug.LogError("다음 씬이 존재하는지 확인 해주십시오.");
           nextSceneIndex = SceneManager.GetActiveScene().buildIndex;  
        }
    }

    protected override void HandleLevelComplete()
    {
        DataManager.Instance.SetTopScene(stageName);
        DataManager.Instance.LoadTopScene();
    }
}