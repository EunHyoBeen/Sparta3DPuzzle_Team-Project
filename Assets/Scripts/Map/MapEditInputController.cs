using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


//SendMessages 방식으로 작동 
public class MapEditInputController : MonoBehaviour
{
    
    public event Action OnLeftButtonEvent;
    public event Action<Vector2> OnLeftButtonMoveEvent;
    public event Action<Vector2> OnRightButtonLookEvent;
    public event Action<Vector2> OnScrollEvent;

    private bool isCanMoveButtonOn = false;
    private bool isRightButtonOn = false;

    public void OnLeftButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isCanMoveButtonOn = true;
            OnLeftButtonEvent?.Invoke();
        }

        else if (context.phase == InputActionPhase.Canceled)
            isCanMoveButtonOn = false;
    }
    
    public void OnScrollButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            isCanMoveButtonOn = true;
        else if (context.phase == InputActionPhase.Canceled)
            isCanMoveButtonOn = false;
    }


    public void OnLeftButtonMove(InputAction.CallbackContext context)
    {
        if (isCanMoveButtonOn)
            OnLeftButtonMoveEvent?.Invoke(context.ReadValue<Vector2>());
        else
        {
            OnLeftButtonMoveEvent?.Invoke(Vector2.zero);
        }
    }

    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            isRightButtonOn = true;
        else if (context.phase == InputActionPhase.Canceled)
            isRightButtonOn = false;
    }

    public void OnRightButtonMove(InputAction.CallbackContext context)
    {
        if (isRightButtonOn)
            OnRightButtonLookEvent?.Invoke(context.ReadValue<Vector2>());
        else
        {
            OnRightButtonLookEvent?.Invoke(Vector2.zero);
        }
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (context.ReadValue<Vector2>().y > 0)
                OnScrollEvent?.Invoke(new Vector2(0, 1));
            else if (context.ReadValue<Vector2>().y < 0)
                OnScrollEvent?.Invoke(new Vector2(0, -1));
        }
    }
}