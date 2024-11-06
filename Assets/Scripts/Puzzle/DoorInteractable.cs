using System.Collections;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public ItemData requiredKey; // �ڹ��踦 ���� �� �ʿ��� Ű
    private bool isOpen = false;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    private UIInventory inventory;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine openCoroutine;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + openAngle, transform.eulerAngles.z);
        inventory = CharacterManager.Instance.Player.inventory; // �÷��̾� �κ��丮 ã��
    }

    public string GetInteractPrompt()
    {
        // ���� �̹� �������� "�� �� ����" ǥ��
        if (isOpen) return "";


        foreach (ItemSlot data in inventory.slots)
        {
            if (data.item == null)
            {
                return requiredKey.name + "�� �ʿ��մϴ�";
            }
            else if (data.item.displayName.Equals(requiredKey.displayName))
            {
                return "�� �� �ֽ��ϴ�";
            }
        }

        return requiredKey.name + "�� �ʿ��մϴ�";
    }

    public void OnInteract()
    {
        // �̹� �������� �ƹ��͵� ���� ����
        if (isOpen) return;

        foreach (ItemSlot data in inventory.slots)
        {
            if (data.item == null)
            {
                return;
            }
            else if (data.item.displayName.Equals(requiredKey.displayName))
            {
                // ���� �ִϸ��̼� ����
                if (openCoroutine != null)
                    StopCoroutine(openCoroutine);

                openCoroutine = StartCoroutine(RotateDoor());
                break;
            }
        }        
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
