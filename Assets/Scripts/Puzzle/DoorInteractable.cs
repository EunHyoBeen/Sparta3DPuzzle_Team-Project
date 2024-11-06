using System.Collections;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public ItemData requiredKey; // 자물쇠를 여는 데 필요한 키
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
        inventory = CharacterManager.Instance.Player.inventory; // 플레이어 인벤토리 찾기
    }

    public string GetInteractPrompt()
    {
        // 문이 이미 열렸으면 "열 수 없다" 표시
        if (isOpen) return "";


        foreach (ItemSlot data in inventory.slots)
        {
            if (data.item == null)
            {
                return requiredKey.name + "가 필요합니다";
            }
            else if (data.item.displayName.Equals(requiredKey.displayName))
            {
                return "열 수 있습니다";
            }
        }

        return requiredKey.name + "가 필요합니다";
    }

    public void OnInteract()
    {
        // 이미 열렸으면 아무것도 하지 않음
        if (isOpen) return;

        foreach (ItemSlot data in inventory.slots)
        {
            if (data.item == null)
            {
                return;
            }
            else if (data.item.displayName.Equals(requiredKey.displayName))
            {
                // 열기 애니메이션 시작
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
        isOpen = true; // 문을 열었음
    }
}
