using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


//SendMessages 방식으로 작동 
public class MapEditInputController : MonoBehaviour
{
    public event Action OnLeftButtonEvent;
    public event Action<Vector2> OnRightButtonEvent;
    public event Action<Vector2> OnScrollEvent;
    
    
    public void OnLeftButton(InputValue value)
    {
        OnLeftButtonEvent?.Invoke();
    }
    
    public void OnRightButton(InputValue value)
    {
        OnRightButtonEvent?.Invoke(value.Get<Vector2>());
    }
    
    public void OnScroll(InputValue value)
    {
        OnScrollEvent?.Invoke(value.Get<Vector2>());
    }
}