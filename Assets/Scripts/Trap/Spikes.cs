using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	public int damage; //?��?지??
	public float damageRate; //?��?지�??�마???�주 줄�?

	//객체 ?�??
	List<IDamageable> things = new List<IDamageable>();

	void Start()
	{
		InvokeRepeating("DealDamage", 0, damageRate);
	}
	void DealDamage()//?��?지 주는?�수
	{
		for (int i = 0; i < things.Count; i++)
		{
			//?��?지�?
			things[i].TakePhysicalDamage(damage);
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out IDamageable damagable))
		{
			things.Add(damagable);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out IDamageable damagable))
		{
			things.Remove(damagable);
		}
	}
}
