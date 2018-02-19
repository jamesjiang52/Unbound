using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceCamera : MonoBehaviour {
	
	Camera mainCamera;

	void Awake () {
		mainCamera = Camera.main;
	}

	void Update () {
		transform.LookAt (transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
	}
}
