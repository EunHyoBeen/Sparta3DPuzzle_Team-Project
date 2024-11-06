using System;
 using UnityEngine;
using UnityEngine.InputSystem;


public class MapEditInputController : MonoBehaviour
{
    public event Action OnLeftButtonEvent;
    public event Action<Vector2> OnLeftButtonMoveEvent;
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnRotationEvent;
    public event Action<Vector2> OnScrollEvent;
    public event Action OnRightButtonEvent;

    private bool isScrollButtonOn = false;
    private bool isRightButtonOn = false;


    public void OnDirectionalInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isRightButtonOn)
        {
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        
        else if (context.phase == InputActionPhase.Started && isScrollButtonOn)
        {
            OnRotationEvent?.Invoke(context.ReadValue<Vector2>());
        }

        else if (context.phase == InputActionPhase.Canceled)
        {
            OnMoveEvent?.Invoke(Vector2.zero);
            OnRotationEvent?.Invoke(Vector2.zero);
        }
    }


    public void OnLeftButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isScrollButtonOn = true;
            OnLeftButtonEvent?.Invoke();
        }

        else if (context.phase == InputActionPhase.Canceled)
            isScrollButtonOn = false;
    }


    public void OnScrollButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            isScrollButtonOn = true;
        else if (context.phase == InputActionPhase.Canceled)
            isScrollButtonOn = false;
    }


    public void OnLeftButtonMove(InputAction.CallbackContext context)
    {
        if (isScrollButtonOn)
            OnLeftButtonMoveEvent?.Invoke(context.ReadValue<Vector2>());
        else
        {
            OnLeftButtonMoveEvent?.Invoke(Vector2.zero);
        }
    }

    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRightButtonOn = true;
            OnRightButtonEvent?.Invoke();
        }

        else if (context.phase == InputActionPhase.Canceled)
            isRightButtonOn = false;
    }


    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 scrollValue = context.ReadValue<Vector2>();

            if (scrollValue.y > 0)
                OnScrollEvent?.Invoke(new Vector2(0, 1));
            else if (scrollValue.y < 0)
                OnScrollEvent?.Invoke(new Vector2(0, -1));
        }
    }
    
}