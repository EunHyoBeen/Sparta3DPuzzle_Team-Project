using System.Collections;
using UnityEngine;

public class ChestInteractable : PuzzleControllerBase, IInteractable
{
    private bool isOpen = false;
    public Lock chestLock;           // 자물쇠 참조 (null일 경우 자물쇠 없음)
    public Transform lidTransform;    // 상자 뚜껑 Transform
    public float openAngle = -120f;
    public float openSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine openCoroutine;

    private void Start()
    {
        closedRotation = lidTransform.localRotation;
        openRotation = Quaternion.Euler(openAngle, 0, 0);
    }

    public string GetInteractPrompt()
    {
        if (isOpen) return "";

        // 자물쇠가 없는 경우 바로 열 수 있음
        if (chestLock == null) return "열 수 있습니다";

        // 자물쇠가 있는 경우 자물쇠가 열렸는지 확인
        return chestLock.IsUnlocked() || chestLock.CanUnlock() ? "열 수 있습니다" : chestLock.requiredKey.name + "가 필요합니다";
    }

    public void OnInteract()
    {
        // 이미 열렸으면 아무것도 하지 않음
        if (isOpen) return;

        // 자물쇠가 있는 경우 자물쇠가 열려 있어야 함
        if (chestLock != null && !chestLock.TryUnlock()) return;

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

        // 사운드 추가(상자 여는 사운드)

        isOpen = true; // 상자를 열었음
    }

    protected override void ActivatePuzzle()
    {
        base.ActivatePuzzle();
        Debug.Log(gameObject.name);
    }
}
