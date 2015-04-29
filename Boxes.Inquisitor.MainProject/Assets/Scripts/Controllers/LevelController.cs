using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController {

	public int tilesX, tilesY;

	private Block[,] LEVEL;
	private List<Point> PLAYER_POS;

	// Use this for initialization
	void Start () {
		LEVEL = new Block[tilesX, tilesY];
		PLAYER_POS = new List<Point> ();
		AddPlayerAtPosition (tilesX / 2, tilesY / 2);
		SetBlock (10, 10, Block.BlockType.player);

	}

	public Block[,] GetLevel(){
		return LEVEL;
	}

	public LevelController(Block[,] LevelData){
		LEVEL = LevelData;
		tilesX = LevelData.GetLength (0);
		tilesY = LevelData.GetLength (1);
	}

	public void AddPlayerAtPosition(int x, int y){
		SetBlock (x, y, Block.BlockType.player);
		AddPlayerPosition(x,y);
	}

	//Tries to move the player relatively.
	public void MoveRelatively(int dx, int dy) {
		List<Point> q = PLAYER_POS;
		PLAYER_POS.Clear ();

		foreach (Point p in q) {
			//If this block can't move then no blocks can move
			if(!CanMove(p.x+dx,p.y+dy))return;
		}

		//If all blocks can move, then move all player blocks

		//First of all, clear all player blocks
		foreach (Point p in q) {
			SetBlock(p.x,p.y,Block.BlockType.background);
		}

		//Then fill in all the new player blocks
		foreach (Point p in q) {
			SetBlock(p.x+dx,p.y+dy,Block.BlockType.player);

			//If it doesn't contains this point, add it to the player positions
			Point x = new Point(p.x+dx,p.y+dy);
			AddPlayerPosition(x);

			//And if its neighbors are collectibles, change them to players
			HatchUntoPlayer(p.x+dx, p.y+dy);
		}
	}
	private void AddPlayerPosition(Point p){
		if(!PLAYER_POS.Contains(p))
			PLAYER_POS.Add(p);
	}

	private void AddPlayerPosition(int x, int y){
		AddPlayerPosition (new Point (x, y));
	}

	private void HatchUntoPlayer(int x, int y){
		//If the left block is a collectible, make it a player
		if (x > 0 && IsBlock (x - 1, y, Block.BlockType.collectible)) {
			SetBlock (x - 1, y, Block.BlockType.player);
			AddPlayerPosition(x-1,y);
		}

		//If the right block is a collectible, make it a player
		if (x < tilesX - 1 && IsBlock (x + 1, y, Block.BlockType.collectible)) {
			SetBlock (x + 1, y, Block.BlockType.player);
			AddPlayerPosition(x+1,y);
		}
		
		//If the upper block is a collectible, make it a player
		if (y > 0 && IsBlock (x, y - 1, Block.BlockType.collectible)) {
			SetBlock (x, y - 1, Block.BlockType.player);
			AddPlayerPosition(x,y-1);
		}
		
		//If the down block is a collectible, make it a player
		if (y < tilesY - 1 && IsBlock (x, y + 1, Block.BlockType.collectible)) {
			SetBlock (x, y + 1, Block.BlockType.player);
			AddPlayerPosition(x,y+1);
		}

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
