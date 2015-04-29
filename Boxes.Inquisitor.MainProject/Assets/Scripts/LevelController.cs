using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public int tilesX = 30, tilesY = 20;

	private Block[,] LEVEL;
	private Point[] PLAYER_POS;

	// Use this for initialization
	void Start () {
		LEVEL = new Block[tilesX, tilesY];
		PLAYER_POS = new Point[1];

		SetBlock (10, 10, Block.BlockType.player);

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
			SetBlock(p.x,p.y,Block.BlockType.background);
		}

		//Then fill in all the new player blocks
		foreach (Point p in PLAYER_POS) {
			SetBlock(p.x+dx,p.y+dy,Block.BlockType.player);

			//And if its neighbors are collectibles, change them to players
			HatchUntoPlayer(p.x+dx, p.y+dy);
		}
	}

	private void HatchUntoPlayer(int x, int y){
		//If the left block is a collectible, make it a player
		if (x > 0 && IsBlock(x-1,y,Block.BlockType.collectible))
			SetBlock(x-1,y,Block.BlockType.player);

		//If the right block is a collectible, make it a player
		if (x < tilesX-1 && IsBlock(x+1,y,Block.BlockType.collectible))
			SetBlock(x+1,y,Block.BlockType.player);
		
		//If the upper block is a collectible, make it a player
		if (y > 0 && IsBlock(x,y-1,Block.BlockType.collectible))
			SetBlock(x,y-1,Block.BlockType.player);
		
		//If the down block is a collectible, make it a player
		if (y < tilesY-1 && IsBlock(x,y+1,Block.BlockType.collectible))
			SetBlock(x,y+1,Block.BlockType.player);

	}

	private void SetBlock(int x, int y, Block.BlockType b){
		LEVEL [x, y].SetType (b);
	}

	private Block.BlockType GetBlock(int x, int y){
		return LEVEL [x, y].getType();
	}

	private bool IsBlock(int x, int y, Block.BlockType b){
		return GetBlock (x, y) == b;
	}

	//Checks if a certain [x,y] can be moved unto by the player
	private bool CanMove(int x, int y){
		//The player can't move outside the playing field
		if (x < 0 || y < 0 || x >= tilesX || y >= tilesY)
			return false;

		//Return true if it's a player (moving unto itself is allowed) or if it's empty.
		return (IsBlock(x,y,Block.BlockType.background) || IsBlock(x,y,Block.BlockType.player));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
