using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class SpawnableManager0 : MonoBehaviour
{
    [SerializeField] ARRaycastManager myRayCastMngr;
    [SerializeField] GameObject spawnablePrefab;

    List<ARRaycastHit> myHits = new List<ARRaycastHit>();
    GameObject spawnedObject;

    private void Start()
    {
        spawnedObject = null;
    }

    private void Update()
    {
        Debug.Log("Entro a Udate");
        if (Input.touchCount == 0)
        {
            Debug.Log("Entro a Input.touchCount");
            if (myRayCastMngr.Raycast(Input.GetTouch(0).position, myHits))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    SpawnPrefab(myHits[0].pose.position);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
                {
                    spawnedObject.transform.position = myHits[0].pose.position;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    spawnedObject = null;
                }
            }
        }
    }

    private void SpawnPrefab(Vector3 spawnPos)
    {
        spawnedObject = Instantiate(spawnablePrefab, spawnPos, Quaternion.identity);
    }
}

