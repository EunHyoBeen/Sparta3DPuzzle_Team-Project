using System.Collections;
using UnityEngine;

public class ChestPuzzle : PuzzleControllerBase
{
    private bool isOpen = false;
    public Transform lidTransform;    // ���� �Ѳ� Transform
    public float openAngle = 120f;
    public float openSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine openCoroutine;

    private void Start()
    {
        closedRotation = lidTransform.localRotation;
        openRotation = Quaternion.Euler(openAngle, 0, 0);
    }

    public void OnInteract()
    {
        // �̹� �������� �ƹ��͵� ���� ����
        if (isOpen) return;

        // ���� �ִϸ��̼� ����
        if (openCoroutine != null)
            StopCoroutine(openCoroutine);

        openCoroutine = StartCoroutine(RotateLid());
    }

    private IEnumerator RotateLid()
    {
        Quaternion targetRotation = openRotation;
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * openSpeed;
            lidTransform.localRotation = Quaternion.Slerp(lidTransform.localRotation, targetRotation, timeElapsed);
            yield return null;
        }

        lidTransform.localRotation = targetRotation;
        isOpen = true; // ���ڸ� ������
    }

    protected override void ActivatePuzzle()
    {
        base.ActivatePuzzle();
        OnInteract();
    }
}
