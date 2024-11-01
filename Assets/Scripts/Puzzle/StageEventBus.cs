using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

//구독자인 경우 , 몇번 퍼즐로부터 정보를 받아올 것 인지
//발행자인 경우 , 몇번 퍼즐로서 정보를 줄 것인지 

public enum PuzzleType
{
    Puzzle1, 
    Puzzle2,
    Puzzle3,
    Puzzle4,
    Puzzle5,
    Puzzle6,
    Puzzle7,
    Puzzle8,
    Puzzle9,
    Puzzle10
}


public class StageEventBus : DestroySingleton<StageEventBus>
{
    private readonly Dictionary<PuzzleType, UnityEvent> stageEvents = new Dictionary<PuzzleType, UnityEvent>();
    
    public void Subscribe(PuzzleType puzzleType, UnityAction listener)
    {
        if (stageEvents.TryGetValue(puzzleType, out var thisEvent))
        {
            //동일 UnityAction 중복 구독 방지
            if (!thisEvent.GetPersistentEventCount().Equals(0))
                thisEvent.AddListener(listener);
        }

        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            stageEvents.Add(puzzleType, thisEvent);
        }
    }

    public void UnSubscribe(PuzzleType puzzleType, UnityAction listener)
    {
        if (stageEvents.TryGetValue(puzzleType, out var thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }


    public void Publish(PuzzleType puzzleType)
    {
        if (stageEvents.TryGetValue(puzzleType, out var thisEvent))
        {
            thisEvent?.Invoke();
        }
    }
}