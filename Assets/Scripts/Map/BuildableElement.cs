using System;
using UnityEngine;


public enum ElementType
{
    Terrain,
    Wall,
    Prop,
    Puzzle,
    Trap
}


public class BuildableElement : MonoBehaviour
{
    [SerializeField]private ElementType type;
   
    //Material 요소 
    private Material originMaterial;
    private MeshRenderer mashRenderer;
    private Material buildableMaterial;
    private Material nonBuildableMaterial;

    public bool CanBuild { get; private set; }
     private void Awake()
    {
        mashRenderer = GetComponent<MeshRenderer>();
        originMaterial = mashRenderer.material;
        buildableMaterial = Resources.Load<Material>($"Map/Materials/Green");       
        nonBuildableMaterial = Resources.Load<Material>($"Map/Materials/Red");       
    }
     

    public void Built()
    {
         mashRenderer.material = originMaterial;
         Debug.Log("건설완료");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            CanBuild = true;
            mashRenderer.material = buildableMaterial;
            Debug.Log("건설 가능 지역 입니다");
        }
       
        else
        {
            CanBuild = false;
            mashRenderer.material = nonBuildableMaterial;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        CanBuild = false;
        mashRenderer.material = nonBuildableMaterial;

    }
    
}
