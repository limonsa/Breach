using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algae : MonoBehaviour, IPlayable
{

    WaterSource _tempWaterSource = null;

    public void AttachWaterSource(WaterSource source)
    {
        _tempWaterSource = source;
    }

    public WaterSource GetAttachedWaterSource()
    {
        return _tempWaterSource;
    }

    public void EmptyWaterSource()
    {
        _tempWaterSource = null;
    }

    public void GetDamage(float damageValue)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
