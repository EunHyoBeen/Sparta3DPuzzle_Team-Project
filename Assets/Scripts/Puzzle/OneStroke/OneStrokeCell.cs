using UnityEngine;

public class OneStrokeCell : MonoBehaviour
{
    public bool isStartPoint = false;
    public bool isEndPoint = false;
    public bool hasTopPath, hasBottomPath, hasLeftPath, hasRightPath;
    public bool isVisited = false;

    // �� ���� ��θ� �����ϴ� ���
    public OneStrokeCell topCell;
    public OneStrokeCell bottomCell;
    public OneStrokeCell leftCell;
    public OneStrokeCell rightCell;

    // ���� ����
    public Color visitedColor = Color.green; // �湮�� ���� ����
    private Color originalColor; // ���� ����

    private Renderer cellRenderer; // Renderer ������Ʈ�� ������ ����

    private void Start()
    {
        // �ʱ� ������ �����ϰ� Renderer ��������
        cellRenderer = GetComponent<Renderer>();
        if (cellRenderer != null)
        {
            originalColor = cellRenderer.material.color; // ���� ���� ����
        }
    }

    public bool VisitCell()
    {
        if (isVisited) return false;

        isVisited = true;

        // ���� ����
        ChangeColor(visitedColor);
        return true;
    }

    public void ResetVisit()
    {
        isVisited = false;

        // ���� ����
        ChangeColor(originalColor);
    }

    private void ChangeColor(Color newColor)
    {
        if (cellRenderer != null)
        {
            cellRenderer.material.color = newColor; // ���� ����
        }
    }

    public void ConnectPaths()
    {
        if (topCell != null) hasTopPath = true;
        if (bottomCell != null) hasBottomPath = true;
        if (leftCell != null) hasLeftPath = true;
        if (rightCell != null) hasRightPath = true;
    }
}
