using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterSource : ScarceEntity
{
    [SerializeField] private int _durationInSeconds = 5;
    [SerializeField] private Material _goodWater;
    [SerializeField] private Material _badWater;
    [SerializeField] private MeshRenderer _water;
    [SerializeField] private Material _goodGrass;
    [SerializeField] private Material _badGrass;
    [SerializeField] private MeshRenderer _grass;

    public static UnityAction<GameObject> EndingLifespanWaterSource;
    public static UnityAction UpdatingHealth;

    private int _currentTime;
    private int _currentDangerTime = 2;
    private Vector3 _scaleChange;
    private bool _isPoisoned;
    private float _lowResource;
    //private float _contraction = -0.5f;
    private bool _contraction = true;
    private Vector3 _localScaleLog;

    public void Start()
    {
        _health.AddLife(100f);
        _isPoisoned = false;
        _lowResource = 60f; //Resulting effect combined with MIN WATERPOWER from GameManager
        _currentTime = -1;
        _currentDangerTime = -1;
        _localScaleLog = gameObject.transform.localScale;
        _scaleChange = new Vector3(-(gameObject.transform.localScale.x / _durationInSeconds),
                                -(gameObject.transform.localScale.y / _durationInSeconds),
                                -(gameObject.transform.localScale.z / _durationInSeconds));
    }

    private void Update()
    {
        if (_health.GetLifeValue() < _lowResource && _currentDangerTime < 0)
        {
            //ShowDirtyWater();
            Beat();
        }
    }

    private void Beat()
    {
        _currentTime = 10;
        StartCoroutine(BeatIEnum());
    }

    IEnumerator BeatIEnum()
    {
        while (_currentDangerTime >= 0)
        {
            yield return new WaitForSeconds(1f);
            _currentDangerTime--;

        }

        if (_contraction)
        {
            gameObject.transform.localScale -= gameObject.transform.localScale / 4;
        }
        else
        {
            gameObject.transform.localScale = _localScaleLog;
        }
        _contraction = !_contraction;
    }

    /// <summary>
    /// Reduces the life of the WaterSource in the amount of the value passed.
    /// which is a porcentage with min value of 0 and max value of 100
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true the water source life ends up positive > 0 (alive)
    /// false otherwise (dead)</returns>
    public bool Reduce(float value)
    {
        bool rta;
        rta = _health.DecreaseLife(value);
        UpdatingHealth?.Invoke();
        return rta;
    }


    public void StartCountdown()
    {
        _currentTime = _durationInSeconds;
        StartCoroutine(TimeIEnum());
    }
    IEnumerator TimeIEnum()
    {
        while (_currentTime >= 0)
        {
            //timeImage.fillAmount = Mathf.InverseLerp(0, durationInSeconds, currentTime);
            gameObject.transform.localScale += _scaleChange;
            yield return new WaitForSeconds(1f);
            _currentTime--;
            //TODO: Do something to call attention over it as it is about to disapear
            //    if (currentTime == (int)100 / (durationInSeconds * stressTimePercentage))
            //{
            //    timeText.color = Color.red;
            //}
        }
        EndingLifespanWaterSource?.Invoke(gameObject);
    }

    public bool GetIsPoisoned()
    {
        return _isPoisoned;
    }

    public void PoisonWater()
    {
        bool alive;

        _isPoisoned = true;
        alive = _health.DecreaseLife(Alien._power);
        if (alive)
        {
            _water.material = _badWater;
            _grass.material = _badGrass;
        }
        else
        {
            gameObject.SetActive(false);
        }
        UpdatingHealth?.Invoke();
    }

    public void PurifyWater()
    {
        _health.AddLife(Algae._power);
        ShowCleanWater();
        UpdatingHealth?.Invoke();
    }

    public void Aid(float value)
    {
        _health.AddLife(value);
        ShowCleanWater();
        UpdatingHealth?.Invoke();
    }

    public void ShowCleanWater()
    {
        _isPoisoned = false;
        _water.material = _goodWater;
        _grass.material = _goodGrass;
    }

    public void ShowDirtyWater()
    {
        _isPoisoned = true;
        _water.material = _badWater;
        _grass.material = _badGrass;
    }

    public override void Attack(float interval)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void Move(Vector2 direction, Vector2 target)
    {
        throw new System.NotImplementedException();
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    public override float GetDamagePower()
    {
        throw new System.NotImplementedException();
    }

    public override void SetDamagePower(float damageValue)
    {
        throw new System.NotImplementedException();
    }
}
