using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using TMPro;
using UnityEngine.InputSystem.XR;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _prefabAlgae;
    [SerializeField] private GameObject _prefabWaterSource;
    [SerializeField] private GameObject _prefabAlien;
    [SerializeField] private GameObject[] _aliensPool = new GameObject[10];
    [SerializeField] private int _maxWaterSources = 5;
    [SerializeField] private GameObject[] _waterSource = new GameObject[5];
    [SerializeField] private int _durationInSeconds = 5;
    [SerializeField] private AudioSource _soundCollisionWithCamera;
    [SerializeField] private UIController _uiController;
    [SerializeField] private TMP_Text _txtGameOver;
    [SerializeField] private GameObject _battlefield;
    [SerializeField] private GameObject _loseCutscene;
    [SerializeField] private GameObject _winCutscene;

    private int _currentTime;

    private Debugger _debugger;
    private Spawner _spawner;
    private List<GameObject> _algaePool = new List<GameObject>();
    private List<GameObject> _waterSourcePool = new List<GameObject>();
    private Vector3 _scaleChange;
    private float _minWaterReserve;
    private bool _isTimeUp;
    private float _firstAidValue;
    private GameObject[] _iniAliensPosition = new GameObject[10];

    private void Start()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();

        _debugger = GetComponent<Debugger>();
        _spawner = GetComponent<Spawner>();

        InputManager.ListeningOneFingerTouch += TranslateOneFingerTouch;
        InputManager.ListeningTwoFingersTouch += TranslateTwoFingersTouch;
        InputManager.ListeningOneFingerDrag += ReleaseAlgaeProtection;
        InputManager.ListeningTwoFingersDrag += Shrink;

        Alien.ReportingCollision += ManageAlienCollision;

        _scaleChange = new Vector3(0.25f, 0.25f, 0.25f);
        _minWaterReserve = 60f;
        _firstAidValue = 5f;
        _iniAliensPosition = _aliensPool;   //At gameOver of each round will set the aliens at the same initial position

    }

    private void Awake()
    {
        _currentTime = -1;
        _waterSourcePool = new List<GameObject>(_waterSource);
        _isTimeUp = false;
    }

    private void OnDestroy()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();

        InputManager.ListeningOneFingerTouch -= TranslateOneFingerTouch;
        InputManager.ListeningTwoFingersTouch -= TranslateTwoFingersTouch;
        InputManager.ListeningOneFingerDrag -= ReleaseAlgaeProtection;
        InputManager.ListeningTwoFingersDrag -= Shrink;

        Alien.ReportingCollision -= ManageAlienCollision;
    }

    private void Update()
    {
        if (_currentTime < 0 && AreRestingAliens())
        {
            _currentTime = _durationInSeconds;
            StartInvasion();
        }
    }

    public void StartInvasion()
    {
        StartCoroutine(AliensIEnum());
    }
    IEnumerator AliensIEnum()
    {
        while (_currentTime >= 0)
        {
            yield return new WaitForSeconds(1f);
            _currentTime--;
        }
        SpawnAlien();
    }

    public float GetMinWaterReserve()
    {
        return _minWaterReserve;
    }

    public void ManageAlienCollision(GameObject alien, GameObject other)
    {
        //if (!other.CompareTag(_prefabAlgae.tag) && !other.CompareTag(_prefabWaterSource.tag) &&
        //    !other.CompareTag(_prefabAlien.tag) && !other.CompareTag("Border"))
        //{
        //    DebugLog($"Alien collided with {other.name}");
        //}

        if (other.gameObject.CompareTag("MainCamera"))
        {
            //DebugLog("Collided with MainCamera");
            _soundCollisionWithCamera.Play();
            //TODO: Fix not working vibration funcionality
            //Vibrator.Vibrate(500);
        }
        else if (other.gameObject.CompareTag(_prefabWaterSource.tag))
        {
            other.GetComponent<WaterSource>().PoisonWater();
            CheckWin();
        }
        else if (other.gameObject.CompareTag("Breach"))
        {
            _isTimeUp = true;
            CheckWin();
        }
    }

    private void CheckWin()
    {
        Debug.Log($"Checking win with water = {CountWaterSourceResources()}");
        if (_isTimeUp)
        {
            _battlefield.SetActive(false);
            _uiController.EnableButtons();
            if (CountWaterSourceResources() >= _minWaterReserve)
            {
                //Show Win to the user
                _winCutscene.SetActive(true);
                UpdateUIGameOver("YOU'VE SAVED EARTH!");
            }
            else
            {
                //Show Lose to the user
                _loseCutscene.SetActive(true);
                UpdateUIGameOver("GAME OVER! \nEarth is lost");
            }
            _aliensPool = _iniAliensPosition;
        }
        else
        {
            if (CountWaterSourceResources() <= 0) {
                //Show Lose to the user
                _battlefield.SetActive(false);
                _uiController.EnableButtons();
                _aliensPool = _iniAliensPosition;
                _loseCutscene.SetActive(true);
                UpdateUIGameOver("GAME OVER! \nEarth is lost");
            }
        }
    }

    private void UpdateUIGameOver(string message)
    {
        _txtGameOver.text = message;
    }

    private float CountWaterSourceResources()
    {
        float rta = 0f;
        float temp;
        foreach(GameObject waterS in _waterSourcePool)
        {
            if (waterS.activeSelf)
            {
                temp = waterS.GetComponent<WaterSource>()._health.GetLifeValue();
                if (temp > 0)
                {
                    rta += temp;
                }
            }
        }
        return rta;
    }

    public bool AreRestingAliens()
    {
        bool rta = false;
        for(int i=0; i<_aliensPool.Length && !rta; i++)
        {
            if (!_aliensPool[i].activeSelf)
            {
                rta = true;
            }
        }
        return rta;
    }

    public int CountAliensFighting()
    {
        int rta = 0;
        for (int i = 0; i < _aliensPool.Length; i++)
        {
            if (_aliensPool[i].activeSelf)
            {
                rta++;
            }
        }
        return rta;
    }

    public void ResetInvasion()
    {
        _aliensPool = _iniAliensPosition;
    }

    public void TranslateOneFingerTouch(EnhancedTouch.Touch touch, int tapsCount) {
        GameObject waterSourceFound;
        waterSourceFound = _spawner.CheckPlacement(_prefabWaterSource.tag, touch);
        switch (tapsCount) { 
            case 1:
                //one tap with one finger (tested)
                if (waterSourceFound != null)
                {
                    //DebugLog($"About to provide first aid to water");
                    ProvideFirstAid(waterSourceFound);
                    //algaeSpawned = SpawnAlgae(waterSourceFound.transform.position);
                    //if (algaeSpawned != null)
                    //{
                    //    waterSourceFound.GetComponent<WaterSource>().PurifyWater();
                    //    algaeSpawned.GetComponent<Algae>()._health.DecreaseLife(Algae._power);
                    //}
                }
                break;
            case 2:
                //DebugLog($"TWO TAPS with o n e finger"); 
                break;
        }
    }

    public void TranslateTwoFingersTouch(EnhancedTouch.Touch touch, int tapsCount)
    {
        GameObject waterSourceFound;
        GameObject algaeSpawned;
        waterSourceFound = _spawner.CheckPlacement(_prefabWaterSource.tag, touch);

        switch (tapsCount)
        {
            case 1:
                //one tap with two fingers (tested)
                if (waterSourceFound != null) //waterSource found on spot
                {
                //    if(_waterSourcePool.Count < _maxWaterSources)
                //    {
                //        SpawnWaterSource(touch);
                //    }
                //    else
                //    {
                //        //TODO: Object pooling Spawn aviso no more NEW water sources available
                //    }
                    
                //}
                //else
                //{
                    algaeSpawned = SpawnAlgae(waterSourceFound.transform.position);
                    if (algaeSpawned != null)
                    {
                        waterSourceFound.GetComponent<WaterSource>().PurifyWater();
                        algaeSpawned.GetComponent<Algae>()._health.DecreaseLife(Algae._power);
                    }

                }
                break;
            case 2:
                //TWO TAPS with two fingers 
                break;
        }
    }

    public void ReleaseAlgaeProtection(EnhancedTouch.Touch touch, int tapsCount)
    {
        //Drag with one finger
        GameObject algaeProtector;
        algaeProtector = _spawner.CheckPlacement(_prefabAlgae.tag, touch);
        if (algaeProtector != null)
        {
            algaeProtector.SetActive(false);
        }
    }

    public void ReleaseWaterSource(EnhancedTouch.Touch touch, int tapsCount)
    {
        GameObject anotherWaterSource;
        anotherWaterSource = _spawner.CheckPlacement(_prefabWaterSource.tag, touch);
        if (anotherWaterSource != null)
        {
            anotherWaterSource.SetActive(false);
        }
    }

    public void ProvideFirstAid(GameObject objectSpotted)
    {
        if (objectSpotted != null)
        {
            objectSpotted.GetComponent<WaterSource>().Aid(_firstAidValue);  
        }
    }

    public void Shrink(EnhancedTouch.Touch touch, int tapsCount)
    {
        GameObject objectSpotted;
        objectSpotted = _spawner.CheckPlacement(_prefabAlien.tag, touch);
        if (objectSpotted != null)
        {
            if((objectSpotted.transform.localScale - _scaleChange).x > 0.01f)
            {
                objectSpotted.transform.localScale -= _scaleChange;
            }
            
        }
    }

    public GameObject SpawnAlgae(Vector3 targetPosition)
    {
        //TODO: Pooling object handling
        GameObject objectSpawned = _spawner.Spawn(_prefabAlgae, targetPosition);
        if (objectSpawned != null) //if space in plane detected
        {
            AddAlgae(objectSpawned);
            //Show alge in use in UI
            _uiController.ShowAliensLeft(CountAliensFighting());
        }
        return objectSpawned;
    }

    public void SpawnWaterSource(EnhancedTouch.Touch touch)
    {
        GameObject objectSpawned = _spawner.SpawnInPlain(_prefabWaterSource, touch.finger.currentTouch.screenPosition);
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
                //Show aliens in scene in UI
                _uiController.ShowAliensLeft(CountAliensFighting());
            }
        } while (!success);
            
    }

    public GameObject CheckObjectPlacement(string prefabTag, EnhancedTouch.Touch positionToCheck)
    {
        return _spawner.CheckPlacement(prefabTag, positionToCheck);
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

    public float CalculateWaterLeft()
    {
        float rta = 0f;
        foreach(GameObject waterS in _waterSourcePool)
        {
            rta += waterS.GetComponent<WaterSource>()._health.GetLifeValue();
        }
        return rta;
    }
}
