using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePuzzle : MonoBehaviour
{
    [SerializeField] private Board puzzleBoard;
    // Start is called before the first frame update
    void Start()
    {
        puzzleBoard.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        puzzleBoard.gameObject.SetActive(true);
        CharacterManager.Instance.Player.OnPuzzle(false);
        puzzleBoard.StartCoroutine(puzzleBoard.OnStart());
    }
}
