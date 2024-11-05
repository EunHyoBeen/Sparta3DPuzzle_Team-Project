using System.Collections;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    public Key requiredKey;           // ���� ���� ���� �ʿ��� Ű (null�̸� Ű �ʿ� ����)
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine openCoroutine;
    private PlayerInventory playerInventory;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + openAngle, transform.eulerAngles.z);
        playerInventory = FindObjectOfType<PlayerInventory>(); // �÷��̾� �κ��丮 ã��
    }

    public string GetInteractPrompt()
    {
        // ���� �̹� �������� "�� �� ����" ǥ��
        if (isOpen) return "";

        // �ڹ��谡 ���� ��� �ٷ� �� �� ����
        if (requiredKey == null) return "�� �� �ֽ��ϴ�";

        // �ڹ��谡 �ִ� ��� Ű�� �ִ��� Ȯ��
        return playerInventory != null && playerInventory.HasKey(requiredKey) ? "�� �� �ֽ��ϴ�" : "GrayKey�� �ʿ��մϴ�";
    }

    public void OnInteract()
    {
        // �̹� �������� �ƹ��͵� ���� ����
        if (isOpen) return;

        // �ڹ��谡 �ִ� ���, �÷��̾ �ش� Ű�� �����ؾ� �� �� ����
        if (requiredKey != null && (playerInventory == null || !playerInventory.HasKey(requiredKey))) return;

        // ���� �ִϸ��̼� ����
        if (openCoroutine != null)
            StopCoroutine(openCoroutine);

        openCoroutine = StartCoroutine(RotateDoor());
    }

    private IEnumerator RotateDoor()
    {
        Quaternion targetRotation = openRotation;
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * openSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, timeElapsed);
            yield return null;
        }

        transform.rotation = targetRotation;
        isOpen = true; // ���� ������
    }
}
