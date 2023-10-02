using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algae : MonoBehaviour, IPlayable
{
    [SerializeField] private int _durationInSeconds = 5;
    public Health _health = new Health();
    public static float _power;

    private int _currentTime;


    // Start is called before the first frame update
    void Start()
    {
        _power = 25f;
        _currentTime = -1;
    }

    private void Update()
    {
        if (_currentTime < 0)
        {
            Protect();
        }
    }

    public void Protect()
    {
        _currentTime = _durationInSeconds;
        StartCoroutine(ProtectIEnum());
    }
    IEnumerator ProtectIEnum()
    {
        while (_currentTime >= 0)
        {
            yield return new WaitForSeconds(1f);
            _currentTime--;

        }
        gameObject.SetActive(false);
    }
    public float GetDamagePower()
    {
        throw new System.NotImplementedException();
    }

    public void SetDamagePower(float damageValue)
    {
        throw new System.NotImplementedException();
    }
}
