using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterSource : ScarceEntity
{
    [SerializeField] private int durationInSeconds = 5;

    public static UnityAction<GameObject> EndingLifespanWaterSource;

    private int currentTime;
    private Vector3 scaleChange;

    public void Start()
    {
        _health.AddLife(100f);
        scaleChange = new Vector3(-(gameObject.transform.localScale.x / durationInSeconds),
                                -(gameObject.transform.localScale.y / durationInSeconds),
                                -(gameObject.transform.localScale.z / durationInSeconds));
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
        return _health.DecreaseLife(value);
    }


    public void StartCountdown()
    {
        currentTime = durationInSeconds;
        StartCoroutine(TimeIEnum());
    }
    IEnumerator TimeIEnum()
    {
        while (currentTime >= 0)
        {
            //timeImage.fillAmount = Mathf.InverseLerp(0, durationInSeconds, currentTime);
            gameObject.transform.localScale += scaleChange;
            yield return new WaitForSeconds(1f);
            currentTime--;
            //TODO: Do something to call attention over it as it is about to disapear
            //    if (currentTime == (int)100 / (durationInSeconds * stressTimePercentage))
            //{
            //    timeText.color = Color.red;
            //}
        }
        EndingLifespanWaterSource?.Invoke(gameObject);
    }

    public override void Attack(float interval)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void GetDamage(float damage)
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
}
