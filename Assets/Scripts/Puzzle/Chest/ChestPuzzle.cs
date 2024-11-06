using System.Collections;
using UnityEngine;

public class ChestPuzzle : PuzzleControllerBase
{
    private bool isOpen = false;
    public Transform lidTransform;    // 상자 뚜껑 Transform
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
        // 이미 열렸으면 아무것도 하지 않음
        if (isOpen) return;

        // 열기 애니메이션 시작
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
        isOpen = true; // 상자를 열었음
    }

    protected override void ActivatePuzzle()
    {
        base.ActivatePuzzle();
        OnInteract();
    }
}
