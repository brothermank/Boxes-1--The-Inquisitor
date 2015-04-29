using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public enum Direction{left, right, up, down};

	private LevelController lc;

	private int tilesX, tilesY;
	public string LevelString = "";
	public static string GloabalLevelString = "";
	public Block[,] LevelData;

	public bool HasWon(){
		return lc.HasWon ();
	}

	public void restartMap(){
		lc.restartMap ();
		DrawContents ();
	}

	// Use this for initialization
	void Start () {
		Debug.Log (GloabalLevelString);
		if (LevelString == "") {
			LevelData = SaveLoadManager.LoadLevel (GloabalLevelString);
		} else {
			LevelData = SaveLoadManager.LoadLevel (LevelString);
		}

		lc = new LevelController (LevelData);
		tilesX = LevelData.GetLength (0);
		tilesY = LevelData.GetLength (1);
		DrawContents ();

		Camera.main.transform.position = new Vector3 (tilesX/2, tilesY/2, -5);
	}

	public void MovePlayer(Direction d){
		Debug.Log (lc);
		if (d == Direction.left)
			lc.MoveRelatively (-1, 0);
		if (d == Direction.right)
			lc.MoveRelatively (1, 0);
		if (d == Direction.down)
			lc.MoveRelatively (0, -1);
		if (d == Direction.up)
			lc.MoveRelatively (0, 1);
	}

	public void DrawContents(){
		Block[,] LEVEL = lc.GetLevel();
		for(int x=0; x<tilesX; x++){
			for(int y=0; y<tilesY; y++){
				LEVEL[x,y].DisplayObject(x,y);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
