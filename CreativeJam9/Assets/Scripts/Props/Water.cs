using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	public AnimationCurve heightCurve;
	public float maxHeight = 1;

	public float minDuration = 6;
	public float maxDuration = 8;

	public float speed = 15;
	public int waterLevel = 2; 

	CapsuleCollider collider;
	void Start()
	{
		//Destroy(gameObject,Random.Range(minDuration,maxDuration));
	}
	public void SetTrajectory(Vector3 direction, float distance)
	{
		collider = GetComponent<CapsuleCollider>();

		StartCoroutine(Trajectory(direction,distance));
	}
	IEnumerator Trajectory(Vector3 direction, float distance)
	{
		float t = 0;

		collider.enabled = false;
		while(t < distance)
		{
			float heightLerp = t/distance;

			float step = Time.deltaTime * speed;
			t += step;
			transform.position = (transform.position + direction * step).SetY(heightCurve.Evaluate(heightLerp) * maxHeight);
			yield return new WaitForEndOfFrame();
		}
		collider.enabled = true;

		//play sploush
	}

	public void Consumed()
	{
		//anim
	
		//sound

		Destroy(gameObject);
	}

}
