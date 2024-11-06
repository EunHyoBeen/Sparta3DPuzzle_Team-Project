using UnityEngine;

public class OneStrokePuzzleElement : PuzzleElement
{

    [SerializeField]private GameObject choicePuzzleTypeUI;
    
    
    public override void InitializePuzzleElement()
    {
        Debug.Log("UI 보이게 실행");
        choicePuzzleTypeUI.SetActive(true);
     }
    
}