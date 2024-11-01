using UnityEngine;

public class PuzzleControllerBase : MonoBehaviour
{
    [Header("PuzzleController Settings")]
    [SerializeField] private PuzzleType subType;
    [SerializeField] private PuzzleType pubType;
    [SerializeField] private bool enableListener = false;
    [SerializeField] private bool enablePublisher = false;
    
    
    
    protected virtual void OnEnable()
    {
        if(StageEventBus.Instance == null)
                Debug.LogError("StageEventBus가 존재하지 않습니다.");
        
        if (enableListener)
            StageEventBus.Instance.Subscribe(subType, OnPuzzleClear);
    }
    
    
    protected virtual void OnDisable()
    {    
        if (enableListener)
            StageEventBus.Instance.UnSubscribe(subType, OnPuzzleClear);
    }
    
    
    protected virtual void PublishPuzzleClear()
    {
        if (enablePublisher)
        {
            StageEventBus.Instance.Publish(pubType);
        }
    }
    
    protected virtual void OnPuzzleClear()
    {
        
    }
}
