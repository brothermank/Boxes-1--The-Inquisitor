using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public int tilesX = 30, tilesY = 20;

	private Block.BlockType[][] LEVEL;
	private Point[] PLAYER_POS;

	// Use this for initialization
	void Start () {
		LEVEL = new Block.BlockType[tilesX] [tilesY];
	}

	//Tries to move the player relatively.
	void MoveRelatively(int dx, int dy) {
		foreach (Point p in PLAYER_POS) {
			//If this block can't move then no blocks can move
			if(!CanMove(p.x+dx,p.y+dy))return;
		}

		//If all blocks can move, then move all player blocks

		//First of all, clear all player blocks
		foreach (Point p in PLAYER_POS) {
			LEVEL[p.x, p.y] = Block.BlockType.background;
		}

		//Then fill in all the new player blocks
		foreach (Point p in PLAYER_POS) {
			LEVEL[p.x+dx, p.y+dy] = Block.BlockType.player;

			//And if its neighbors are collectibles, change them to players
			HatchUntoPlayer(p.x+dx, p.y+dy);
		}
	}

	private void HatchUntoPlayer(int x, int y){
		//If the left block is a collectible, make it a player
		if (x > 0 && LEVEL [x - 1] [y] == Block.BlockType.collectible)
			LEVEL [x - 1] [y] = Block.BlockType.player;
		
		//If the right block is a collectible, make it a player
		if (x < tilesX-1 && LEVEL [x + 1] [y] == Block.BlockType.collectible)
			LEVEL [x + 1] [y] = Block.BlockType.player;
		
		//If the upper block is a collectible, make it a player
		if (y > 0 && LEVEL [x] [y - 1] == Block.BlockType.collectible)
			LEVEL [x] [y - 1] = Block.BlockType.player;
		
		//If the down block is a collectible, make it a player
		if (y < tilesY-1 && LEVEL [x] [y + 1] == Block.BlockType.collectible)
			LEVEL [x] [y + 1] = Block.BlockType.player;
	}

	//Checks if a certain [x,y] can be moved unto by the player
	private bool CanMove(int x, int y){
		//The player can't move outside the playing field
		if (x < 0 || y < 0 || x >= tilesX || y >= tilesY)
			return false;

		//Return true if it's a player (moving unto itself is allowed) or if it's empty.
		return (LEVEL [x] [y] == Block.BlockType.background || LEVEL [x] [y] == Block.BlockType.player);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
