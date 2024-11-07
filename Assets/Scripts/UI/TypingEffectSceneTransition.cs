using System;
using System.Collections;
using TMPro;
using UnityEngine;

 
public class TypingEffectSceneTransition : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    [SerializeField] private string fullText = "당신은 꿈 속에 갇혔습니다.\n여기서 빠져나가야 합니다..."; 
    [SerializeField] private float typingTime = 0.05f;
    [SerializeField] private string sceneName;
    
    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
         StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = "";  
        foreach (char letter in fullText)
        {
            textComponent.text += letter;  
            yield return new WaitForSeconds(typingTime);  
        }
        FadeManager.Instance.LoadScene(sceneName);
    }
}
