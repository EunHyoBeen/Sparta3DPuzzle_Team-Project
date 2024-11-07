using UnityEngine;

public class PuzzleControllerBase : MonoBehaviour
{
    [Header("PuzzleController Settings")]
    [SerializeField] protected PuzzleType[] previousPuzzleTypes;
    [SerializeField] protected PuzzleType currentPuzzleType;

    public PuzzleType CurrentPuzzleType
    {
        get { return currentPuzzleType; }
    }

    public PuzzleType PreviousPuzzleType
    {
        get { return previousPuzzleTypes[0]; }
    }
    
    public void SetCurrentPuzzleType(PuzzleType type)
    {
            currentPuzzleType = type;
     }
    
    public void SetPreviousPuzzleType(PuzzleType type)
    {
        previousPuzzleTypes[0] = type;
     }
    
    protected virtual void OnEnable()
    {
        SubscribeToPreviousPuzzles();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeFromPreviousPuzzles();
    }

    protected virtual void PublishPuzzleClear()
    {
        StageEventBus.Instance?.Publish(currentPuzzleType);
    }

    protected virtual void ActivatePuzzle()
    {
        UnsubscribeFromPreviousPuzzles();
    }

    
    private void SubscribeToPreviousPuzzles()
    {
        if (previousPuzzleTypes.Length == 0) return;

        foreach (var puzzleType in previousPuzzleTypes)
        {
            StageEventBus.Instance.Subscribe(puzzleType, ActivatePuzzle);
        }
    }

    private void UnsubscribeFromPreviousPuzzles()
    {
        if (previousPuzzleTypes.Length == 0) return;

        foreach (var puzzleType in previousPuzzleTypes)
        {
            StageEventBus.Instance.UnSubscribe(puzzleType, ActivatePuzzle);
        }
    }
}