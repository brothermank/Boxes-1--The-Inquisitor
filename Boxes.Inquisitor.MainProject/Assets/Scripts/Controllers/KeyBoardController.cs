using UnityEngine;
using System.Collections;

public class KeyBoardController : MonoBehaviour {

	public KeyCode up = KeyCode.UpArrow;
	public KeyCode down = KeyCode.DownArrow;
	public KeyCode left = KeyCode.LeftArrow;
	public KeyCode right = KeyCode.RightArrow;
	public KeyCode r = KeyCode.R;
	public GameController gc;	

	private Score scoreSystem;

	void Start(){
		scoreSystem = GameObject.Find("gameController").GetComponentInParent<Score> ();
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
			if(gc.MovePlayer(GameController.Direction.up))
				scoreSystem.addMove();

		}
		if (Input.GetKeyDown (down)) {
			if(gc.MovePlayer(GameController.Direction.down))
				scoreSystem.addMove();
		}
		if (Input.GetKeyDown (left)) {
			if(gc.MovePlayer(GameController.Direction.left))
				scoreSystem.addMove();
		}
		if (Input.GetKeyDown (right)) {
			if(gc.MovePlayer(GameController.Direction.right))
				scoreSystem.addMove();
		}
		if (Input.GetKeyDown (r)) {	
			gc.restartMap();
		}
	}
}
