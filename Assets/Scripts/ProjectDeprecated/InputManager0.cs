using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)] //To be sure that InputManager gets executed first
public class InputManager0 : Singleton<InputManager0>
{/*
    public static UnityAction<Vector2, float> StartingTouch;
    public static UnityAction<Vector2, float> EndingTouch;

    private TouchContols touchControls;

    private void Awake()
    {
        touchControls = new TouchContols();
    }
    private void OnEnable()
    {
        touchControls.Enable();
    }
    private void OnDisable()
    {
        touchControls.Disable();
    }
    private void Start()
    {
        touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }
    private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log($"TOUCH STARTED {touchControls.Touch.TouchPosition.ReadValue<Vector2>()}");
        StartingTouch?.Invoke(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
    }
    private void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log($"TOUCH ENDED {touchControls.Touch.TouchPosition.ReadValue<Vector2>()}");
        EndingTouch?.Invoke(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
    }*/
}
