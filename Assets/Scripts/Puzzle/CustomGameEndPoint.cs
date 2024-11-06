using UnityEngine;

public class CustomGameEndPoint : EndPoint
{
    protected override void HandleLevelComplete()
    {
        Debug.Log("클리어");
    }
}