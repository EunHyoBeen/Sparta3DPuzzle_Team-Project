
    using System;
    using UnityEngine;

    public class TestPubPuzzle :PuzzleControllerBase
    {
        private void OnCollisionEnter(Collision other)
        {
            PublishPuzzleClear();
        }
    }
