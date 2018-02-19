using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

	public float startingHealth = 100f;
	public float startingStamina = 100f;
	public float startingMagicka = 100f;
	public float currentHealth;
	public float currentStamina;
	public float currentMagicka;

	public float staminaRegenRate = 5f;
	public float staminaRegenDelay = 5f;
	public float magickaRegenRate = 8f;
	public float magickaRegenDelay = 3f;

	public Slider healthBar;

	float staminaRegenTimer;
	float magickaRegenTimer;
	EnemyAttack enemyAttack;

	void Awake () {
		currentHealth = startingHealth;
		currentStamina = startingStamina;
		currentMagicka = startingMagicka;

		staminaRegenTimer = 0f;
		magickaRegenTimer = 0f;

		healthBar.maxValue = startingHealth;

		enemyAttack = GetComponent <EnemyAttack> ();
	}

	void Update () {
		staminaRegenTimer += Time.deltaTime;
		magickaRegenTimer += Time.deltaTime;
		
		if (staminaRegenTimer >= staminaRegenDelay) 
			RegenStamina (staminaRegenRate * Time.deltaTime);

		if (magickaRegenTimer >= magickaRegenDelay) 
			RegenMagicka (magickaRegenRate * Time.deltaTime);
	}

	public void LoseHealth (float amount) {
		if (currentHealth - amount > 0f)
			currentHealth -= amount;
		else {
			currentHealth = 0f;
			Death ();
		}

		healthBar.value = currentHealth;
	}

	public void LoseStamina (float amount) {
		if (currentStamina - amount > 0f) 
			currentStamina -= amount;
		else 
			currentStamina = 0f;

		staminaRegenTimer = 0f;
	}

	public void LoseMagicka (float amount) {
		if (currentMagicka - amount > 0f)
			currentMagicka -= amount;
		else
			currentMagicka = 0f;

		magickaRegenTimer = 0f;
	}

	void RegenStamina (float amount) {
		if (currentStamina + amount < startingStamina)
			currentStamina += amount;
		else
			currentStamina = startingStamina;
	}

	void RegenMagicka (float amount) {
		if (currentMagicka + amount < startingMagicka) 
			currentMagicka += amount;
		else
			currentMagicka = startingMagicka;
	}

	void Death () {
		LoseStamina (currentStamina);
		LoseMagicka (currentMagicka);
		staminaRegenRate = 0f;
		magickaRegenRate = 0f;

		enemyAttack.enabled = false;
	}
}
