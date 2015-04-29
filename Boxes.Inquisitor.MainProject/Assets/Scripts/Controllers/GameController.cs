using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public enum Direction{left, right, up, down};

	private LevelController lc;

	private int tilesX, tilesY;
	public string LevelString = "";
	public Block[,] LevelData;

	public bool HasWon(){
		return lc.HasWon ();
	}

	// Use this for initialization
	void Start () {
		LevelData = SaveLoadManager.LoadMap (LevelString);
		lc = new LevelController (LevelData);
		tilesX = LevelData.GetLength (0);
		tilesY = LevelData.GetLength (1);
		DrawContents ();
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
