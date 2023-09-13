using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;


[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameManager _gm;
    [SerializeField] private GameObject _prefabAlgae;
    [SerializeField] private GameObject _prefabWaterSource;

    private ARRaycastManager _arRaycastMngr;
    private ARPlaneManager _arPlaneMngr;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastMngr = GetComponent<ARRaycastManager>();
        _arPlaneMngr = GetComponent<ARPlaneManager>();
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        InputManager.ListeningFingerInput += Spawn;
        //EnhancedTouch.Touch.onFingerDown += Spawn;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        //EnhancedTouch.Touch.onFingerDown -= Spawn;
    }


    private void DebugLog(string message)
    {
        _gm.DebugLog(message);
    }

    public void Spawn(EnhancedTouch.Touch touch, int fingersUsed) {
        GameObject prefabToSpawn = null;
        if (fingersUsed == 1)
        {
            prefabToSpawn = _prefabAlgae;
        }
        else if (fingersUsed == 2)
        {
            prefabToSpawn = _prefabWaterSource;
        }

        DebugLog($"{fingersUsed} touches have been detected");

        if (prefabToSpawn != null && _arRaycastMngr.Raycast(touch.finger.currentTouch.screenPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            DebugLog($"prefabToSpawn is {prefabToSpawn.name} \n");
            Pose pose = _hits[0].pose;
            GameObject objectSpawned = Instantiate(prefabToSpawn, pose.position, pose.rotation);

            //For the spawned object on the floor to face the camera
            //floor is PlaneAlignment.HorizontalUp and ceiling is PlaneAlignment.HorizontalDown  
            //if (_arPlaneMngr.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)

            Vector3 cameraPos = Camera.main.transform.position; //In the inspector the ARCamera must be tagged as MainCamera
            Vector3 direction = cameraPos - objectSpawned.transform.position; //This is to achieve the object facing the direction: ARCamera <- objectSpowned
            Vector3 targetRotationEuler = Quaternion.LookRotation(direction).eulerAngles;
            //Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, objectSpawned.transform.up.normalized); //Sets x and z to 0 so the spawned object si not tilted, just rotated on y to face the ARCamera
            Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, objectSpawned.transform.position);
            Quaternion targetRotation = Quaternion.Euler(scaledEuler);
            objectSpawned.transform.rotation = objectSpawned.transform.rotation * targetRotation; //Applies the extra rotation to the original rotation; Quaternions CANNOT be added together, they must be multiplied

        }
    }

    public void Spawn0(EnhancedTouch.Finger finger)
    {
        float lastTapTime = 0;
        float doubleTapThreshold = 0.3f;

        GameObject prefabToSpawn = null;
        //Supporting multitouch in the screen (1st finger.index=0, 2nd finger.index=1 ... and so on
        //DebugLog($"finger.index IN SPAWNER is {finger.index}");

        if (finger.index == 0)
        {
            prefabToSpawn = _prefabAlgae;
        }
        else if (finger.index == 1)
        {
            prefabToSpawn = _prefabWaterSource;
        }

        DebugLog($"{finger.lastTouch.tapCount} touches have been detected");
        if(finger.lastTouch.tapCount == 1)
        {
            EnhancedTouch.Touch touch = finger.currentTouch;
            if(touch.phase.Equals(TouchPhase.Began))
            {
                if (Time.time - lastTapTime <= doubleTapThreshold)
                {
                    lastTapTime = 0;
                    DebugLog($"DOUBLE TAB DETECTED");
                }
                else {
                    lastTapTime = Time.time;
                }
            }
        }

        if (prefabToSpawn != null && _arRaycastMngr.Raycast(finger.currentTouch.screenPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            DebugLog($"prefabToSpawn is {prefabToSpawn.name} \n");
            Pose pose = _hits[0].pose; 
            GameObject objectSpawned = Instantiate(prefabToSpawn, pose.position, pose.rotation);

            //For the spawned object on the floor to face the camera
            //floor is PlaneAlignment.HorizontalUp and ceiling is PlaneAlignment.HorizontalDown  
            //if (_arPlaneMngr.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)

            Vector3 cameraPos = Camera.main.transform.position; //In the inspector the ARCamera must be tagged as MainCamera
            Vector3 direction = cameraPos - objectSpawned.transform.position; //This is to achieve the object facing the direction: ARCamera <- objectSpowned
            Vector3 targetRotationEuler = Quaternion.LookRotation(direction).eulerAngles;
            //Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, objectSpawned.transform.up.normalized); //Sets x and z to 0 so the spawned object si not tilted, just rotated on y to face the ARCamera
            Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, objectSpawned.transform.position);
            Quaternion targetRotation = Quaternion.Euler(scaledEuler);
            objectSpawned.transform.rotation = objectSpawned.transform.rotation * targetRotation; //Applies the extra rotation to the original rotation; Quaternions CANNOT be added together, they must be multiplied
        
        }

        //if (finger.index >= 0 && finger.index < 2)
        //{
        //    prefabToSpawn = _prefab[finger.index];
        //    DebugLog($"prefabToSpawn is {prefabToSpawn.name}");
        //}

        //if (prefabToSpawn != null &&
        //    _arRaycastMngr.Raycast(finger.currentTouch.screenPosition, _hits, TrackableType.PlaneWithinPolygon))
        //{
        //    Pose pose = _hits[^1].pose; // _hits[^1] is the same as _hits[_hits.Count -1]
        //    GameObject objectSpawned = Instantiate(prefabToSpawn, pose.position, pose.rotation);

        //    //For the spawned object on the floor to face the camera
        //    //floor is PlaneAlignment.HorizontalUp and ceiling is PlaneAlignment.HorizontalDown  
        //    //if (_arPlaneMngr.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)

        //    Vector3 cameraPos = Camera.main.transform.position; //In the inspector the ARCamera must be tagged as MainCamera
        //    Vector3 direction = cameraPos - objectSpawned.transform.position; //This is to achieve the object facing the direction: ARCamera <- objectSpowned
        //    Vector3 targetRotationEuler = Quaternion.LookRotation(direction).eulerAngles;
        //    //Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, objectSpawned.transform.up.normalized); //Sets x and z to 0 so the spawned object si not tilted, just rotated on y to face the ARCamera
        //    Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, objectSpawned.transform.position);
        //    Quaternion targetRotation = Quaternion.Euler(scaledEuler);
        //    objectSpawned.transform.rotation = objectSpawned.transform.rotation * targetRotation; //Applies the extra rotation to the original rotation; Quaternions CANNOT be added together, they must be multiplied
        //}
    }
}
