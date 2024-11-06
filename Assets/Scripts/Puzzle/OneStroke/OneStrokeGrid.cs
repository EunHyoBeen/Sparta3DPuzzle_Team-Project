using System.Collections;
using UnityEngine;

public class OneStrokeGrid : PuzzleControllerBase, IInteractable
{
    private bool isClear = false;
    public int rows = 5;
    public int columns = 5;
    public GameObject cellPrefab; // �� ������
    public Transform generatorTransform; // ���� ���� ��ġ (�θ� ������Ʈ)
    public Camera mainCamera; // ���� ī�޶�
    public Camera puzzleCamera; // ���� ���� ī�޶�

    private OneStrokeCell[,] grid;
    private OneStrokeCell previousCell = null;  // ������ �湮�� ���� ����
    private Vector3 cellScale; // ������ ������ ����
    private bool isDrawing = false; // ��ĥ ������ ����
    private bool inPuzzleView = false; // ���� ���� ������� ����

    private void Start()
    {
        mainCamera = Camera.main;
        cellScale = cellPrefab.transform.localScale;
        GenerateGrid();

        mainCamera.gameObject.SetActive(true);
        puzzleCamera.gameObject.SetActive(false);
    }

    public void SetCurrentPuzzleType(PuzzleType type)
    {
        currentPuzzleType = type;
    }

    private void GenerateGrid()
    {
        grid = new OneStrokeCell[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject cellObject = Instantiate(cellPrefab, generatorTransform);
                cellObject.transform.localScale = cellScale;
                cellObject.transform.localPosition = new Vector3(0, i * cellScale.y, j * cellScale.z);
                grid[i, j] = cellObject.GetComponent<OneStrokeCell>();
            }
        }

        // ���� ���� ���� ����
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i > 0) grid[i, j].topCell = grid[i - 1, j]; // ���� �� ����
                if (i < rows - 1) grid[i, j].bottomCell = grid[i + 1, j]; // �Ʒ��� �� ����
                if (j > 0) grid[i, j].leftCell = grid[i, j - 1]; // ���� �� ����
                if (j < columns - 1) grid[i, j].rightCell = grid[i, j + 1]; // ������ �� ����
            }
        }

        // �Ϻ� �� ���� (����)
        Destroy(grid[2, 1].gameObject);
        Destroy(grid[2, 0].gameObject);
        Destroy(grid[4, 2].gameObject);
        Destroy(grid[1, 3].gameObject);
    }

    public string GetInteractPrompt()
    {
        if (isClear) return "�̹� Ŭ������ �����Դϴ�";
        return inPuzzleView ? "" : "�Ѻױ׸��� ������ Ǯ�� ���ڸ� �������!";
    }

    public void OnInteract()
    {
        if (isClear) return;
        if (inPuzzleView) ExitPuzzleView();
        else EnterPuzzleView();
    }

    private void EnterPuzzleView()
    {
        inPuzzleView = true;
        mainCamera.gameObject.SetActive(false);
        puzzleCamera.gameObject.SetActive(true);

        CharacterManager.Instance.Player.OnPuzzle(!inPuzzleView);

        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(DrawPuzzle());
    }

    private void ExitPuzzleView()
    {
        inPuzzleView = false;
        mainCamera.gameObject.SetActive(true);
        puzzleCamera.gameObject.SetActive(false);

        StopCoroutine(DrawPuzzle());
        ResetPuzzle();

        CharacterManager.Instance.Player.OnPuzzle(!inPuzzleView);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator DrawPuzzle()
    {
        isDrawing = true;
        while (true)
        {
            if (Input.GetMouseButton(0)) // ���콺 ���� ��ư�� ���� ����
            {
                Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition); // ���� ī�޶󿡼� ���� ĳ��Ʈ
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    OneStrokeCell cell = hit.collider.GetComponent<OneStrokeCell>(); // ���� �� ã��
                    if (cell != null && !cell.isVisited && IsNeighborCell(previousCell, cell)) // �湮���� ���� ���̸� ���� ���� �̿��� ������ Ȯ��
                    {
                        cell.VisitCell();
                        cell.GetComponent<Renderer>().material.color = Color.green;
                        CheckPuzzleClear();

                        previousCell = cell; // ���� ���� ���� ���� ������Ʈ
                        isDrawing = true;
                    }
                }
            }
            else if (isDrawing)
            {
                ResetPuzzle();
                previousCell = null;
                isDrawing = false;
            }
            yield return null;
        }
    }

    // ���� ���� ���� ���� 1ĭ �̻� ������ �ִ��� Ȯ���ϴ� �޼���
    private bool IsNeighborCell(OneStrokeCell fromCell, OneStrokeCell toCell)
    {
        if (fromCell == null) return true; // ó�� ���� �湮�� ���

        // ���� ���� ���� ���� ���� ���踦 Ȯ��: �����¿� ���� �� �� �ϳ����� üũ
        return fromCell.topCell == toCell || fromCell.bottomCell == toCell ||
               fromCell.leftCell == toCell || fromCell.rightCell == toCell;
    }

    private void ResetPuzzle()
    {
        foreach (OneStrokeCell cell in grid)
        {
            if (cell != null)
            {
                cell.ResetVisit();
                cell.GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }

    private void CheckPuzzleClear()
    {
        foreach (OneStrokeCell cell in grid)
        {
            if (cell != null && !cell.isVisited) return;
        }
        isClear = true;
        PublishPuzzleClear();
        Debug.Log("���� Ŭ����!");
        ExitPuzzleView();
    }
}
