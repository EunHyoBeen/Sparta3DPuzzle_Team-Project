using System.Collections;
using UnityEngine;

public class OneStrokeGrid : MonoBehaviour, IInteractable
{
    private bool isClear = false;
    public int rows = 5;
    public int columns = 5;
    public GameObject cellPrefab; // 셀 프리팹
    public Transform generatorTransform; // 퍼즐 생성 위치 (부모 오브젝트)
    public Camera mainCamera; // 메인 카메라
    public Camera puzzleCamera; // 퍼즐 전용 카메라
    public Rigidbody playerRb; // 플레이어 Rigidbody 추가

    private OneStrokeCell[,] grid;
    private OneStrokeCell previousCell = null;  // 이전에 방문한 셀을 저장
    private Vector3 cellScale; // 프리팹 스케일 저장
    private bool isDrawing = false; // 색칠 중인지 여부
    private bool inPuzzleView = false; // 현재 퍼즐 모드인지 여부

    //private Vector3 playerInitialPosition;
    //private Quaternion playerInitialRotation;

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

        // 셀들 간의 연결 설정
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i > 0) grid[i, j].topCell = grid[i - 1, j]; // 위쪽 셀 연결
                if (i < rows - 1) grid[i, j].bottomCell = grid[i + 1, j]; // 아래쪽 셀 연결
                if (j > 0) grid[i, j].leftCell = grid[i, j - 1]; // 왼쪽 셀 연결
                if (j < columns - 1) grid[i, j].rightCell = grid[i, j + 1]; // 오른쪽 셀 연결
            }
        }

        // 일부 셀 삭제 (예시)
        Destroy(grid[2, 1].gameObject);
        Destroy(grid[2, 0].gameObject);
        Destroy(grid[4, 2].gameObject);
        Destroy(grid[1, 3].gameObject);
    }

    public string GetInteractPrompt()
    {
        if (isClear) return "이미 클리어한 퍼즐입니다";
        return inPuzzleView ? "" : "한붓그리기 퍼즐을 풀고 상자를 열어보세요!";
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

        playerRb.isKinematic = true;

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

        playerRb.isKinematic = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator DrawPuzzle()
    {
        isDrawing = true;
        while (true)
        {
            if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼을 누른 상태
            {
                Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition); // 퍼즐 카메라에서 레이 캐스트
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    OneStrokeCell cell = hit.collider.GetComponent<OneStrokeCell>(); // 맞은 셀 찾기
                    if (cell != null && !cell.isVisited && IsNeighborCell(previousCell, cell)) // 방문하지 않은 셀이며 이전 셀과 이웃한 셀인지 확인
                    {
                        cell.VisitCell();
                        cell.GetComponent<Renderer>().material.color = Color.green;
                        CheckPuzzleClear();

                        previousCell = cell; // 현재 셀을 이전 셀로 업데이트
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

    // 이전 셀과 현재 셀이 1칸 이상 떨어져 있는지 확인하는 메서드
    private bool IsNeighborCell(OneStrokeCell fromCell, OneStrokeCell toCell)
    {
        if (fromCell == null) return true; // 처음 셀은 방문을 허용

        // 이전 셀과 현재 셀의 연결 관계를 확인: 상하좌우 인접 셀 중 하나인지 체크
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
        Debug.Log("퍼즐 클리어!");
        ExitPuzzleView();
    }
}
