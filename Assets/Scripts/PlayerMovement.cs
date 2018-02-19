using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 6f;
	public float sprintSpeed = 12f;
	public float crouchSpeed = 2f;
	public float blockingSpeed = 2f;
	public float jumpForce = 50f;
	public float sprintStaminaCost = 10f;
	public float sprintAcceleration = 1f;
	public float adjustedSprintSpeed;
	
	float cameraRayLength = 1000f;
	int floorMask;
	PlayerHealth playerHealth;
	PlayerAttack playerAttack;
	Animator anim;
	Rigidbody rigidBody;
	bool isSprinting;
	bool canJump;

	void Awake () {
		playerHealth = GetComponent <PlayerHealth> ();
		playerAttack = GetComponent <PlayerAttack> ();
		anim = GetComponent <Animator> ();
		rigidBody = GetComponent <Rigidbody> ();

		adjustedSprintSpeed = movementSpeed;
		
		floorMask = LayerMask.GetMask ("Floor");
		
		isSprinting = false;
		canJump = true;
	}

	void Update () {
		float horizontalMovement = Input.GetAxisRaw ("Horizontal");
		float verticalMovement = Input.GetAxisRaw ("Vertical");

		rigidBody.AddForce(0, -9.81f, 0, ForceMode.Acceleration);

		Move (horizontalMovement, verticalMovement);
		Turn ();

		if (Input.GetButton ("Jump") && canJump && !(playerAttack.isBlocking)) 
			Jump ();

		if (isSprinting)
			playerHealth.LoseStamina (sprintStaminaCost * Time.deltaTime);
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag ("Ground"))
			canJump = true;
	}

	void Move (float horizontal, float vertical) {
		isSprinting = false;
		Vector3 movement = new Vector3 (horizontal, 0f, vertical);
		if ((horizontal != 0f) || (vertical != 0f))
			anim.SetBool("Running", true);
		else 
			anim.SetBool("Running", false);	

		if (Input.GetButton ("Sprint") && !(playerAttack.isBlocking) && (playerHealth.currentStamina != 0f)) {
			adjustedSprintSpeed = Mathf.Lerp (adjustedSprintSpeed, sprintSpeed, sprintAcceleration * Time.deltaTime);
			movement = movement.normalized * adjustedSprintSpeed * Time.deltaTime;
			isSprinting = true;
			anim.SetBool("Running", false);
		} else if (Input.GetButton ("Crouch")) {
			movement = movement.normalized * crouchSpeed * Time.deltaTime;
			adjustedSprintSpeed = movementSpeed;
		} else if (playerAttack.isBlocking) {
			movement = movement.normalized * blockingSpeed * Time.deltaTime;
			adjustedSprintSpeed = movementSpeed;
		} else {
			movement = movement.normalized * movementSpeed * Time.deltaTime;
			adjustedSprintSpeed = movementSpeed;
		}
		
		anim.SetBool("Sprinting", isSprinting);
		rigidBody.MovePosition (transform.position + movement);
	}

	void Jump () {
		rigidBody.AddForce (0f, jumpForce, 0f, ForceMode.Impulse);
		canJump = false;
		
		adjustedSprintSpeed = movementSpeed;
	}

	void Turn () {
		Ray cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit mousePos;

		if (Physics.Raycast (cameraRay, out mousePos, cameraRayLength, floorMask)) {
			Vector3 playerToMouse = mousePos.point - transform.position;
			playerToMouse.y = 0f;
			Quaternion playerRotate = Quaternion.LookRotation (playerToMouse);
			rigidBody.MoveRotation (playerRotate);
		}
	}
}