
    using System;
    using UnityEngine;

    //clear하면 클리어했다고 정보 뿌리는 놈 
    public class TestPubPuzzle :PuzzleControllerBase
    {
        private void OnCollisionEnter(Collision other)
        {
            // 클리어 했다고 판단이 되면 요놈을 호출 
            PublishPuzzleClear();
        }
    }
