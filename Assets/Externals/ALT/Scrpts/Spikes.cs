using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	public int damage; //데미지량
	public float damageRate; //데미지를 얼마나 자주 줄지

	//객체 저장
	//List<> things = new List<>();

	void Start()
	{
		InvokeRepeating("DealDamage", 0, damageRate);
	}
	void DealDamage()//데미지 주는함수
	{
		//for(int i = 0; i < things.const; i++)
		{
			//데미지값
			//things[i]. (damage);
		}
	}
	void OnTriggerEnter(Collider other)
	{
		//if(other.TryGetComponent(out /*인터페이스*/ damagabe))
		{
			//things.Add(damagabe);
		}
	}
	void OnTriggerExit(Collider other)
	{
		//if(other.TryGetComponent(out /**/ damagabe))
		{
			//this.Remove(damagabe);
		}
	}
}
