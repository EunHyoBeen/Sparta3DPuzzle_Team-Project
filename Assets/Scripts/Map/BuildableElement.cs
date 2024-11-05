using System;
using System.Collections;
using UnityEngine;


public class BuildableElement : MonoBehaviour
{
    
    
    private Material originMaterial;
    private MeshRenderer meshRenderer;
    private Material buildableMaterial;
    private Material nonBuildableMaterial;

    private Coroutine curCoroutine;
    public bool isTerrain { get; private set; }
    public bool CanBuild { get; private set; }

    public void SetTerrain()
    {
        isTerrain = true;
    }

    private void Awake()
    {
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        buildableMaterial = Resources.Load<Material>($"Map/Materials/Green");
        nonBuildableMaterial = Resources.Load<Material>($"Map/Materials/Red");
        isTerrain = false;
    }

    private void Start()
    {
        meshRenderer.material = nonBuildableMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            CanBuild = true;
            meshRenderer.material = buildableMaterial;
        }

        else
        {
            CanBuild = false;
            meshRenderer.material = nonBuildableMaterial;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Terrain")) 
        {
            CanBuild = false;
            meshRenderer.material = nonBuildableMaterial;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        CanBuild = false;
        meshRenderer.material = nonBuildableMaterial;
    }
}