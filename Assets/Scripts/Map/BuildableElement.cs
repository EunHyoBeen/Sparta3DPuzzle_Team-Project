using System;
using System.Collections;
using UnityEngine;


public class BuildableElement : MonoBehaviour
{
    private Material originMaterial;
    private MeshRenderer meshRenderer;
    private Material buildableMaterial;
    private Material nonBuildableMaterial;
 
    private const float detectDistance = 1f;
    private const float  checkSameRadius= 1f;

    private int terrainLayerMask;
    private Collider[] hitColliders = new Collider[10];

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
        CanBuild = false;
        meshRenderer.material = nonBuildableMaterial;
        terrainLayerMask = LayerMask.GetMask("Terrain");
        StartCoroutine(CheckBuildAvailability());
    }


    private IEnumerator CheckBuildAvailability()
    {
        while (true)
        {
            ShootDownRay();
            yield return new WaitForFixedUpdate();
        }
    }

    
    private Ray[] GetDownRays()
    {
        return new[]
        {
            new Ray(transform.position + transform.up * 0.2f, -transform.up),
            new Ray(transform.position + transform.up * 0.2f + transform.right * 0.7f, -transform.up),
            new Ray(transform.position + transform.up * 0.2f - transform.right * 0.7f, -transform.up),
        };
    }

    
    private void ShootDownRay()
    {

        foreach (var ray in GetDownRays())
        {
            if (Physics.Raycast(ray, detectDistance, terrainLayerMask))
            {
                CanBuild = true;
                meshRenderer.material = buildableMaterial;
                 CheckSamePosition();
                return;
            }
        }
        
        CanBuild = false;
        meshRenderer.material = nonBuildableMaterial;
    }

    //TODO 물체 콜라이더에 따라 크기조정
    private void CheckSamePosition()
    {
        float radius = 0.05f;
        Vector3 pos = transform.position + transform.up * 0.1f;
        int numHits = Physics.OverlapSphereNonAlloc(pos, radius, hitColliders);

        
        for (int i = 0; i < numHits; i++)
        {
            
            GameObject hitObject = hitColliders[i].gameObject;

            if (hitObject != gameObject && !hitObject.transform.IsChildOf(transform))
            {
                CanBuild = false;
                meshRenderer.material = nonBuildableMaterial;
                return;
            }
        }

        CanBuild = true;
        meshRenderer.material = buildableMaterial;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var ray in GetDownRays())
        {
            Gizmos.DrawRay(ray.origin, ray.direction * detectDistance);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.up * 0.1f, checkSameRadius);


    }
}