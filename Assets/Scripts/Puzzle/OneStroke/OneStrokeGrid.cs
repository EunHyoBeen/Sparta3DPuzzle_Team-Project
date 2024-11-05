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
    public GameObject cellPrefab; // 셀 프리팹
    public Transform generatorTransform; // 퍼즐 생성 위치 (부모 오브젝트)
    public Camera puzzleCamera; // 퍼즐 전용 카메라

    private OneStrokeCell[,] grid;
    private Vector3 cellScale; // 프리팹 스케일 저장

    private bool isDrawing = false; // 색칠 중인지 여부

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

                if (i > 0) // 위쪽 셀
                    currentCell.topCell = grid[i - 1, j];

                if (i < rows - 1) // 아래쪽 셀
                    currentCell.bottomCell = grid[i + 1, j];

                if (j > 0) // 왼쪽 셀
                    currentCell.leftCell = grid[i, j - 1];

                if (j < columns - 1) // 오른쪽 셀
                    currentCell.rightCell = grid[i, j + 1];

                // 경로 연결 상태 업데이트
                currentCell.ConnectPaths();
            }
        }
    }

    public string GetInteractPrompt()
    {
        if (isClear) return "";

        return "한붓그리기 퍼즐을 풀고 왼쪽의 상자를 열어보세요!";
    }

    public void OnInteract()
    {
        // 이미 깼으면 아무것도 하지 않음
        if (isClear) return;

        // 퍼즐 카메라로 카메라 교체
        mainCamera.gameObject.SetActive(false);
        puzzleCamera.gameObject.SetActive(true);

        // 커서 락 해제
        Cursor.lockState = CursorLockMode.None;

        // 드래그 색칠 시작
        StartCoroutine(DrawPuzzle());
    }

    private IEnumerator DrawPuzzle()
    {
        isDrawing = true;

        while (!isClear)
        {
            Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 마우스 클릭 여부 확인
            if (Input.GetMouseButton(0)) // 왼쪽 마우스 버튼 클릭
            {
                if (Physics.Raycast(ray, out hit))
                {
                    OneStrokeCell cell = hit.collider.GetComponent<OneStrokeCell>();
                    if (cell != null && !cell.isVisited)
                    {
                        // 색칠 로직 (셀을 방문 처리)
                        cell.VisitCell();
                        cell.GetComponent<Renderer>().material.color = Color.green; // 색칠하기 (원하는 색으로 변경 가능)

                        // 퍼즐 클리어 체크
                        CheckPuzzleClear();
                    }
                }
            }

            yield return null; // 다음 프레임까지 대기
        }

    }

    private void CheckPuzzleClear()
    {
        // 모든 셀이 방문되었는지 체크
        foreach (OneStrokeCell cell in grid)
        {
            if (!cell.isVisited)
                return; // 방문하지 않은 셀이 있으면 리턴
        }

        // 모든 셀이 방문되었다면 퍼즐 클리어
        isClear = true;
        Debug.Log("퍼즐 클리어!");

        // 퍼즐 클리어 시 필요한 추가 로직 (예: 상자 열기 등)
        puzzleCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
