using UnityEngine;

using System.Collections;

public class CameraController : MonoBehaviour {

	public bool canPan = true;
	Camera controlledCam;
	public float zoomSpeed = 7;
	public float baseMoveSpeed = 0.045f;
	float actualMoveSpeed{
		get{
			return baseMoveSpeed * Camera.main.orthographicSize;
		}
	}
	
	void Start () {
		controlledCam = GetComponent<Camera> ();
	}
	void Update () {
		Controls ();
	}
	/// <summary>
	/// Checks for keyboard input, and executes assigned actions.
	/// </summary>	
	void Controls(){
		float scroll = 0;
		if((scroll = Input.GetAxis("Mouse ScrollWheel")) != 0){
			controlledCam.orthographicSize -= scroll * 1.5f;
			if(controlledCam.orthographicSize < 1){
				controlledCam.orthographicSize = 1;
			}
		}

		//movement
		if (Input.GetKey (KeyCode.W) || (Input.mousePosition.y > Screen.height && canPan)) {
			Camera.main.transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.up, actualMoveSpeed);
		} 
		if (Input.GetKey (KeyCode.S) || (Input.mousePosition.y <= 0 && canPan)) {
			Camera.main.transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.down, actualMoveSpeed);
		} 
		if (Input.GetKey (KeyCode.A) || (Input.mousePosition.x <= 0 && canPan)) {
			Camera.main.transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.left, actualMoveSpeed);
		} 
		if (Input.GetKey (KeyCode.D) || (Input.mousePosition.x > Screen.width && canPan)) {
			Camera.main.transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.right, actualMoveSpeed);
		}

		//Reset to original
		if (Input.GetKey (KeyCode.Space)) {
			float tilesX = (float) GameController.MainGC.OriginalLevelData.GetLength(0);
			float tilesY = (float) GameController.MainGC.OriginalLevelData.GetLength(1);
			Camera.main.transform.position = new Vector3 (tilesX/2f - 0.5f, tilesY/2f - 0.5f, -1.5f);
			CameraController.ResizeMainCamTo ((int)tilesX, (int)tilesY);
		}
	}

	/// <summary>
	/// Resizes the orthographic size, so all of a rectangle with the designated width and height, fits into the view.
	/// </summary>
	public static void ResizeMainCamTo(int width, int height){
		if (height * Camera.main.aspect > width) {
			Camera.main.orthographicSize = (float)height / 2;
		} else {
			Camera.main.orthographicSize = (float)width / 2 / Camera.main.aspect;
		}
	}
}
