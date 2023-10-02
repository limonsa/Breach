using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Alien : ScarceEntity, IPlayable
{
    [SerializeField] private int _durationInSeconds = 5;
    [SerializeField] private float _speed = 3f;

    public static UnityAction<GameObject, GameObject> ReportingCollision;

    private int _currentTime;
    private Vector3 _scaleChange;

    public static float _power;

    public void Start()
    {
        _health.AddLife(100f);
        _scaleChange = new Vector3(0.10f, 0.10f, 0.10f);
        _currentTime = -1;
        _power = 20f;
    }

    private void Update()
    {
        if (_currentTime < 0)
        {
            StartGrowing();
        }
        MoveRandomly();
    }

    public void StartGrowing()
    {
        _currentTime = _durationInSeconds;
        StartCoroutine(TimeIEnum());
    }
    IEnumerator TimeIEnum()
    {
        while (_currentTime >= 0)
        {
            yield return new WaitForSeconds(1f);
            _currentTime--;

        }
        gameObject.transform.localScale += _scaleChange;
    }

    private void MoveRandomly()
    {
        Vector3 direction = new Vector3(0, 0, Random.Range(0, 1.0f));
        transform.Translate(direction * _speed * Time.deltaTime);
        transform.Rotate(0, Random.Range(0, 1.0f) * 10 * Time.deltaTime, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        transform.Rotate(0f, 90.0f, 0f, Space.Self);
        if (!collision.gameObject.CompareTag("Border"))
        {
            ReportingCollision?.Invoke(gameObject, collision.gameObject);
        }
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
