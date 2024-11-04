using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Transform lid;             // 상자의 뚜껑 부분 Transform
    public float openAngle = -120f;    // 뚜껑이 열릴 각도
    public float openSpeed = 2f;      // 뚜껑이 열리는 속도
    private bool isOpen = false;      // 상자가 열렸는지 여부

    // 자물쇠가 올바른 열쇠로 풀릴 때 호출될 메서드
    public void Open()
    {
        if (!isOpen)
        {
            StartCoroutine(OpenLid());
        }
    }

    // 뚜껑을 여는 코루틴
    private IEnumerator OpenLid()
    {
        isOpen = true;
        Quaternion initialRotation = lid.localRotation;
        Quaternion targetRotation = Quaternion.Euler(openAngle, 0, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * openSpeed;
            lid.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            yield return null;
        }

        lid.localRotation = targetRotation;
    }
}
