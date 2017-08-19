using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public Transform centerOfStage;

	public UIFollowGameObject[] UIFollow;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		OnGameStart();
	}

	void OnGameStart()
	{


	}

	public void DeactivateUIFollow()
	{
		foreach(UIFollowGameObject ui in UIFollow)
			ui.gameObject.SetActive(false);
	}
}
