using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayable
{
    public static float _power;

    float GetDamagePower();
    void SetDamagePower(float damageValue);
}
