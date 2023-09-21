using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;


[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameManager _gm;
    [SerializeField] private int _scenarioSizeX = 20;
    [SerializeField] private int _scenarioSizeY = 5;
    [SerializeField] private int _scenarioSizeZ = 20;

    private ARRaycastManager _arRaycastMngr;
    private ARPlaneManager _arPlaneMngr;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastMngr = GetComponent<ARRaycastManager>();
        _arPlaneMngr = GetComponent<ARPlaneManager>();
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }

    public GameObject Spawn(GameObject prefabToSpawn, Vector2 targetPosition) {
        GameObject objectSpawned = null;
        if (prefabToSpawn != null && _arRaycastMngr.Raycast(targetPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = _hits[0].pose;
            objectSpawned = Instantiate(prefabToSpawn, pose.position, pose.rotation);

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
        //_gm.DebugLog($"prefabToSpawn is {objectSpawned.ToString()} \n");
        return objectSpawned;
    }

    public GameObject SpawnRandom(GameObject prefabToSpawn)
    {
        GameObject objectSpawned = null;
        Vector2 targetPosition;
        System.Random rnd = new System.Random();
        int num = rnd.Next();
        float posX, posY, posZ;
        posX = rnd.Next(3,_scenarioSizeX) * (float)rnd.NextDouble();
        posY = rnd.Next(_scenarioSizeY) + (float)rnd.NextDouble();
        posZ = rnd.Next(3,_scenarioSizeZ) * (float)rnd.NextDouble();
        targetPosition = new Vector3(posX, posY, posZ);
        objectSpawned = Instantiate(prefabToSpawn, targetPosition, Quaternion.identity);
        _gm.DebugLog($"{prefabToSpawn.name} randomly spawn at {objectSpawned.transform.position}");
        return objectSpawned;
    }

    public GameObject CheckPlacement(string name, EnhancedTouch.Touch targetPosition)
    {
        /*if (_arRaycastMngr.Raycast(targetPosition, _hits, TrackableType.FeaturePoint))
        {
            _gm.DebugLog($"Raycast hit a {_hits[0].hitType}");
        }*/
        GameObject objectFound = null;
        Vector3 pos = targetPosition.startScreenPosition;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Spawnable") && hit.collider.gameObject.name.Contains(name))
            {
                objectFound = hit.collider.gameObject;
                //_gm.DebugLog($"SPAWNER CheckPlacement detected {hit.collider.name} at {hit.collider.transform.position}");
            }
        }
        return objectFound;
    }
}
