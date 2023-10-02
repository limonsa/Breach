using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScarceEntity : MonoBehaviour, IPlayable
{
    //Composition (part of originally IDamagable)
    public Health _health = new Health();

    public abstract void Move(Vector2 direction, Vector2 target);

    // Two Overloads
    public virtual void Move(Vector2 direction) { }
    public virtual void Move(float speed) { }

    public abstract void Shoot();

    public abstract void Die();

    public abstract void Attack(float interval);

    public abstract float GetDamagePower();
    public abstract void SetDamagePower(float damageValue);
}
