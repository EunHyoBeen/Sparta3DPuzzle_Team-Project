using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Builder : MonoBehaviour
{
    
    [SerializeField]float snapDistance = 0.1f; 
    [SerializeField]float snapSpeed = 30f; 
    
    private BuildableElement curElement;
    private MapEditInputController controller;
    private MapGenerator  generator;
    
    private string curElementResourcePath;

    private string curElementData; 
    private Vector3 curSnapPos;
    private bool canBuild;
    private bool isSnapped = false;

    private Stack<GameObject> elementHistory = new Stack<GameObject>();
    private GameObject curPlayerPos;
    private GameObject curEndPoint;
    
    
    private void Awake()
    {
        controller = GetComponent<MapEditInputController>();
    }

    private void Start()
    {
        controller.OnLeftButtonEvent += TryBuildElement;
        controller.OnRightButtonEvent += DeleteBuildElement;
    }

 
    public void CreateBuildElement(string path)
    {
        DeleteBuildElement();
        curElementResourcePath = path;
        GameObject obj = Instantiate(Resources.Load<GameObject>(curElementResourcePath));
        obj.AddComponent<Rigidbody>().isKinematic = true;
        obj.GetComponentInChildren<Collider>().isTrigger = true;
        curElement = obj.AddComponent<BuildableElement>();
 
        SetElementByLayer(obj);


        obj.layer = 0;
        obj.transform.GetChild(0).gameObject.layer = 0;
        StartCoroutine(MoveCurElement());
    }

    
    private void SetElementByLayer(GameObject obj)
    {
        if (obj.layer == LayerMask.NameToLayer("Terrain"))
            curElement.SetTerrain();
        
        if(obj.layer == LayerMask.NameToLayer("EndPoint"))
            curElement.SetEndPoint();
        
        if(obj.layer == LayerMask.NameToLayer("PlayerPos"))
            curElement.SetPlayerPos();
    }

    
    public void DeleteBuildElement()
    {
        if(curElement !=null)
            Destroy(curElement.gameObject);
        
        curElement = null;
    }

    public void Build()
    {
        GameObject obj = Instantiate(
            Resources.Load<GameObject>(curElementResourcePath),
            DetectNearElement(),  
            Quaternion.identity, 
            MapEditor.Instance.mapContainer.transform  
        );

        AssignPlayerPosElement(obj);
        AssignEndPointElement(obj);
        
       
         if(obj.TryGetComponent<IPuzzleElement>(out IPuzzleElement puzzleElement))
         {
             puzzleElement.InitializePuzzleElement();
         }
        elementHistory.Push(obj);
    }

    private void AssignPlayerPosElement(GameObject obj)
    {
        if(!curElement.isPlayerPos)
            return;

        if (curPlayerPos == null)
        {
            curPlayerPos = obj;
            return;
        }

        
        Destroy(curPlayerPos);
        curPlayerPos = obj;

    }

    private void AssignEndPointElement(GameObject obj)
    {
        
        if(!curElement.isEndPoint)
            return;

        if (curEndPoint == null)
        {
            curEndPoint = obj;
            return;
        }
        
        Destroy(curEndPoint);
        curEndPoint = obj;
    }

    public void UndoBuild()
    {
        if(elementHistory.Count > 0)
            Destroy( elementHistory.Pop());
    }

    private IEnumerator MoveCurElement()
    {
        while (curElement != null)
        {
            Vector3 targetPosition = DetectNearElement();

             float distance = Vector3.Distance(curElement.transform.position, targetPosition);

            if (distance < snapDistance && isSnapped)
            {
                 curElement.transform.position = Vector3.MoveTowards(curElement.transform.position,
                     targetPosition, snapSpeed * Time.deltaTime);
            }
            else
            {
                 curElement.transform.position = targetPosition;
            }

            yield return new WaitForFixedUpdate();
        }
    }
    
    private Vector3 DetectNearElement()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerMask = LayerMask.GetMask("Terrain");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            isSnapped = true;
            if (curElement.isTerrain == true)
            {
                return CalculateTerrainPosition(hit);
            }

            return hit.point;
        }

        isSnapped = false;
        return ray.origin + ray.direction * 3f;
    }
    

    private Vector3 CalculateTerrainPosition(RaycastHit hit) 
    {
         Vector3 gameObjectBottomCenter = hit.transform.position;
         float colliderHeight = hit.collider.bounds.size.y;
         Vector3 topPosition = gameObjectBottomCenter + colliderHeight * Vector3.up ;
         
        return topPosition;
    }

    private void TryBuildElement()
    {
        
        if (curElement == null || !curElement.CanBuild)
            return;

        StopCoroutine(MoveCurElement());
        Build();
        DeleteBuildElement();
        CreateBuildElement(curElementResourcePath);
    }
}