using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Reflection;

//[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class InputManager : MonoBehaviour
{
    [SerializeField] private GameManager _gm;

    public static UnityAction<EnhancedTouch.Touch, int> ListeningFingerInput;
    //public static UnityEvent<EnhancedTouch.Finger> ListeningFingerUp;
    //public static UnityEvent<EnhancedTouch.Finger> ListeningFingerMove;

    private float _lastTapTime = 0;
    private float _multipleTapThreshold = 0.3f;
    private float _startTimeStamp = -1;
    private int _multipleFingersCount = 0;
    private int _fingersLifted = 0;

    private void AddFingerCount(float startedTime)
    {
        if(_startTimeStamp == -1)
        {
            _startTimeStamp = startedTime;
        }
        if(_startTimeStamp - startedTime < _multipleTapThreshold)
        {
            _multipleFingersCount++;
        }
    }

    private void FinishFingerCount(int totalFingersInPlay) {
        _fingersLifted++;
        if(_fingersLifted == totalFingersInPlay)
        {
            _gm.DebugLog($"TriggerFingerUp says {_multipleFingersCount} tab");
            _multipleFingersCount = 0;
        }
    }

    
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
        if (EnhancedTouch.Touch.activeFingers.Count > 0) {
            if (EnhancedTouch.Touch.activeTouches[0].phase.ToString() == "Began") 
            {
                //Debug.Log($"UPDATE : entered in Began if check COUNTED {EnhancedTouch.Touch.activeFingers.Count} fingers");
                ListeningFingerInput?.Invoke(EnhancedTouch.Touch.activeTouches[0], EnhancedTouch.Touch.activeFingers.Count);
            }
        }

        /*foreach (EnhancedTouch.Touch touch in EnhancedTouch.Touch.activeTouches){
            if(touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                fingersCount++;
            }
        }*/
        /*if (EnhancedTouch.Touch.activeFingers.Count > 0) {
            foreach (EnhancedTouch.Finger finger in EnhancedTouch.Touch.activeFingers) {
                _gm.DebugLog($"{finger.index} says ... Phase: {finger.currentTouch.phase} | Position: {finger.currentTouch.startScreenPosition}");
            }
            
        }*/

        //if (EnhancedTouch.Touch.activeFingers.Count == 1)
        //{
        //    EnhancedTouch.Touch activeTouch = EnhancedTouch.Touch.activeFingers[0].currentTouch;
        //    _gm.DebugLog($"Phase: {activeTouch.phase} | Position: {activeTouch.startScreenPosition}");
        //}
    }

    //public void TriggerFingerDown(EnhancedTouch.Finger finger)
    //{
    //    float t = (float)EnhancedTouch.Touch.activeFingers[finger.index].currentTouch.startTime;
    //    AddFingerCount(t);
    //   /* int multipleTouches = 0;
    //    float startedTime = (float)EnhancedTouch.Touch.activeFingers[0].currentTouch.startTime;
    //    _gm.DebugLog($"INPUT MANAGER detected touch at {startedTime}");
    //    ListeningFingerDown?.Invoke(finger);

    //    foreach (EnhancedTouch.Finger f in EnhancedTouch.Touch.activeFingers) {
    //        if (f.currentTouch.startTime - startedTime < _multipleTapThreshold)
    //        {
    //            multipleTouches++;
    //        }
    //    }
    //    _gm.DebugLog($"INPUT MANAGER detected {multipleTouches} tabs");*/


    //    /*        
    //    if (finger.index == 0)
    //    {
    //        _gm.DebugLog("INPUT MANAGER detected single tab");
    //    }
    //    else if (finger.index == 1)
    //    {
    //        if (EnhancedTouch.Touch.activeFingers[0].currentTouch.startTime - EnhancedTouch.Touch.activeFingers[1].currentTouch.startTime < multipleTapThreshold)
    //        {
    //            _gm.DebugLog("INPUT MANAGER detected double tab");
    //        }
    //    }
    //    else if (finger.index == 2)
    //    {
    //        if (EnhancedTouch.Touch.activeFingers[0].currentTouch.startTime - EnhancedTouch.Touch.activeFingers[1].currentTouch.startTime < multipleTapThreshold &&
    //            EnhancedTouch.Touch.activeFingers[1].currentTouch.startTime - EnhancedTouch.Touch.activeFingers[2].currentTouch.startTime < multipleTapThreshold)
    //        {
    //            _gm.DebugLog("INPUT MANAGER detected triple tab");
    //        }
    //    }*/
    //}

    //public void TriggerFingerUp(EnhancedTouch.Finger finger)
    //{
    //    ListeningFingerUp?.Invoke(finger);

    //    //_gm.DebugLog($"TriggerFingerUp says: {EnhancedTouch.Touch.activeFingers.Count}");
    //    //FinishFingerCount(EnhancedTouch.Touch.activeFingers.Count);


    //    /*EnhancedTouch.Touch activeTouch = EnhancedTouch.Touch.activeFingers[finger.index].currentTouch;

    //    _gm.DebugLog($"TRIGGER FINGER UP ... Finger with index: {finger.index} got to Phase: {activeTouch.phase}");
    //    if (activeTouch.phase.Equals(TouchPhase.Ended)) {
    //        _gm.DebugLog($"Finger with index: {finger.index} got to Phase: {activeTouch.phase}");
    //    }*/
    //}

    //public void TriggerFingerMove(EnhancedTouch.Finger finger)
    //{
    //    ListeningFingerMove?.Invoke(finger);
    //}
}

