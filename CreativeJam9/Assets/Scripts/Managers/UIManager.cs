using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance;
	void Awake()
	{
		instance = this;
	}

	[SerializeField]Image[] waterImage;

	public void AjustWaterLevel(int playerID, int waterLevel)
	{
		waterImage[playerID].fillAmount = (float)waterLevel / 100;
	}
}
