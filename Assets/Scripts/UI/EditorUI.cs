
using UnityEngine;

public enum ElementType
{
    Terrain,
    Wall,
    Prop,
    Puzzle,
    Trap
}

public class EditorUI : MonoBehaviour
{
    [SerializeField] private Transform elementButtonContainer;
    
    
    void Start()
    {
        gameObject.SetActive(false);
        MapEditor.Instance.generator.OnGenerateDefaultMap += Activate;
    }
    
    private void Activate()
    { 
        gameObject.SetActive(true);
    }

    private void SetElementButtonByType(ElementType type)
    {
        
        
        switch (type)
        {
            case ElementType.Terrain:
                break;
            case ElementType.Wall:
                break;
            case ElementType.Prop:
                break;
            case ElementType.Puzzle:
                break;
            case ElementType.Trap:
                break;

        }
        
        
        
    
        
    }

}
