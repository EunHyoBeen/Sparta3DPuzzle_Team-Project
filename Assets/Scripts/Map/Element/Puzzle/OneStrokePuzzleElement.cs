using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OneStrokePuzzleElement : MonoBehaviour, IPuzzleElement
{

    [SerializeField]private GameObject choicePuzzleTypeUI;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button enrollButton;

    private OneStrokeGrid strokeGrid;
    private int puzzleType;

    private void Awake()
    {
        strokeGrid = GetComponent<OneStrokeGrid>();
    }

    private void Start()
    {
        Debug.Log(puzzleType);
        enrollButton.onClick.AddListener(()=> EnrollPuzzleType());
    }

    public void InitializePuzzleElement()
    {
        choicePuzzleTypeUI.SetActive(true);
    }

    public void EnrollPuzzleType()
    {
        int.TryParse(inputField.text, out puzzleType);
        if (1 <= puzzleType && puzzleType <= 10)
        {
            PuzzleType type = (PuzzleType)(puzzleType - 1);
            strokeGrid.SetCurrentPuzzleType(type);
            choicePuzzleTypeUI.SetActive(false);
            return;
        }

        else
        {
            Debug.Log("올바른 번호를 입력해주세요.");
        }
    }
}