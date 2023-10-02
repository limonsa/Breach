using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breach : MonoBehaviour
{
    [SerializeField] private int _durationInSeconds = 5;
    [SerializeField] private float _yDistanceDescend = 0.5f;

    private int _currentTime;

    public void Start()
    {
        _currentTime = -1;
    }

    private void Update()
    {
        if (_currentTime < 0)
        {
            Descend();
        }
    }

    public void Descend()
    {
        _currentTime = _durationInSeconds;
        StartCoroutine(DescendIEnum());
    }
    IEnumerator DescendIEnum()
    {
        while (_currentTime >= 0)
        {
            yield return new WaitForSeconds(1f);
            _currentTime--;

        }
        gameObject.transform.position += Vector3.down * _yDistanceDescend;
    }
}
