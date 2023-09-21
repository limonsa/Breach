using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : ScarceEntity, IPlayable
{
    [SerializeField] private int durationInSeconds = 5;

    private int currentTime;
    private Vector3 scaleChange;

    public void Start()
    {
        _health.AddLife(100f);
        scaleChange = new Vector3((gameObject.transform.localScale.x / durationInSeconds),
                                (gameObject.transform.localScale.y / durationInSeconds),
                                (gameObject.transform.localScale.z / durationInSeconds));
    }

    private void OnEnable()
    {
        StartGrowing();
    }

    public void StartGrowing()
    {
        currentTime = durationInSeconds;
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
        gameObject.transform.localScale += scaleChange;
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
