using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	public float minDuration = 6;
	public float maxDuration = 8;


	public int waterLevel = 2; 
	void Start()
	{
		Destroy(gameObject,Random.Range(minDuration,maxDuration));
	}
	public void Consumed()
	{
		//anim
	
		//etc

		Destroy(gameObject);
	}

}
