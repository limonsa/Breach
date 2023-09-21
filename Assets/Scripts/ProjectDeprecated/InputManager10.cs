using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Events;
using UnityEngine.InputSystem.HID;
using Unity.VisualScripting;

public class InputManager10 : MonoBehaviour
{
    [SerializeField] private GameManager _gm;

    public static UnityAction<EnhancedTouch.Touch> ListeningOneTouch;
    public static UnityAction<EnhancedTouch.Touch> ListeningTwoTouches;
    public static UnityAction<GameObject, EnhancedTouch.Touch, EnhancedTouch.Touch> ListeningDragAndDrop;

    private float _multipleTapThreshold = 0.3f;
    private TouchInputState _touchState = TouchInputState.Inactive;
    private Vector2 iniDragPosition;
    private GameObject _movingObject;

    Vector3 v3Temp;
    Vector3 offset = new Vector3();
    float distanceZ;

    private void Start()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
    }

    private void OnDestroy()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        

        Transform toDrag;
        

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
                if (_touchState == TouchInputState.Inactive)
                {
                    _movingObject = CheckStartDragObject(EnhancedTouch.Touch.activeTouches[0]);
                    if (_movingObject != null)
                    {
                        //TODO:offset = Camera.main.ScreenToViewportPoint(_movingObject.transform.position);
                        offset = Camera.main.ViewportToWorldPoint(_movingObject.transform.position);
                        _gm.DebugLog($"_movinObject position is:{_movingObject.transform.position} and offset:{offset}");
                        _gm.DebugLog($"EnhancedTouch.Touch.activeTouches[0].screenPosition is:{EnhancedTouch.Touch.activeTouches[0].screenPosition} ");
                        //distanceZ = _movingObject.transform.position.z - Camera.main.transform.position.z;
                        distanceZ = _movingObject.transform.position.z;
                        offset.z = distanceZ;

                        _touchState = TouchInputState.Dragging;
                    }
                }
                else if (_touchState == TouchInputState.Dragging)
                {
                    v3Temp = EnhancedTouch.Touch.activeTouches[0].screenPosition;
                    v3Temp.z = distanceZ;
                    //TODO:v3Temp = Camera.main.ScreenToViewportPoint(v3Temp);
                    v3Temp = Camera.main.ViewportToWorldPoint(v3Temp);

                    _movingObject.transform.position = v3Temp;

                }
            }
            else if (EnhancedTouch.Touch.activeTouches[0].phase.Equals(UnityEngine.InputSystem.TouchPhase.Ended))
            {
                if (EnhancedTouch.Touch.activeTouches[0].time - EnhancedTouch.Touch.activeTouches[0].startTime < _multipleTapThreshold)
                {
                    //Debug.Log($"UPDATE: One click detected");
                    ListeningOneTouch?.Invoke(EnhancedTouch.Touch.activeTouches[0]);
                }
                else
                {
                    v3Temp = EnhancedTouch.Touch.activeTouches[0].screenPosition;
                    //TODO:v3Temp = Camera.main.ScreenToViewportPoint(v3Temp);
                    v3Temp = Camera.main.ViewportToWorldPoint(v3Temp);
                    v3Temp.z = distanceZ;
                    _movingObject.transform.position = v3Temp;

                    _gm.DebugLog($"Dragging ended up in {v3Temp} with distanceZ:{distanceZ}");
                    ListeningDragAndDrop?.Invoke(_movingObject, EnhancedTouch.Touch.activeTouches[0], EnhancedTouch.Touch.activeTouches[0]);
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

    private GameObject CheckStartDragObject(EnhancedTouch.Touch touch)
    {
        return _gm.CheckObjectPlacement("Algae", touch);
    }

}

public enum TouchInputState0
{
    Inactive,
    Clicking,
    Dragging,
    Dropping
}
