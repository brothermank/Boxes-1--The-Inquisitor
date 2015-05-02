using UnityEngine;

using System.Collections;

public class CameraController : MonoBehaviour {
	
	Camera controlledCam;
	
	public float zoomSpeed = 4;
	public float baseMoveSpeed = 0.515f;
	float actualMoveSpeed{
		get{
			return baseMoveSpeed * Camera.main.orthographicSize;
		}
	}
	
	// Use this for initialization
	void Start () {
		controlledCam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		Controls ();
	}
	
	void Controls(){
		if (Input.GetKey (KeyCode.Q)) {
			controlledCam.orthographicSize += zoomSpeed * Time.deltaTime;
		} 
		else if (Input.GetKey (KeyCode.E)) {
			controlledCam.orthographicSize -= zoomSpeed * Time.deltaTime;
			if(controlledCam.orthographicSize < 1){
				controlledCam.orthographicSize = 1;
			}
		} 
		
		if (Input.GetKey (KeyCode.W)) {
			Camera.main.transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.up, actualMoveSpeed);
		} 
		else if (Input.GetKey (KeyCode.S)) {
			Camera.main.transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.down, actualMoveSpeed);
		} 
		else if (Input.GetKey (KeyCode.A)) {
			Camera.main.transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.left, actualMoveSpeed);
		} 
		else if (Input.GetKey (KeyCode.D)) {
			Camera.main.transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.right, actualMoveSpeed);
		} 
	}

	
	public static void ResizeMainCamTo(int width, int height){
		if (height * Camera.main.aspect > width) {
			Camera.main.orthographicSize = (float)height / 2;
		} else {
			Camera.main.orthographicSize = (float)width / 2 / Camera.main.aspect;
		}
	}
}
