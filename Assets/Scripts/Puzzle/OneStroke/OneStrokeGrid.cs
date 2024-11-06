using System.Collections;
using UnityEngine;

public class OneStrokeGrid : MonoBehaviour, IInteractable
{
    private bool isClear = false;
    public int rows = 5;
    public int columns = 5;
    public GameObject cellPrefab; // �� ������
    public Transform generatorTransform; // ���� ���� ��ġ (�θ� ������Ʈ)
    public Camera mainCamera; // ���� ī�޶�
    public Camera puzzleCamera; // ���� ���� ī�޶�
    public Transform player; // �÷��̾� Transform �߰�

    private OneStrokeCell[,] grid;
    private OneStrokeCell previousCell = null;  // ������ �湮�� ���� ����
    private Vector3 cellScale; // ������ ������ ����
    private bool isDrawing = false; // ��ĥ ������ ����
    private bool inPuzzleView = false; // ���� ���� ������� ����

    private Vector3 playerInitialPosition;
    private Quaternion playerInitialRotation;

    private void Start()
    {
        cellScale = cellPrefab.transform.localScale;
        GenerateGrid();

        mainCamera.gameObject.SetActive(true);
        puzzleCamera.gameObject.SetActive(false);
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

        // �÷��̾� ��ġ�� ȸ�� ����
        playerInitialPosition = player.position;
        playerInitialRotation = player.rotation;

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

        // �÷��̾� ��ġ�� ȸ�� ����
        player.position = playerInitialPosition;
        player.rotation = playerInitialRotation;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator DrawPuzzle()
    {
        isDrawing = true;
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    OneStrokeCell cell = hit.collider.GetComponent<OneStrokeCell>();
                    if (cell != null && !cell.isVisited)
                    {
                        cell.VisitCell();
                        cell.GetComponent<Renderer>().material.color = Color.green;
                        CheckPuzzleClear();
                        isDrawing = true;
                    }
                }
            }
            else if (isDrawing)
            {
                ResetPuzzle();
                isDrawing = false;
            }
            yield return null;
        }
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
        Debug.Log("���� Ŭ����!");
        ExitPuzzleView();
    }
}
