using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;


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