using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health
{
    private float _life;    //min[0-100]max

    


    public Health()
    {
        _life = 0;
    }

    public void AddLife(float value)
    {
        if (_life + value > 100)
        {
            _life = 100f;
        }
        else
        {
            _life += value;
        }
    }

    /// <summary>
    /// Decreases the life in the amount of the value passed.
    /// Life is a porcentage with min value of 0 and max value of 100
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true if after decreasing life it remains positive > 0 (alive)
    /// false if after decreasing life is 0 or negative (dead)</returns>
    public bool DecreaseLife(float value)
    {
        bool isAlive = true;
        if (_life - value <= 0)
        {
            _life = 0f;
            isAlive = false;
        }
        else
        {
            _life -= value;
        }
        return isAlive;
    }

    public float GetLifeValue() {
        return _life;
    }
}
