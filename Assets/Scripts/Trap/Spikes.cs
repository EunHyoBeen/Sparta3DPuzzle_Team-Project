using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	public int damage; //?°ë?ì§€??
	public float damageRate; //?°ë?ì§€ë¥??¼ë§ˆ???ì£¼ ì¤„ì?

	//ê°ì²´ ?€??
	List<IDamageable> things = new List<IDamageable>();

	void Start()
	{
		InvokeRepeating("DealDamage", 0, damageRate);
	}
	void DealDamage()//?°ë?ì§€ ì£¼ëŠ”?¨ìˆ˜
	{
		for (int i = 0; i < things.Count; i++)
		{
			//?°ë?ì§€ê°?
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
