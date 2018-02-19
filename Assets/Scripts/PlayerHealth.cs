using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public float startingHealth = 100f;
	public float startingStamina = 100f;
	public float startingMagicka = 100f;
	public float currentHealth;
	public float currentStamina;
	public float currentMagicka;
	public Slider healthBar;
	public Slider staminaBar;
	public Slider magickaBar;
	public Text healthNumber;
	public Text staminaNumber;
	public Text magickaNumber;

	public float healthRegenRate = 2f;
	public float healthRegenDelay = 10f;
	public float staminaRegenRate = 5f;
	public float staminaRegenDelay = 5f;
	public float magickaRegenRate = 8f;
	public float magickaRegenDelay = 3f;

	public float damageHealthFlashSpeed = 5f;
	public Image damageImage;
	public Color flashColor = new Color (1f, 0f, 0f, 0.1f);
	public Color healthAlertColor = new Color (1f, 0f, 0f, 1f);

	float healthRegenTimer;
	float staminaRegenTimer;
	float magickaRegenTimer;
	PlayerMovement playerMovement;
	PlayerAttack playerAttack;
	Color healthNormalColor = new Color (1f, 1f, 1f, 1f);

	bool damaged;

	void Awake () {
		healthBar.maxValue = startingHealth;
		healthBar.value = startingHealth;
		staminaBar.maxValue = startingStamina;
		staminaBar.value = startingStamina;
		magickaBar.maxValue = startingMagicka;
		magickaBar.value = startingMagicka;

		currentHealth = startingHealth;
		currentStamina = startingStamina;
		currentMagicka = startingMagicka;
		
		healthNumber.text = Mathf.Round(currentHealth) + "/" + startingHealth;
		staminaNumber.text = Mathf.Round(currentStamina) + "/" + startingStamina;
		magickaNumber.text = Mathf.Round(currentMagicka) + "/" + startingMagicka;
		healthNumber.color = healthNormalColor;
		staminaNumber.color = healthNormalColor;
		magickaNumber.color = healthNormalColor;

		healthRegenTimer = 0f;
		staminaRegenTimer = 0f;
		magickaRegenTimer = 0f;

		playerMovement = GetComponent <PlayerMovement> ();
		playerAttack = GetComponent <PlayerAttack> ();

		damaged = false;
	}

	void Update () {
		healthRegenTimer += Time.deltaTime;
		staminaRegenTimer += Time.deltaTime;
		magickaRegenTimer += Time.deltaTime;

		if (healthRegenTimer >= healthRegenDelay) 
			RegenHealth (healthRegenRate * Time.deltaTime);

		if (staminaRegenTimer >= staminaRegenDelay) 
			RegenStamina (staminaRegenRate * Time.deltaTime);

		if (magickaRegenTimer >= magickaRegenDelay) 
			RegenMagicka (magickaRegenRate * Time.deltaTime);

		if (damaged)
			damageImage.color = flashColor;
		else 
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, damageHealthFlashSpeed * Time.deltaTime);

		damaged = false;
		
		healthNumber.text = Mathf.Round(currentHealth) + "/" + startingHealth;
		staminaNumber.text = Mathf.Round(currentStamina) + "/" + startingStamina;
		magickaNumber.text = Mathf.Round(currentMagicka) + "/" + startingMagicka;
		healthNumber.color = healthNormalColor;
		staminaNumber.color = healthNormalColor;
		magickaNumber.color = healthNormalColor;

		if (currentHealth <= 20f)
			healthNumber.color = healthAlertColor;

		if (currentStamina <= 20f)
			staminaNumber.color = healthAlertColor;

		if (currentMagicka <= 20f)
			magickaNumber.color = healthAlertColor;
	}

	public void LoseHealth (float amount) {
		damaged = true;
		if (playerAttack.isBlocking)
			amount *= (1f/playerAttack.blockReduceDamage);

		if (currentHealth - amount > 0f)
			currentHealth -= amount;
		else {
			currentHealth = 0f;
			Death ();
		}

		healthBar.value = currentHealth;
		healthRegenTimer = 0f;
		
		playerMovement.adjustedSprintSpeed = playerMovement.movementSpeed;
	}

	public void LoseStamina (float amount) {
		if (currentStamina - amount > 0f) 
			currentStamina -= amount;
		else
			currentStamina = 0f;

		staminaBar.value = currentStamina;
		staminaRegenTimer = 0f;
	}

	public void LoseMagicka (float amount) {
		if (currentMagicka - amount > 0f)
			currentMagicka -= amount;
		else
			currentMagicka = 0f;

		magickaBar.value = currentMagicka;
		magickaRegenTimer = 0f;
		
		playerMovement.adjustedSprintSpeed = playerMovement.movementSpeed;
	}

	void RegenHealth (float amount) {
		if (currentHealth + amount < startingHealth) 
			currentHealth += amount;
		else
			currentHealth = startingHealth;

		healthBar.value = currentHealth;
	}

	void RegenStamina (float amount) {
		if (currentStamina + amount < startingStamina) 
			currentStamina += amount;
		else
			currentStamina = startingStamina;

		staminaBar.value = currentStamina;
	}

	void RegenMagicka (float amount) {
		if (currentMagicka + amount < startingMagicka) 
			currentMagicka += amount;
		else
			currentMagicka = startingMagicka;

		magickaBar.value = currentMagicka;
	}

	void Death () {
		LoseStamina (currentStamina);
		LoseMagicka (currentMagicka);
		healthRegenRate = 0f;
		staminaRegenRate = 0f;
		magickaRegenRate = 0f;

		playerMovement.enabled = false;
		playerAttack.enabled = false;
	}
}
