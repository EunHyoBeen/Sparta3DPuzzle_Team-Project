using UnityEngine;
using System.Collections;

public class OpenLid : MonoBehaviour
{
    public Transform lid; // 상자의 뚜껑 오브젝트
    private bool isOpen = false; // 뚜껑 상태 확인
    private float openAngle = 120f; // 열릴 각도
    private float closedAngle = 0f; // 닫힐 각도
    private float speed = 2f; // 회전 속도

    public void ToggleLid()
    {
        if (isOpen)
        {
            StartCoroutine(RotateLid(closedAngle)); // 닫기
        }
        else
        {
            StartCoroutine(RotateLid(openAngle)); // 열기
        }
        isOpen = !isOpen; // 상태 변경
    }

    private IEnumerator RotateLid(float targetAngle)
    {
        Quaternion targetRotation = Quaternion.Euler(targetAngle, 0, 0); // 목표 각도
        while (Quaternion.Angle(lid.localRotation, targetRotation) > 0.1f) // 목표 각도에 도달할 때까지
        {
            lid.localRotation = Quaternion.Slerp(lid.localRotation, targetRotation, Time.deltaTime * speed);
            yield return null; // 다음 프레임까지 대기
        }
        lid.localRotation = targetRotation; // 정확하게 목표 각도로 맞춤
    }
}
