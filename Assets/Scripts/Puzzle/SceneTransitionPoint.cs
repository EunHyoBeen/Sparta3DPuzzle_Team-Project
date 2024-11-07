using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionPoint : EndPoint
{
    [SerializeField] private string stageName;
    

    protected override void HandleLevelComplete()
    {
        DataManager.Instance.SetTopScene(stageName);
        DataManager.Instance.LoadTopScene();
    }
}