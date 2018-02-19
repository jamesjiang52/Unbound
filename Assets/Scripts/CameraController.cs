using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float smoothing = 5f;

	Vector3 offset;

	void Start () {
		offset = transform.position - player.transform.position;
	}

	void Update () {
		Vector3 targetPos = player.transform.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetPos, smoothing * Time.deltaTime);
	}
}
