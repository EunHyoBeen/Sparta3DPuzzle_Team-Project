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
    
    
    //발행번호 메서드로 변경하는 경우는 연쇄작용 같은 곳에서 이뤄지는게 좋음. , 지금 한 방식은 그냥 똑같은 동작을 멀리 돌아서 한 거임.
    // SET이 복잡해서 코드가 길어지면 메서드를 추출해서 하는 경우는 있음. 그런 경우 아니면 일반 프로퍼티로 쓰면 됨 
    public void SetCurrentPuzzleType(PuzzleType type)
    {
            currentPuzzleType = type;
    }
    

    
    //구독번호 이전 번호 3번을 구독하게함 /  1번을 본인퍼즐 발행하게함. 
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
    
    //발행 번호(3) 구독 번호(1)  
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