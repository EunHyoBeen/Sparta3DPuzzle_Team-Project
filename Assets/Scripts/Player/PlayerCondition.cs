using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(int  damage);
}

public class PlayerCondition : MonoBehaviour , IDamageable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;

    // Update is called once per frame
    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(health.curValue <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("ав╬З╢ы.");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
}
