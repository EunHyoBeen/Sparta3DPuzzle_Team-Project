using UnityEngine;
    public class TestPuzzle : PuzzleControllerBase
    {
        //이전 기믹의 퍼즐이 클러어면 퍼불러옴 
        protected override void ActivatePuzzle()
        {
            base.ActivatePuzzle();
            Debug.Log("Test Puzzle 기믹 온");
        }
    }