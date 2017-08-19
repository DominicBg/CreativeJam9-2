using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour {

	public static WaterManager instance;
	
	public GameObject waterDropPrefab;
	public int dropPercentageFromDamage = 75;
	
	public float minDistanceWaterImpact = 5;
	public float maxDistanceWaterImpact = 15;
	public float angleWaterImpact = 25;
	
	void Awake()
	{
		instance = this;
	}
	
	public void SpawnWater(int damageHit, Vector3 fromPosition, Vector3 direction)
	{
		
		int waterDropAmmount = GiveIntPercent(dropPercentageFromDamage,damageHit);
		int numberOfWaterDrop = (waterDropAmmount / 2).MinimumOne();
		
		Debug.Log(waterDropAmmount + " " + numberOfWaterDrop);
		
		for (int i = 0; i < numberOfWaterDrop; i++) 
		{
			GameObject waterDrop = Instantiate(waterDropPrefab,fromPosition,Quaternion.identity);
			Vector3 angledDirection = GameMath.RotateVectorY(Random.Range(-angleWaterImpact,angleWaterImpact),direction);
			float distance = Random.Range(minDistanceWaterImpact,maxDistanceWaterImpact);
			waterDrop.GetComponent<Water>().SetTrajectory(angledDirection,distance);
		}
	}
	
	public static int GiveIntPercent(int percent, int from)
	{
		int result = (int)((float)from * ((float)percent/100));
		if(result <= 0)
			result = 1;
		
		return result;
	}
}
