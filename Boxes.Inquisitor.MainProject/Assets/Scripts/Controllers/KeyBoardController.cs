using UnityEngine;
using System.Collections;

public class KeyBoardController : MonoBehaviour {

	public KeyCode up = KeyCode.UpArrow;
	public KeyCode down = KeyCode.DownArrow;
	public KeyCode left = KeyCode.LeftArrow;
	public KeyCode right = KeyCode.RightArrow;
	public KeyCode r = KeyCode.R;
	public GameController gc;	

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
		if (Input.GetKeyDown (up)) {
			gc.MovePlayer(GameController.Direction.up);
		}
		if (Input.GetKeyDown (down)) {
			gc.MovePlayer(GameController.Direction.down);
		}
		if (Input.GetKeyDown (left)) {
			gc.MovePlayer(GameController.Direction.left);
		}
		if (Input.GetKeyDown (right)) {
			gc.MovePlayer(GameController.Direction.right);
		}
		if (Input.GetKeyDown (r)) {	
			gc.restartMap();
		}
	}
}
