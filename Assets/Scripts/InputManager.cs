using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameManager _gm;

    public static UnityAction<EnhancedTouch.Touch> ListeningOneTouch;
    public static UnityAction<EnhancedTouch.Touch> ListeningTwoTouches;
    public static UnityAction<Vector2, Vector2> ListeningDragAndDrop;
    //public static UnityEvent<EnhancedTouch.Finger> ListeningFingerUp;
    //public static UnityEvent<EnhancedTouch.Finger> ListeningFingerMove;

    private float _multipleTapThreshold = 0.3f;
    private TouchInputState _touchState = TouchInputState.Inactive;
    private Vector2 iniDragPosition;

    
    private void Start()
    {
        //EnhancedTouch.Touch.onFingerDown += TriggerFingerDown;
        //EnhancedTouch.Touch.onFingerUp += TriggerFingerUp;
        //EnhancedTouch.Touch.onFingerMove += TriggerFingerMove;
    }

    private void Awake()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        //TODO: Ask OT if += to the events made in Start or in Awake or OnEneable
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        //EnhancedTouch.Touch.onFingerDown -= TriggerFingerDown;
        //EnhancedTouch.Touch.onFingerDown -= TriggerFingerUp;
        //EnhancedTouch.Touch.onFingerMove -= TriggerFingerMove;
    }

    private void Update()
    {        
        if (EnhancedTouch.Touch.activeFingers.Count == 1)
        {
            //startedTime = (float)EnhancedTouch.Touch.activeTouches[0].time;
            //Debug.Log($"One click detected | phase:{EnhancedTouch.Touch.activeTouches[0].phase} | inProgress:{EnhancedTouch.Touch.activeTouches[0].inProgress} | isInProgress:{EnhancedTouch.Touch.activeTouches[0].isInProgress} | isTap:{EnhancedTouch.Touch.activeTouches[0].isTap} | time: {EnhancedTouch.Touch.activeTouches[0].time} | startedTime: {EnhancedTouch.Touch.activeTouches[0].startTime}");
            //Debug.Log($"UPDATE: One click detected | {EnhancedTouch.Touch.activeTouches[0].ToString()}");
            if (EnhancedTouch.Touch.activeTouches[0].phase.Equals(UnityEngine.InputSystem.TouchPhase.Began))
            {
                iniDragPosition = EnhancedTouch.Touch.activeFingers[0].currentTouch.screenPosition;
            }
            else if (EnhancedTouch.Touch.activeTouches[0].phase.Equals(UnityEngine.InputSystem.TouchPhase.Moved))
            {
                if (_touchState == TouchInputState.Inactive) {
                    //Debug.Log($"UPDATE: First part of dragging detected");
                    _touchState = TouchInputState.Dragging;
                }
            }
            else if (EnhancedTouch.Touch.activeTouches[0].phase.Equals(UnityEngine.InputSystem.TouchPhase.Ended))
            {
                if (EnhancedTouch.Touch.activeTouches[0].time - EnhancedTouch.Touch.activeTouches[0].startTime < _multipleTapThreshold)
                {
                    //Debug.Log($"UPDATE: One click detected");
                    ListeningOneTouch?.Invoke(EnhancedTouch.Touch.activeTouches[0]);
                }
                else {
                    //iniDragPosition -that has been saved is the same as- EnhancedTouch.Touch.activeTouches[0].startScreenPosition
                    //Debug.Log($"UPDATE : drag finished from {iniDragPosition} to startScreenPosition:{EnhancedTouch.Touch.activeTouches[0].startScreenPosition} or screenPosition:{EnhancedTouch.Touch.activeTouches[0].screenPosition} ");
                    ListeningDragAndDrop?.Invoke(iniDragPosition, EnhancedTouch.Touch.activeTouches[0].screenPosition);
                }
                _touchState = TouchInputState.Inactive;
            }
        }
        else if (EnhancedTouch.Touch.activeFingers.Count == 2)
        {
            if (EnhancedTouch.Touch.activeTouches[0].phase.Equals(UnityEngine.InputSystem.TouchPhase.Began))
            {
                //_gm.DebugLog($"INPUT MANAGER.Update >>> Double tab detected COUNTED {EnhancedTouch.Touch.activeFingers.Count} fingers");
                ListeningTwoTouches?.Invoke(EnhancedTouch.Touch.activeTouches[0]);
                _touchState = TouchInputState.Inactive;
            }
        }
     }
}

public enum TouchInputState
{
    Inactive,
    Clicking,
    Dragging,
    Dropping
}

