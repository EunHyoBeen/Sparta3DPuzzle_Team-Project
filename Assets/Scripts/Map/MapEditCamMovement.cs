using System;
using UnityEngine;

public class MapEditCamMovement : MonoBehaviour
{
    private Vector2 curCameraZoomDir;
    private Vector2 curMouseMovementDir;
    private Vector2 curKeyboardMovementDir;
    private Vector2 curCameraLookDir;
    private float curCameraDepth;
    private Transform EditCam;
    private MapEditInputController controller;


    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float zoomSpeed = 1f;

    private void Awake()
    {
        controller = GetComponent<MapEditInputController>();
        EditCam = Camera.main.transform;
    }

    private void Start()
    {
        controller.OnRotationEvent += SetLookDir;
        controller.OnScrollEvent += Zoom;
        controller.OnMoveEvent += SetKeyboardMovementDir;
    }

    private void LateUpdate()
    {
        Rotate();
        Move();
    }


    private void Move()
    {
        // 방향 벡터 계산
        Vector3 dir = EditCam.forward * curKeyboardMovementDir.y + EditCam.right * curKeyboardMovementDir.x;
        
        dir.Normalize();


        dir *= moveSpeed * Time.deltaTime;
        EditCam.position += dir;
    }


    private void Rotate()
    {
        float xAngle = -curCameraLookDir.y * rotationSpeed * Time.deltaTime;
        float yAngle = curCameraLookDir.x * rotationSpeed * Time.deltaTime;

        Vector3 currentRotation = EditCam.transform.eulerAngles;
        currentRotation.x += xAngle;
        currentRotation.y += yAngle;

        EditCam.eulerAngles = currentRotation;
    }

    private void Zoom(Vector2 newDir)
    {
        EditCam.position += EditCam.transform.forward * newDir.y * zoomSpeed;
    }


    private void SetKeyboardMovementDir(Vector2 newDir)
    {
        curKeyboardMovementDir = newDir.normalized;
    }

    private void SetLookDir(Vector2 newDir)
    {
        curCameraLookDir = newDir.normalized;
    }
}