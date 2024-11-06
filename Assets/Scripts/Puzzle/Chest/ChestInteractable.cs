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

        // �ڹ��谡 ���� ��� �ٷ� �� �� ����
        if (chestLock == null) return "�� �� �ֽ��ϴ�";

        // �ڹ��谡 �ִ� ��� �ڹ��谡 ���ȴ��� Ȯ��
        return chestLock.IsUnlocked() || chestLock.CanUnlock() ? "�� �� �ֽ��ϴ�" : chestLock.requiredKey.name + "�� �ʿ��մϴ�";
    }

    public void OnInteract()
    {
        // �̹� �������� �ƹ��͵� ���� ����
        if (isOpen) return;

        // �ڹ��谡 �ִ� ��� �ڹ��谡 ���� �־�� ��
        if (chestLock != null && !chestLock.TryUnlock()) return;

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

        // ���� �߰�(���� ���� ����)

        isOpen = true; // ���ڸ� ������
    }

    protected override void ActivatePuzzle()
    {
        base.ActivatePuzzle();
        Debug.Log(gameObject.name);
    }
}
