using UnityEngine;

public class ResourceLoadButton : MonoBehaviour
{
    private string resourcePath;

    private void Start()
    {
        resourcePath = $"Map/Space/terrain";
    }


    public void SetBuilderElement()
    {
        MapEditor.Instance.builder.CreateBuildElement(resourcePath);
    }

}
