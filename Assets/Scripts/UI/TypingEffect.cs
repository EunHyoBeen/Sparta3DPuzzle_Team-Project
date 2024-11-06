using System.Collections;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    private TextMeshProUGUI textComponent; // TextMeshProUGUI 컴포넌트
    [SerializeField] private string fullText = "This is the text that will appear with a typing effect."; // 전체 텍스트
    [SerializeField] private float typingTime = 0.05f; // 한 글자당 출력 속도 (초 단위)

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        // 타이핑 효과 시작
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = ""; // 초기화하여 빈 텍스트로 시작
        foreach (char letter in fullText)
        {
            textComponent.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingTime); // 다음 글자 출력 전 대기
        }
    }
}
