using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool canControl = true;

    [Header("Move")]
    public int moveSpeed;
    private Vector2 curMoveInput;
    public int jumpForce;
    public LayerMask groundLayerMask;
    private bool isRunning;
    public float useStamina;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXRot;
    public float maxXRot;
    private float camCurXRot;
    public float lookSensivity;

    public bool canLook = true;

    public Action inventory;


    private Vector2 mouseDelta;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CharacterManager.Instance.Player.onPuzzleEvent += CheckControll;
    }

    private void FixedUpdate()
    {
        if(canControl)
            Move();

        if (isRunning)
            Run();
    }

    private void LateUpdate()
    {
        if(canControl)
            Look();
    }

    private void CheckControll(bool canControl)
    {
        this.canControl = canControl;
        if (canLook)
        {
            Look();
        }
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMoveInput.y + transform.right * curMoveInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    void Look()
    {
        camCurXRot += mouseDelta.y * lookSensivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXRot, maxXRot);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensivity, 0);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMoveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMoveInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool isGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray (transform.position + (transform.forward * 0.2f)+ ( transform.up * 0.01f),Vector2.down),
            new Ray (transform.position + (-transform.forward * 0.2f)+ ( transform.up * 0.01f),Vector2.down),
            new Ray (transform.position + (transform.right * 0.2f)+ ( transform.up * 0.01f),Vector2.down),
            new Ray (transform.position + (-transform.right * 0.2f)+ ( transform.up * 0.01f),Vector2.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.2f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                Run();
            }
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            RunCancel();
        }
    }

    public void Run()
    {
        if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
        {
            isRunning = true;
            moveSpeed = 4;
        }
        else
            RunCancel();
    }

    public void RunCancel()
    {
        isRunning = false;
        moveSpeed = 2;
    }
}
