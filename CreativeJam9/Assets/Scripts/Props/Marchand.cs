using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marchand : MonoBehaviour {


	[Header("Offer")]
	[SerializeField] Sprite[] offerSprites;
	[SerializeField] SpriteRenderer OfferSpriteRenderer;

	[Header("Param")]
	[SerializeField] float speed = 1;
	[SerializeField] float middleRadius = 5;

	[SerializeField] float minTime = 2;
	[SerializeField] float maxTime = 5;
	[SerializeField] float timeBeforeLeaving = 10;
	[SerializeField] int costSellingBonus = 50;
	float timeBeforeWait = 5;

	Transform tr;

	float desiredEulerY = 0;
	float timeWalking;
	float changeDirectionCpt;

	public enum SellingBonus{MoveSpeed,CannonSize,TriShot};
	public SellingBonus currentSellingBonus;

	public enum MarchantState{Walking,Waiting,SoldItem};
	public MarchantState state = MarchantState.Walking;

	// Use this for initialization
	void Start () 
	{
		tr = GetComponent<Transform>();
		tr.position = GameManager.instance.centerOfStage.position + (Random.onUnitSphere * middleRadius*2).SetY(0);
		tr.LookAt(GameManager.instance.centerOfStage.position + (Random.insideUnitSphere * middleRadius).SetY(0));
		OfferSpriteRenderer.gameObject.SetActive(false);

		//randomise selling bonus
		currentSellingBonus = (SellingBonus)Random.Range(0,System.Enum.GetValues(typeof(SellingBonus)).Length);
		timeBeforeWait = Random.Range(minTime, maxTime);
		Invoke("Leaving",timeBeforeLeaving);
	}
	
	void Update()
	{
		if(timeWalking > timeBeforeWait && state == MarchantState.Walking)
		{	
			SetWaiting();
			state = MarchantState.Waiting;
		}

		if(state == MarchantState.Walking)
		{
			UpdateWalking();
			timeWalking += Time.deltaTime;

		}
		else if( state == MarchantState.Waiting)
		{
			UpdateWaiting();
		}
		else if(state == MarchantState.SoldItem)
		{
			//
		}
	}
	void UpdateWalking()
	{
		tr.position += transform.forward * speed * Time.deltaTime;
	}
	void UpdateWaiting()
	{

	}

	void SetWaiting()
	{
		OfferSpriteRenderer.gameObject.SetActive(true);
		Debug.Log("I AM WAITING");
	}

	public void SoldItem()
	{
		//poff anim
		Leaving();
	}

	void Leaving()
	{
		GameManager.instance.DeactivateUIFollow();

		Destroy(gameObject);
	}

}
