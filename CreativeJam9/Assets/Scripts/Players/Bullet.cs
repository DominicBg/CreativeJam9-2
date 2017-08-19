using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public int playerID;
	//public int damage;

	Transform tr;
	float speed;

	public void InitialiseBullet(int _playerID, Vector3 direction, float _speed)
	{
		tr = GetComponent<Transform>();

		tr.LookAt(tr.position + direction);
		speed = _speed;
		playerID = _playerID;
		//damage = _damage;
		Destroy(gameObject,2);
	}

	void Update()
	{
		tr.position += tr.forward * Time.deltaTime * speed;
	}
	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag("Wall"))
		{
			HitTarget();
		}
	}
	public void HitTarget()
	{
		//play animation

		Destroy (gameObject);
	}
}
