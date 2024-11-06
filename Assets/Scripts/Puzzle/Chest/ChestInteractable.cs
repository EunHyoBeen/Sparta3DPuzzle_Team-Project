using System.Collections;
using UnityEngine;

public class ChestInteractable : PuzzleControllerBase, IInteractable
{
    private bool isOpen = false;
    public Lock chestLock;           // �ڹ��� ���� (null�� ��� �ڹ��� ����)
    public Transform lidTransform;    // ���� �Ѳ� Transform
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


        if (chestLock == null) return "열 수 있습니다";

        return chestLock.IsUnlocked() || chestLock.CanUnlock() ? "열 수 있습니다" : chestLock.requiredKey.name + "가 필요합니다";    }

    public void OnInteract()
    {
        if (isOpen) return;

        if (chestLock != null && !chestLock.TryUnlock()) return;

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


        isOpen = true; 
    }

    protected override void ActivatePuzzle()
    {
        base.ActivatePuzzle();
        Debug.Log(gameObject.name);
    }
}
