using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

	[Header("Info")]
	public int playerID;
	Player player;

	[Header("Component")]
	Transform tr;
	Rigidbody rb;
	BoxCollider boxCollider;
	MeshRenderer meshRenderer;
	Color baseColor;

	[SerializeField] Transform cactusTr;
	[SerializeField] Transform baseTr;
	[SerializeField] Transform cannonShootPosition;
	UIFollowGameObject BuyUI;

	[SerializeField] GameObject bulletPrefab;
	//[SerializeField] GameObject waterPrefab;

	[Header("Stats")]
	public int waterLevel = 100; //HP
	public float moveSpeed;
	public float rotationSpeed;
	public float cannonBallSpeed;
	public int minDamage;

	[Header("Cooldown")]
	public float cooldownShoot = 0.5f;
	public float cooldownDash = 0.5f;
	public float cooldownKnockBack = 0.5f;

	[Header("Dash/Knock")]
	public float dashForce = 15;
	public float dashImmunityTime = 0.2f;
	public float knockbackForce = 15;
	public int dropPercentage = 30;
	public float stunnedDelay = 1;
	public Color knockbackColor = Color.red;
	//public int damage = 10;

	[Header("Scaling")]
	public Vector3 basicScale = Vector3.one;
	public Vector3 waterWeightScaling = Vector3.one;

	[Header("Sliding")]
	public float slideDirectionFactor;
	public float slideFactor;

	[Header("Marchand")]
	public float currentMarchandTime;
	public float marchandBuyTime;
	private float marchandBuySpeed;

	[Header("Bonus")]
	public float bonusMoveSpeed = 8;
	public float bonusCannonBallSize = 8;

	private bool gotTriShot;
	private bool gotBonusMoveSpeed;
	private bool gotBigCannonBall;

	private Vector3 lastMoveInputVector;
	private Vector3 slideVector;

	//Buttons
	private Vector3 shootDir;
	private bool onShootCooldown; 
	private bool releasedShootTrigger = true;

	private bool onDashCooldown; 
	private bool releasedDashTrigger = true;

	private bool isStunned = false;
	private bool isDashing = false;
	private bool isDead;
	void Start () 
	{
		rb = GetComponent<Rigidbody>(); 
		tr = GetComponent<Transform>();
		boxCollider = GetComponent<BoxCollider>();
		meshRenderer = cactusTr.GetComponent<MeshRenderer>();
		player = ReInput.players.GetPlayer(playerID);
		UpdateWaterLevel();
		baseColor = meshRenderer.material.color;

		BuyUI = GameManager.instance.UIFollow[playerID];
		BuyUI.target = transform;
		BuyUI.gameObject.SetActive(false);

		marchandBuySpeed = 1 / marchandBuyTime;
	}
	
	void Update () 
	{
		if(isStunned)
			return;

		//if(isDead)
		//	return;


	
		InputMouvement();
		InputAim();
		RotateBase();
		InputShoot();
		InputDash();
	}

	void InputMouvement()
	{
		Vector3 moveVector = new Vector3(player.GetAxis("MoveX"),0,player.GetAxis("MoveY"));

		if(moveVector.magnitude > 1)
			moveVector.Normalize();

		//save for dash
		lastMoveInputVector = moveVector;

		if(moveVector.magnitude == 0)
		{		
			//slow down sliding
			slideVector *= slideFactor;
		}
		else
		{
			//change direction slide
			slideVector += moveVector * slideDirectionFactor;
			if(slideVector.magnitude > 1)
				slideVector.Normalize();
		}

		float currentMoveSpeed = (!gotBonusMoveSpeed) ? moveSpeed : bonusMoveSpeed;
		tr.position += (moveVector + slideVector) * currentMoveSpeed * Time.deltaTime;
	}

	void InputAim()
	{
		Vector3 aimVector = new Vector3(player.GetAxis("AimX"),0,player.GetAxis("AimY"));

		if(aimVector.magnitude < .3)
			return;

		aimVector.Normalize();
		shootDir = aimVector;
	}
	void RotateBase()
	{
		float step = rotationSpeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(baseTr.forward, shootDir, step, 0.0f);
		baseTr.rotation = Quaternion.LookRotation(newDir);
	}
	#region Shoot
	void InputShoot()
	{
		if(player.GetAxis("Shoot") > .5f)
		{
			if(!onShootCooldown && releasedShootTrigger)
			{
				releasedShootTrigger = false;
				ShootBullet();
				StartCoroutine(CooldownShoot());
			}
		}
		else
		{
			releasedShootTrigger = true;
		}
	}
	IEnumerator CooldownShoot()
	{
		onShootCooldown = true;
		yield return new WaitForSeconds(cooldownShoot);
		onShootCooldown = false;
	}
	void ShootBullet()
	{
		GameObject bullet = Instantiate(bulletPrefab, cannonShootPosition.position, Quaternion.identity);
		bullet.GetComponent<Bullet>().InitialiseBullet(playerID,baseTr.forward,cannonBallSpeed);
	
		if(gotBigCannonBall)
			bullet.transform.localScale = Vector3.one * bonusCannonBallSize;
	}
	#endregion

	#region Dash
	void InputDash()
	{
		if(player.GetAxis("Dash") > .5f)
		{
			if(!onDashCooldown && releasedDashTrigger)
			{
				releasedDashTrigger = false;
				Dash();
				StartCoroutine(CooldownDash());
				StartCoroutine(DashImmunity());
			}
		}
		else
		{
			releasedDashTrigger = true;
		}

	}
	void Dash()
	{
		float realForce = ((gotBonusMoveSpeed) ? dashForce * bonusMoveSpeed * 0.2f : dashForce);
		rb.AddForce((lastMoveInputVector) * realForce,ForceMode.Impulse);
	}
	IEnumerator CooldownDash()
	{
		onDashCooldown = true;
		yield return new WaitForSeconds(cooldownDash);
		onDashCooldown = false;
	}
	IEnumerator DashImmunity()
	{
		isDashing = true;
		yield return new WaitForSeconds(dashImmunityTime);
		isDashing = false;
	}

	#endregion


	#region Bullet
	void HitByBullet(Bullet bullet)
	{
		if(bullet.playerID != playerID && !isDashing)
		{
			//knockback
			rb.AddForce((bullet.transform.forward) * knockbackForce,ForceMode.Impulse);
			Damaged(bullet);
		}
	}
	void Damaged(Bullet bullet)
	{
		//get hit > loose 15% of total
		int damageHit = WaterManager.GiveIntPercent(dropPercentage,waterLevel).Minimum(minDamage);

		waterLevel -= damageHit;
		
		if(waterLevel <= 0)
		{
			waterLevel = 0;
			DeathPlayer();
		}
		else
		{
			StartCoroutine(Stunned());
		}

		WaterManager.instance.SpawnWater(damageHit,tr.position,-bullet.transform.forward);
		UpdateWaterLevel();

		float intensity = (float)damageHit / 100;;
		float duration = (float)damageHit / 100;
		GameEffect.Shake(Camera.main.gameObject,intensity,duration,true);
		bullet.HitTarget();
	}
	IEnumerator Stunned()
	{
		isStunned = true;
		boxCollider.enabled = false;
		int numberOfFlash = 5;
		float fractionStunDelay = stunnedDelay / numberOfFlash * .5f;

		for (int i = 0; i < numberOfFlash; i++) 
		{
			meshRenderer.material.SetColor("_Color",knockbackColor);
			yield return new WaitForSeconds(fractionStunDelay);

			meshRenderer.material.SetColor("_Color",baseColor);
			yield return new WaitForSeconds(fractionStunDelay);
		}
		//yield return new WaitForSeconds(stunnedDelay);
		isStunned = false;
		yield return new WaitForSeconds(fractionStunDelay);

		meshRenderer.material.SetColor("_Color",baseColor);

		boxCollider.enabled = true;
	}

	void DeathPlayer()
	{
		isDead = true;
		gameObject.SetActive(false);
	}
	#endregion

	#region water
	void GrabWater(Water water)
	{
		waterLevel += water.waterLevel;
		UpdateWaterLevel();
		water.Consumed();
	}

	void UpdateWaterLevel()
	{
		if(waterLevel > 100)
			waterLevel = 100;

		cactusTr.localScale = basicScale + ((float)waterLevel * waterWeightScaling);

		UIManager.instance.AjustWaterLevel(playerID,waterLevel);
	}
	#endregion 

	#region Marchand/Buy
	void NearMarchand(Marchand currentMarchand)
	{
		if(currentMarchand.state != Marchand.MarchantState.Waiting || !player.GetButton("Buy"))
			return;
	
		currentMarchandTime += marchandBuySpeed * Time.deltaTime;
		if(currentMarchandTime > 1)
		{
			currentMarchand.SoldItem();
			currentMarchandTime = 0;
			BuyBonus (currentMarchand.currentSellingBonus);
		}
		UpdateMarchandUI();
	}
	void ExitMarchand()
	{
		currentMarchandTime = 0;
		BuyUI.gameObject.SetActive(false);
	}
	void UpdateMarchandUI()
	{
		BuyUI.gameObject.SetActive(true);
		BuyUI.img.fillAmount = currentMarchandTime;

	}
	void BuyBonus(Marchand.SellingBonus bonus)
	{
		switch(bonus)
		{
		case Marchand.SellingBonus.MoveSpeed:
			gotBonusMoveSpeed = true;
			break;
		case Marchand.SellingBonus.CannonSize:
			gotBigCannonBall = true;
			break;
		case Marchand.SellingBonus.TriShot:
			gotTriShot = true;
			break;
		}
	}
	#endregion


	#region Collider
	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag("Bullet"))
		{
			HitByBullet(col.GetComponent<Bullet>());
		}
		else if(col.CompareTag("Water") && !isStunned)
		{
			GrabWater(col.GetComponent<Water>());
		}
	}

	void OnTriggerStay(Collider col)
	{
		if(col.CompareTag("Marchand"))
		{
			NearMarchand(col.GetComponent<Marchand>());
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(col.CompareTag("Marchand"))
		{
			ExitMarchand();
		}
	}

	#endregion

}
