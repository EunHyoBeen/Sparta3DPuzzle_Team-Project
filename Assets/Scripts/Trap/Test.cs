using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamaglbe
{
	void TakePhysicalDamage(int damage);
}
public class Test : MonoBehaviour, IDamaglbe
{
	public event Action onTakeDamage;
	public void TakePhysicalDamage(int damage)
	{
		Debug.Log("데미지");
	}

}
