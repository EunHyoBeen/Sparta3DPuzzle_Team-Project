using System.Collections;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    public Key requiredKey;           // 문을 열기 위해 필요한 키 (null이면 키 필요 없음)
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
        playerInventory = FindObjectOfType<PlayerInventory>(); // 플레이어 인벤토리 찾기
    }

    public string GetInteractPrompt()
    {
        // 문이 이미 열렸으면 "열 수 없다" 표시
        if (isOpen) return "";

        // 자물쇠가 없는 경우 바로 열 수 있음
        if (requiredKey == null) return "열 수 있습니다";

        // 자물쇠가 있는 경우 키가 있는지 확인
        return playerInventory != null && playerInventory.HasKey(requiredKey) ? "열 수 있습니다" : "GrayKey가 필요합니다";
    }

    public void OnInteract()
    {
        // 이미 열렸으면 아무것도 하지 않음
        if (isOpen) return;

        // 자물쇠가 있는 경우, 플레이어가 해당 키를 소지해야 열 수 있음
        if (requiredKey != null && (playerInventory == null || !playerInventory.HasKey(requiredKey))) return;

        // 열기 애니메이션 시작
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
        isOpen = true; // 문을 열었음
    }
}
