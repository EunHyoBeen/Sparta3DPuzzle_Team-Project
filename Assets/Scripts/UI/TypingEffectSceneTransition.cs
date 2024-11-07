using System;
using System.Collections;
using TMPro;
using UnityEngine;

 
public class TypingEffectSceneTransition : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    [SerializeField] private string fullText = "This is the text that will appear with a typing effect."; 
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
