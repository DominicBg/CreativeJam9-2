using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowGameObject : MonoBehaviour {

	public Transform target;
	Transform tr;
	public Image img;

	void Start()
	{
		tr = GetComponent<Transform>();
	}
	// Update is called once per frame
	void Update ()
	{
		tr.position = Camera.main.WorldToScreenPoint(target.position);
	}
}
