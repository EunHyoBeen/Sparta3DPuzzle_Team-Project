using System;
using System.Collections;
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
   
    private Material originMaterial;
    private MeshRenderer mashRenderer;
    private Material buildableMaterial;
    private Material nonBuildableMaterial;

    private Coroutine curCoroutine;
    
    public bool CanBuild { get; private set; }
     private void Awake()
    {
        mashRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        buildableMaterial = Resources.Load<Material>($"Map/Materials/Green");       
        nonBuildableMaterial = Resources.Load<Material>($"Map/Materials/Red");       
    }

    private void Start()
    {
        mashRenderer.material = nonBuildableMaterial;
    }

 
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {

            if(curCoroutine != null)
                StopCoroutine(curCoroutine);
          
            CanBuild = true;
            mashRenderer.material = buildableMaterial;
            Debug.Log("건설 가능 지역 입니다");
        }
       
        else
        {
            Debug.Log(other.name);
            CanBuild = false;
            mashRenderer.material = nonBuildableMaterial;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        StartNewCoroutine(ChangeMaterial());
    }



    private void StartNewCoroutine(IEnumerator coroutine)
    {
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
        }

        curCoroutine = StartCoroutine(coroutine);
    }

    private IEnumerator ChangeMaterial()
    {
        
        CanBuild = false;
         
        yield return new WaitForSeconds(0.09f);
        mashRenderer.material = nonBuildableMaterial;
    }

}
