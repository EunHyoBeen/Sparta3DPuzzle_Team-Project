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
    [SerializeField] private Transform spawnPoint;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;

 

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
        transform.position = spawnPoint.position;
        health.curValue = health.MaxValue;
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        Debug.Log("current value "+ stamina.curValue);
        Debug.Log("amount "+ amount);
        if (stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }
}
