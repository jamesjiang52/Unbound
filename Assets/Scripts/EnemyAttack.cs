using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	public float damagePerHit = 20f;
	public float attackDelay = 1f;
	public float range = 1f;

	public GameObject player;

	float attackTimer;
	bool playerInRange;
	SphereCollider attackRadius;
	PlayerHealth playerHealth;

	void Awake () {
		attackTimer = attackDelay;
		playerInRange = false;
		attackRadius = GetComponent <SphereCollider> ();
		attackRadius.radius = range;
		playerHealth = player.GetComponent <PlayerHealth> ();
	}

	void Update () {
		attackTimer += Time.deltaTime;

		if (attackTimer >= attackDelay && playerInRange)
			Attack ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject == player)
			playerInRange = true;
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject == player)
			playerInRange = false;
	}

	void Attack () {
		attackTimer = 0f;

		if (playerHealth.currentHealth > 0f)
			playerHealth.LoseHealth (damagePerHit);
	}
}
