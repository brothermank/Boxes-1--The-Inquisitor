using UnityEngine;
using System.Collections;

public class KeyBoardController : MonoBehaviour {

	public KeyCode up = KeyCode.UpArrow;
	public KeyCode down = KeyCode.DownArrow;
	public KeyCode left = KeyCode.LeftArrow;
	public KeyCode right = KeyCode.RightArrow;
	public KeyCode r = KeyCode.R;
	public GameController gc;	

	public float holdTimeForExtraMove = 0.1f;
	public float timeSinceLastMove = 0;

	void Start(){
	}

	// Update is called once per frame
	void Update () {
		if(!gc.paused)
			Controls ();
	}

	
	/// <summary>
	/// Handles input from the keyboard
	/// </summary>
	void Controls(){
		bool hasMoved = false;
		if (Input.GetKeyDown (up) || (Input.GetKey(up) && timeSinceLastMove > holdTimeForExtraMove)) {
			gc.MovePlayer(GameController.Direction.up);
			hasMoved = true;
		}
		if (Input.GetKeyDown (down) || (Input.GetKey(down) && timeSinceLastMove > holdTimeForExtraMove)) {
			gc.MovePlayer(GameController.Direction.down);
			hasMoved = true;
		}
		if (Input.GetKeyDown (left) || (Input.GetKey(left) && timeSinceLastMove > holdTimeForExtraMove)) {
			gc.MovePlayer(GameController.Direction.left);
			hasMoved = true;
		}
		if (Input.GetKeyDown (right) || (Input.GetKey(right) && timeSinceLastMove > holdTimeForExtraMove)) {
			gc.MovePlayer(GameController.Direction.right);
			hasMoved = true;
		}
		if (Input.GetKeyDown (r)) {	
			gc.restartMap();
		}

		if (hasMoved) {
			timeSinceLastMove = 0;
		} else {
			timeSinceLastMove += Time.deltaTime;
		}
	}
}
