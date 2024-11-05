using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class OneStrokeGrid : MonoBehaviour, IInteractable
{
    private Camera mainCamera;
    private bool isClear = false;
    public int rows = 5;
    public int columns = 5;
    public GameObject cellPrefab; // �� ������
    public Transform generatorTransform; // ���� ���� ��ġ (�θ� ������Ʈ)
    public Camera puzzleCamera; // ���� ���� ī�޶�

    private OneStrokeCell[,] grid;
    private Vector3 cellScale; // ������ ������ ����

    private bool isDrawing = false; // ��ĥ ������ ����

    private void Start()
    {
        cellScale = cellPrefab.transform.localScale;
        GenerateGrid();
        ConnectCells();

        mainCamera = Camera.main;
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
                cellObject.transform.localPosition = new Vector3(
                    0,
                    i * cellScale.y,
                    j * cellScale.z
                );

                grid[i, j] = cellObject.GetComponent<OneStrokeCell>();
            }
        }

        Destroy(grid[2, 1].gameObject);
        Destroy(grid[2, 0].gameObject);
        Destroy(grid[4, 2].gameObject);
        Destroy(grid[1, 3].gameObject);
    }

    private void ConnectCells()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                OneStrokeCell currentCell = grid[i, j];

                if (i > 0) // ���� ��
                    currentCell.topCell = grid[i - 1, j];

                if (i < rows - 1) // �Ʒ��� ��
                    currentCell.bottomCell = grid[i + 1, j];

                if (j > 0) // ���� ��
                    currentCell.leftCell = grid[i, j - 1];

                if (j < columns - 1) // ������ ��
                    currentCell.rightCell = grid[i, j + 1];

                // ��� ���� ���� ������Ʈ
                currentCell.ConnectPaths();
            }
        }
    }

    public string GetInteractPrompt()
    {
        if (isClear) return "";

        return "�Ѻױ׸��� ������ Ǯ�� ������ ���ڸ� �������!";
    }

    public void OnInteract()
    {
        // �̹� ������ �ƹ��͵� ���� ����
        if (isClear) return;

        // ���� ī�޶�� ī�޶� ��ü
        mainCamera.gameObject.SetActive(false);
        puzzleCamera.gameObject.SetActive(true);

        // Ŀ�� �� ����
        Cursor.lockState = CursorLockMode.None;

        // �巡�� ��ĥ ����
        StartCoroutine(DrawPuzzle());
    }

    private IEnumerator DrawPuzzle()
    {
        isDrawing = true;

        while (!isClear)
        {
            Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���콺 Ŭ�� ���� Ȯ��
            if (Input.GetMouseButton(0)) // ���� ���콺 ��ư Ŭ��
            {
                if (Physics.Raycast(ray, out hit))
                {
                    OneStrokeCell cell = hit.collider.GetComponent<OneStrokeCell>();
                    if (cell != null && !cell.isVisited)
                    {
                        // ��ĥ ���� (���� �湮 ó��)
                        cell.VisitCell();
                        cell.GetComponent<Renderer>().material.color = Color.green; // ��ĥ�ϱ� (���ϴ� ������ ���� ����)

                        // ���� Ŭ���� üũ
                        CheckPuzzleClear();
                    }
                }
            }

            yield return null; // ���� �����ӱ��� ���
        }

    }

    private void CheckPuzzleClear()
    {
        // ��� ���� �湮�Ǿ����� üũ
        foreach (OneStrokeCell cell in grid)
        {
            if (!cell.isVisited)
                return; // �湮���� ���� ���� ������ ����
        }

        // ��� ���� �湮�Ǿ��ٸ� ���� Ŭ����
        isClear = true;
        Debug.Log("���� Ŭ����!");

        // ���� Ŭ���� �� �ʿ��� �߰� ���� (��: ���� ���� ��)
        puzzleCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
