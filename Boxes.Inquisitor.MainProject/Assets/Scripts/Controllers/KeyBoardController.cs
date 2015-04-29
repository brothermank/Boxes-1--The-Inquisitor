using UnityEngine;
using System.Collections;

public class KeyBoardController : MonoBehaviour {

	public KeyCode up = KeyCode.UpArrow;
	public KeyCode down = KeyCode.DownArrow;
	public KeyCode left = KeyCode.LeftArrow;
	public KeyCode right = KeyCode.RightArrow;
	public GameController gc;


	// Update is called once per frame
	void Update () {
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
	}
}
