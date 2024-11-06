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
    public Transform player; // 플레이어 Transform 추가

    private OneStrokeCell[,] grid;
    private OneStrokeCell previousCell = null;  // 이전에 방문한 셀을 저장
    private Vector3 cellScale; // 프리팹 스케일 저장
    private bool isDrawing = false; // 색칠 중인지 여부
    private bool inPuzzleView = false; // 현재 퍼즐 모드인지 여부

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

        // 플레이어 위치와 회전 고정
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

        // 플레이어 위치와 회전 복원
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
        Debug.Log("퍼즐 클리어!");
        ExitPuzzleView();
    }
}
