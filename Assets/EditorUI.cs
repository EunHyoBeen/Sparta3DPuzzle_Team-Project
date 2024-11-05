
using UnityEngine;

public class EditorUI : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
        MapEditor.Instance.generator.OnGenerateDefaultMap += Activate;
    }

    
    
    private void Activate()
    {
        Debug.Log("UI 활서오하");
        gameObject.SetActive(true);
    }
}
