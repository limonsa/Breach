using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _prefabAlgae;
    [SerializeField] private GameObject _prefabWaterSource;
    [SerializeField] private GameObject[] _aliensPool = new GameObject[10];
    [SerializeField] private int durationInSeconds = 5;

    private int currentTime;

    private Debugger _debugger;
    private Spawner _spawner;
    private List<GameObject> _algaePool = new List<GameObject>();
    private List<GameObject> _waterSourcePool = new List<GameObject>();
    private Vector3 scaleChange;

    private void Awake()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();

        InputManager.ListeningOneFingerTouch += TranslateOneFingerTouch;
        InputManager.ListeningTwoFingersTouch += TranslateTwoFingersTouch;
        InputManager.ListeningOneFingerDrag += ReleaseWaterSource;
        InputManager.ListeningTwoFingersDrag += Amplify;
    }
    private void OnDestroy()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();

        InputManager.ListeningOneFingerTouch -= TranslateOneFingerTouch;
        InputManager.ListeningTwoFingersTouch -= TranslateTwoFingersTouch;
    }

    private void Start()
    {
        _debugger = GetComponent<Debugger>();
        _spawner = GetComponent<Spawner>();
        scaleChange = new Vector3(0.25f, 0.25f, 0.25f);
        currentTime = -1;
    }

    private void Update()
    {
        if(currentTime < 0 && AreRestingAliens())
        {
            currentTime = durationInSeconds;
            StartInvasion();
        }
    }

    public void StartInvasion()
    {
        
        StartCoroutine(TimeIEnum());
    }
    IEnumerator TimeIEnum()
    {
        while (currentTime >= 0)
        {
            //timeImage.fillAmount = Mathf.InverseLerp(0, durationInSeconds, currentTime);
            yield return new WaitForSeconds(1f);
            currentTime--;
            //TODO: Do something to call attention over it as it is about to disapear
            //    if (currentTime == (int)100 / (durationInSeconds * stressTimePercentage))
            //{
            //    timeText.color = Color.red;
            //}
        }
        SpawnAlien();
    }

    public bool AreRestingAliens()
    {
        bool rta = false;
        for(int i=0; i<_aliensPool.Length; i++)
        {
            if (!_aliensPool[i].activeSelf)
            {
                rta = true;
            }
        }
        return rta;
    }

    public void TranslateOneFingerTouch(EnhancedTouch.Touch touch, int tapsCount) {
        GameObject anotherAlgae;
        anotherAlgae = _spawner.CheckPlacement("Algae", touch);
        
        switch (tapsCount) { 
            case 1:
                if (anotherAlgae == null) //No algae found on spot
                {
                    SpawnAlgae(touch);
                }
                else
                {
                    anotherAlgae.SetActive(false);
                }
                break;
            case 2:
                //DebugLog($"TWO TAPS with o n e finger"); 
                break;
        }
    }

    public void TranslateTwoFingersTouch(EnhancedTouch.Touch touch, int tapsCount)
    {
        GameObject anotherWaterSource;
        anotherWaterSource = _spawner.CheckPlacement("Water", touch);

        switch (tapsCount)
        {
            case 1:
                if (anotherWaterSource == null) //No waterSource found on spot
                {
                    SpawnWaterSource(touch);
                }
                else
                {
                    //one tap with two fingers (tested)
                }
                break;
            case 2:
                //TWO TAPS with two fingers 
                break;
        }
    }

    public void ReleaseWaterSource(EnhancedTouch.Touch touch, int tapsCount)
    {
        GameObject anotherWaterSource;
        anotherWaterSource = _spawner.CheckPlacement("Water", touch);
        if (anotherWaterSource != null)
        {
            anotherWaterSource.SetActive(false);
        }
    }

    public void Amplify(EnhancedTouch.Touch touch, int tapsCount)
    {
        GameObject objectSpotted;
        objectSpotted = _spawner.CheckPlacement("e", touch);
        if (objectSpotted != null)
        {
            objectSpotted.transform.localScale += scaleChange; ;
        }
    }

    public void SpawnAlgae(EnhancedTouch.Touch touch)
    {
        GameObject objectSpawned = _spawner.Spawn(_prefabAlgae, touch.finger.currentTouch.screenPosition);
        if (objectSpawned != null) //if space in plane detected
        {
            AddAlgae(objectSpawned);
        }
    }

    public void SpawnWaterSource(EnhancedTouch.Touch touch)
    {
        GameObject objectSpawned = _spawner.Spawn(_prefabWaterSource, touch.finger.currentTouch.screenPosition);
        if (objectSpawned != null) //if space in plane detected
        {
            AddWaterSource(objectSpawned);
        }
    }

    public void SpawnAlien()
    {
        int i;
        bool success = false;
        do
        {
            i = Random.Range(0, _aliensPool.Length);
            if (!_aliensPool[i].activeSelf)
            {
                _aliensPool[i].SetActive(true);
                success = true;
                DebugLog($"Alien {i} spawn");
            }
        } while (!success);
            
    }

    public GameObject CheckObjectPlacement(string prefabName, EnhancedTouch.Touch positionToCheck)
    {
        return _spawner.CheckPlacement(prefabName, positionToCheck);
    }


    public void AddAlgae(GameObject algae)
    {
        _algaePool.Add(algae);
    }

    public void AddWaterSource(GameObject waterSource)
    {
        _waterSourcePool.Add(waterSource);
    }

    public void DebugLog(string message)
    {
        _debugger.DebugLog(message + "\n");
    }
}
