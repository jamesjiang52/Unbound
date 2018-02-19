using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	public float damagePerHit = 20f;
	public float powerDamagePerHit = 40f;
	public float attackDelay = 1f;
	public float powerAttackDelay = 2f;
	public float powerAttackStaminaCost = 40f;
	public float blockReduceDamage = 4f;
	public float range = 1f;
	public bool isBlocking;

	float attackTimer;
	float powerAttackHoldTimer = 0.75f;
	float powerAttackHoldTimerTimer = 0f;
	bool enemyInRange;
	bool lastAttackPower;
	bool attackAnimToggle = true;
	SphereCollider attackRadius;
	GameObject enemy;
	PlayerHealth playerHealth;
	PlayerMovement playerMovement;
	Animator anim;
	EnemyHealth enemyHealth;

	void Awake () {
		attackTimer = attackDelay;
		enemyInRange = false;
		lastAttackPower = false;
		attackRadius = GetComponent <SphereCollider> ();
		attackRadius.radius = range;
		playerHealth = GetComponent <PlayerHealth> ();
		playerMovement = GetComponent <PlayerMovement> ();
		anim = GetComponent <Animator> ();
		isBlocking = false;
	}
	
	void Update () {
		isBlocking = false;
		attackTimer += Time.deltaTime;
		
		if (Input.GetButton ("Fire2"))
			isBlocking = true;

		if (Input.GetButton ("Fire1") && !(isBlocking)) {
			if (((attackTimer >= attackDelay) && !(lastAttackPower)) || ((attackTimer >= powerAttackDelay) && lastAttackPower))
				powerAttackHoldTimerTimer += Time.deltaTime;
		}

		if (Input.GetButtonUp ("Fire1") && !(isBlocking) && powerAttackHoldTimerTimer <= powerAttackHoldTimer) {
			if (((attackTimer >= attackDelay) && !(lastAttackPower)) || ((attackTimer >= powerAttackDelay) && lastAttackPower))
				Attack ();
		}

		if (powerAttackHoldTimerTimer > powerAttackHoldTimer)
			PowerAttack ();
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			enemyInRange = true;
			enemy = other.gameObject;
			enemyHealth = enemy.GetComponent <EnemyHealth> ();
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Enemy"))
			enemyInRange = false;
	}

	void Attack () {
		attackTimer = 0f;
	
		if (enemyInRange && enemyHealth.currentHealth > 0f)
			enemyHealth.LoseHealth (damagePerHit);
		
		if (attackAnimToggle) {
			anim.SetTrigger ("Attack1");
			attackAnimToggle = false;
		} else {
			anim.SetTrigger ("Attack2");
			attackAnimToggle = true;
		}

		lastAttackPower = false;
		powerAttackHoldTimerTimer = 0f;
		
		playerMovement.adjustedSprintSpeed = playerMovement.movementSpeed;
	}

	void PowerAttack () {
		attackTimer = 0f;
		if (enemyInRange && enemyHealth.currentHealth > 0f)
			enemyHealth.LoseHealth (powerDamagePerHit);
		
		anim.SetTrigger ("PowerAttack");
		playerHealth.LoseStamina (powerAttackStaminaCost);
		lastAttackPower = true;
		powerAttackHoldTimerTimer = 0f;
		
		playerMovement.adjustedSprintSpeed = playerMovement.movementSpeed;
	}
}
