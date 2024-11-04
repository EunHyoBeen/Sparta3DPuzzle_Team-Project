using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Builder : MonoBehaviour
{
    
    [SerializeField] float snapingThreshold = 1f;
    [SerializeField] float detectDistance = 2f;
    
    
    private BuildableElement curElement;
    private MapEditInputController controller;


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
        CreateBuildElement();
    }


    //TODO 나중에는 UI에서 값 받아와서 생성 
    public void CreateBuildElement()
    {
        curElement = null;
        //GameObject obj = Instantiate(Resources.Load<GameObject>($"Map/Space/tunnel_diagonal_long_A"));
        GameObject obj = Instantiate(Resources.Load<GameObject>($"Map/Space/rocks_A"));
        //GameObject obj = Instantiate(Resources.Load<GameObject>($"Map/Space/terrain"));
        obj.AddComponent<Rigidbody>().freezeRotation = true;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Collider>().isTrigger = true;
        curElement = obj.AddComponent<BuildableElement>();
        
        StartCoroutine(MoveCurElement());
    }

    public void DeleteBuildElement()
    {
        
        Destroy(curElement.gameObject);
        curElement = null;
    }

    public void Build()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>($"Map/Space/terrain"),curElement.transform.position,Quaternion.identity);
    }


    //TODO 시점 상 절대 못 설치하는 위치가 있음 
    private IEnumerator MoveCurElement()
    {
        while (curElement != null)
        {
            yield return null;

            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(curElement.transform.position).z);
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            Debug.Log("마우스 포인트 월드"+mouseWorldPosition);
            Vector3 detectedPosition = DetectNearElement();

            bool hasDetectedPosition = detectedPosition != Vector3.zero;
            bool significantSnapChange = !isSnapped || Vector3.Distance(curSnapPos, detectedPosition) > snapingThreshold;

            if (hasDetectedPosition && significantSnapChange)
            {
                curSnapPos = detectedPosition;
                curElement.transform.position = curSnapPos;
                isSnapped = true;
            }
            else if (!hasDetectedPosition || Vector3.Distance(mouseWorldPosition, curSnapPos) > snapingThreshold)
            {
                isSnapped = false;
                curSnapPos = Vector3.zero;
            }

            if (!isSnapped)
            {
                curElement.transform.position = mouseWorldPosition;
            }
        }
    }





    private Vector3 DetectNearElement()
    {
        Collider cor = curElement.GetComponent<Collider>();
        float offset =  0.01f;  
       
        Ray[] rays = new Ray[]
        {
            new Ray(cor.bounds.center - curElement.transform.up * offset, -curElement.transform.up),         
            new Ray(cor.bounds.center + curElement.transform.right * offset, curElement.transform.right),     
            new Ray(cor.bounds.center - curElement.transform.right * offset, -curElement.transform.right),   
            new Ray(cor.bounds.center+curElement.transform.forward * offset, curElement.transform.forward),  
            new Ray(cor.bounds.center  - curElement.transform.forward, -curElement.transform.forward)  
        };
       
        Collider myCollider = curElement.GetComponent<Collider>();

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], out RaycastHit hit, detectDistance))
            {
                
                if (hit.collider == myCollider)
                    continue;
                
                Vector3 hitColliderExtents = hit.collider.bounds.extents;

                Vector3 myColliderExtents = myCollider.bounds.extents;

                Vector3 snapPosition = hit.point; 

                // switch (i)
                // {
                //     case 0: 
                //         snapPosition += hit.transform.up; 
                //         break;
                //     case 1: // 오른쪽에서 충돌한 경우
                //         snapPosition -= hit.transform.right.normalized; 
                //         break;
                //     case 2: // 왼쪽에서 충돌한 경우
                //         snapPosition += hit.transform.right.normalized; 
                //         break;
                //     case 3: // 뒤쪽에서 충돌한 경우
                //         snapPosition -= hit.transform.forward.normalized ; 
                //         break;
                //     case 4: // 앞쪽에서 충돌한 경우
                //         snapPosition += hit.transform.forward.normalized;
                //         break;
                // }
                
                Debug.Log($"Snap Position: {snapPosition}");

                return snapPosition; 
            }
        }

        
       

        return Vector3.zero;
    }

    
    

    private void OnDrawGizmos()
    {
        if (curElement == null) return;
        Collider cor = curElement.GetComponent<Collider>();
        float offset = 0.01f; 

        Gizmos.color = Color.red;
 
        Gizmos.DrawRay(cor.bounds.center - curElement.transform.up * offset , -curElement.transform.up * detectDistance);
        Gizmos.DrawRay(cor.bounds.center + curElement.transform.right * offset , curElement.transform.right * detectDistance);
        Gizmos.DrawRay(cor.bounds.center - curElement.transform.right * offset, -curElement.transform.right * detectDistance);
        Gizmos.DrawRay(cor.bounds.center +curElement.transform.forward * offset, curElement.transform.forward * detectDistance);
        Gizmos.DrawRay(cor.bounds.center - curElement.transform.forward * offset, -curElement.transform.forward * detectDistance);
    }


    private void TryBuildElement()
    {
        if (curElement == null && !curElement.CanBuild)
            return;

        StopCoroutine(MoveCurElement());

        Build();
        DeleteBuildElement();
    }
}