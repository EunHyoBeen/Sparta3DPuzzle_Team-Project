using System;
using UnityEngine;

public class MapEditCamMovement : MonoBehaviour
{
    private Vector2 curCameraZoomDir;
    private Vector2 curCameraMovementDir;
    private Vector2 curCameraLookDir;
    private float curCameraDepth;
    private Camera EditCam;
    private MapEditInputController controller;


    [SerializeField] private float rotationSpeed = 1f; 
    [SerializeField] private float moveSpeed = 1f; 
    [SerializeField] private float zoomSpeed = 1f;

    private void Awake()
    {
        controller = GetComponent<MapEditInputController>();
        EditCam = Camera.main;
    }

    private void Start()
    {
        controller.OnLeftButtonMoveEvent += SetMovementDir;
        controller.OnRightButtonLookEvent += SetLookDir;
        controller.OnScrollEvent += Zoom;
    }

    private void LateUpdate()
    {
        Move();
        Look();
    }


    private void Move()
    {
        Vector3 movement = new Vector3(
            -curCameraMovementDir.x,
            -curCameraMovementDir.y,
            0
        );

        EditCam.transform.position += movement * moveSpeed;
    }

    private void Look()
    {
        float xAngle = -curCameraLookDir.y * rotationSpeed;
        float yAngle = curCameraLookDir.x * rotationSpeed;

        Vector3 currentRotation = EditCam.transform.eulerAngles;
        currentRotation.x += xAngle;
        currentRotation.y += yAngle;
    
        EditCam.transform.eulerAngles = currentRotation;
    }

    private void Zoom(Vector2 newDir)
    {
        EditCam.transform.position += EditCam.transform.forward * newDir.y * zoomSpeed; 
    }
 
    private void SetMovementDir(Vector2 newDir)
    {
        curCameraMovementDir = newDir.normalized;
    }

    private void SetLookDir(Vector2 newDir)
    {
        curCameraLookDir = newDir.normalized;
    }
}