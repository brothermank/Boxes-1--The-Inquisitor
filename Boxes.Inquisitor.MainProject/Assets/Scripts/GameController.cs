using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public enum Direction{left, right, up, down};

	private LevelController lc;
	public int frameWidth = 800, frameHeight = 600;
	public int tilesX = 8, tilesY = 6;
	public Block[,] LevelData;

	// Use this for initialization
	void Start () {
		lc = new LevelController (LevelData);

	}

	public void MovePlayer(Direction d){
		if (d == Direction.left)
			lc.MoveRelatively (-1, 0);
		if (d == Direction.right)
			lc.MoveRelatively (1, 0);
		if (d == Direction.up)
			lc.MoveRelatively (0, -1);
		if (d == Direction.down)
			lc.MoveRelatively (0, 1);
	}

	public void DrawContents(){
		Block[,] LEVEL = lc.GetLevel();
		for(int x=0; x<tilesX; x++){
			for(int y=0; y<tilesY; y++){
				LEVEL[x,y].DisplayObject(frameWidth*x/tilesX,frameHeight*y/tilesY);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
