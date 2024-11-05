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
    private string curElementResourcePath;

    private string curElementData; 
    private Vector3 curSnapPos;
    private bool canBuild;
    private bool isSnapped = false;
    

    private void Awake()
    {
        controller = GetComponent<MapEditInputController>();
    }

    private void Start()
    {
        controller.OnLeftButtonEvent += TryBuildElement;
    }


     public void CreateBuildElement(string path)
    {
        DeleteBuildElement();
        curElementResourcePath = path;
        
        GameObject obj = Instantiate(Resources.Load<GameObject>(curElementResourcePath));
        obj.AddComponent<Rigidbody>().freezeRotation = true;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Collider>().isTrigger = true;
        obj.layer = LayerMask.NameToLayer("Default");
        
        
        curElement = obj.AddComponent<BuildableElement>();
        StartCoroutine(MoveCurElement());
    }

    public void DeleteBuildElement()
    {
        if(curElement !=null)
            Destroy(curElement.gameObject);
        
        curElement = null;
    }

    public void Build()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(curElementResourcePath),DetectNearElement(),Quaternion.identity);
    }

    private IEnumerator MoveCurElement()
    {

        while (curElement != null)
        {
            Vector3 targetPosition = DetectNearElement();

             float distance = Vector3.Distance(curElement.transform.position, targetPosition);


            if (distance > snapDistance && isSnapped)
            {
                 curElement.transform.position = Vector3.MoveTowards(curElement.transform.position, targetPosition, snapSpeed * Time.deltaTime);
            }
            else
            {
                 curElement.transform.position = targetPosition;
            }

            yield return null;
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
            Vector3 objectTopPosition = hit.point;

            if (hit.collider.gameObject.layer == curElement.gameObject.layer)
                return hit.transform.position;
                
            return objectTopPosition;
        }
        
        isSnapped = false;
        return ray.origin + ray.direction * 3f;
    }

    

    private void TryBuildElement()
    {
        
        if (curElement == null || !curElement.CanBuild)
            return;

        StopCoroutine(MoveCurElement());

        Build();
        DeleteBuildElement();
    }
}