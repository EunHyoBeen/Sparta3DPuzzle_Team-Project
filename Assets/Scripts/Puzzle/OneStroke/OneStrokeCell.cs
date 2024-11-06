using UnityEngine;

public class OneStrokeCell : MonoBehaviour
{
    public bool isVisited = false;


    // 색상 설정
    public Color visitedColor = Color.green; // 방문한 셀의 색상
    private Color originalColor; // 원래 색상

    private Renderer cellRenderer; // Renderer 컴포넌트를 저장할 변수

    private void Start()
    {
        // 초기 색상을 저장하고 Renderer 가져오기
        cellRenderer = GetComponent<Renderer>();
        if (cellRenderer != null)
        {
            originalColor = cellRenderer.material.color; // 원래 색상 저장
        }
    }

    public bool VisitCell()
    {
        if (isVisited) return false;

        isVisited = true;

        // 색상 변경
        ChangeColor(visitedColor);
        return true;
    }

    public void ResetVisit()
    {
        isVisited = false;

        // 색상 복원
        ChangeColor(originalColor);
    }

    private void ChangeColor(Color newColor)
    {
        if (cellRenderer != null)
        {
            cellRenderer.material.color = newColor; // 색상 변경
        }
    }
}
