using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ResourceLoadButton : MonoBehaviour
{
    private string _resourcePath;
    private Sprite icon;
    
    public void Setting(string resourcePath , string iconPath)
    {
        _resourcePath = resourcePath;
        icon = Resources.Load<Sprite>(iconPath);
        Debug.Log(iconPath);
        Debug.Log(icon);
        gameObject.GetComponent<Image>().sprite = icon;
    }

    public void SetBuilderElement()
    {
        MapEditor.Instance.builder.CreateBuildElement(_resourcePath);
    }

}
