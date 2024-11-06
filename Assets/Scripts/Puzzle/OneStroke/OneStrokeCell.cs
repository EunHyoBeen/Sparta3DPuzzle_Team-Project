using UnityEngine;

public class OneStrokeCell : MonoBehaviour
{
    public bool isVisited = false;

    // ���� ����
    public Color visitedColor = Color.green; // �湮�� ���� ����
    private Color originalColor; // ���� ����

    private Renderer cellRenderer; // Renderer ������Ʈ�� ������ ����

    // ���� ��, ��, ��, �� �� ����
    public OneStrokeCell topCell;
    public OneStrokeCell bottomCell;
    public OneStrokeCell leftCell;
    public OneStrokeCell rightCell;

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
}
