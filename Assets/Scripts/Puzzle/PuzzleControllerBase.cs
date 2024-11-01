using UnityEngine;

public class PuzzleControllerBase : MonoBehaviour
{
    [Header("PuzzleController Settings")]
    [SerializeField] private PuzzleType[] previousPuzzleTypes;
    [SerializeField] private PuzzleType currentPuzzleType;

    private void Awake()
    {
        if (StageEventBus.Instance == null)
        {
            Debug.LogError("StageEventBus가 존재하지 않습니다.");
        }
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