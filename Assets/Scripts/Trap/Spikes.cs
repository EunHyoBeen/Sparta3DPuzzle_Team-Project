using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	public int damage; //데미지량
	public float damageRate; //데미지를 얼마나 자주 줄지

	//객체 저장
	List<IDamaglbe> things = new List<IDamaglbe>();

	void Start()
	{
		InvokeRepeating("DealDamage", 0, damageRate);
	}
	void DealDamage()//데미지 주는함수
	{
		for (int i = 0; i < things.Count; i++)
		{
			//데미지값
			things[i].TakePhysicalDamage(damage);
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out IDamaglbe damagable))
		{
			things.Add(damagable);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out IDamaglbe damagable))
		{
			things.Remove(damagable);
		}
	}
}
