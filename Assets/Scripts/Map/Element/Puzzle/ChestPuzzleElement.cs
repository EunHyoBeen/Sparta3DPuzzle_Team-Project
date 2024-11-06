using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestPuzzleElement : MonoBehaviour, IPuzzleElement
{
    [SerializeField]private GameObject choicePreviousPuzzleTypeUI;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button enrollButton;
   
    private ChestPuzzle chestPuzzle;
    private int puzzleType;

    
    private void Awake()
    {
        chestPuzzle = GetComponent<ChestPuzzle>();
    }

    private void Start()
    {
        Debug.Log(puzzleType);
        enrollButton.onClick.AddListener(()=> EnrollPuzzleType());
    }

    public void InitializePuzzleElement()
    {
        choicePreviousPuzzleTypeUI.SetActive(true);
    }

    public void EnrollPuzzleType()
    {
        int.TryParse(inputField.text, out puzzleType);
        if (1 <= puzzleType && puzzleType <= 10)
        {
            PuzzleType type = (PuzzleType)(puzzleType - 1);
            chestPuzzle.SetPreviousPuzzleType(type);
            choicePreviousPuzzleTypeUI.SetActive(false);
            return;
        }

        else
        {
            Debug.Log("올바른 번호를 입력해주세요.");
        }
    }
}