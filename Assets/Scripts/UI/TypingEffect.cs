using System.Collections;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    private TextMeshProUGUI textComponent; // TextMeshProUGUI ������Ʈ
    [SerializeField] private string fullText = "This is the text that will appear with a typing effect."; // ��ü �ؽ�Ʈ
    [SerializeField] private float typingTime = 0.05f; // �� ���ڴ� ��� �ӵ� (�� ����)

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        // Ÿ���� ȿ�� ����
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = ""; // �ʱ�ȭ�Ͽ� �� �ؽ�Ʈ�� ����
        foreach (char letter in fullText)
        {
            textComponent.text += letter; // �� ���ھ� �߰�
            yield return new WaitForSeconds(typingTime); // ���� ���� ��� �� ���
        }
    }
}
