using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController {

	public int tilesX, tilesY;

	private Block[,] LEVEL;
	private List<Point> PLAYER_POS, GOAL_POS;
	

	/// <summary>
	/// Gets the 2-dimensional Block-array associated with this LevelController
	/// </summary>
	public Block[,] GetLevel(){
		return LEVEL;
	}

	/// <summary>
	/// Instantiates a new LevelController with the specified levelData
	/// </summary>
	public LevelController(Block[,] LevelData){
		LEVEL = LevelData;

		tilesX = LevelData.GetLength (0);
		tilesY = LevelData.GetLength (1);

		PLAYER_POS = new List<Point> ();
		GOAL_POS = new List<Point> ();

		FindPlayerPositions ();
		FindGoalPositions ();
	}

	/// <summary>
	/// Iterates through the level-data array and adds all player blocks to PLAYER_POS
	/// </summary>
	private void FindPlayerPositions(){
		for (int x=0; x<tilesX; x++) {
			for (int y=0; y<tilesY; y++) {
				if(IsBlock(x,y,Block.BlockType.player)){
					AddPlayerPosition(x,y);
				}
			}
		}
	}

	/// <summary>
	/// Iterates through the level-data array and adds all goal blocks to GOAL_POS
	/// </summary>
	private void FindGoalPositions(){
		for (int x=0; x<tilesX; x++) {
			for (int y=0; y<tilesY; y++) {
				if(IsBlock(x,y,Block.BlockType.goal)){
					AddGoalPosition(x,y);
				}
			}
		}
	}

	public bool HasWon(){

		//Are there equally many goal positions and player positions?
		//If not, just skip already
		if (PLAYER_POS.Count != GOAL_POS.Count)
			return false;

		//Otherwise check if all goal positions match with player positions
		//For every goal block...
		foreach (Point goal in GOAL_POS) {

			bool hasPartner = false;

			//Check through all player blocks and find a match
			foreach (Point player in PLAYER_POS) {

				//If there is a match, just skip this
				if(player.x==goal.x && player.y==goal.y){
					hasPartner=true;
					break;
				}
			}

			//If any one goal does not have a partner, then the player has not won yet
			if(!hasPartner)return false;
		}

		//But if all goals have had partners, and there are as many goal blocks as player blocks, then the player has won.
		return true;
	}
	private void Win(){
		Application.LoadLevel (0);
	}

	private void setGoalBlocksBack(){
		foreach (Point goal in GOAL_POS) {
			if(IsBlock (goal.x,goal.y,Block.BlockType.background))
				SetBlock(goal.x,goal.y,Block.BlockType.goal);
		}
	}

	/// <summary>
	/// Adds a new Player object at a specified [x,y] position in the array.	
	/// </summary>
	public void AddPlayerAtPosition(int x, int y){
		SetBlock (x, y, Block.BlockType.player);
		AddPlayerPosition(x,y);
	}

	/// <summary>
	/// Attempts to move the player in a relative direction. If the path is obstructed, the player will not move. 	
	/// </summary>
	public void MoveRelatively(int dx, int dy) {
		List<Point> q = new List<Point>(PLAYER_POS);

		foreach (Point p in q) {
			//If this block can't move then no blocks can move

			if(!CanMove(p.x+dx,p.y+dy)){
				return;
			}
		}
		
		PLAYER_POS.Clear ();

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

		setGoalBlocksBack ();
		bool hasWon = HasWon ();
		Debug.Log ("won? " + hasWon);
		if (hasWon)
			Win ();
	}

	/// <summary>
	/// Atte	
	/// </summary>
	private void AddPlayerPosition(Point p){
		if(!PLAYER_POS.Contains(p))
			PLAYER_POS.Add(p);
		HatchUntoPlayer (p.x, p.y);
	}

	private void AddPlayerPosition(int x, int y){
		AddPlayerPosition (new Point (x, y));
	}

	private void AddGoalPosition(Point p){
		if(!GOAL_POS.Contains(p))
			GOAL_POS.Add(p);
	}
	
	private void AddGoalPosition(int x, int y){
		AddGoalPosition (new Point (x, y));
	}

	private bool HatchUntoPlayer(int x, int y){

		bool b = false;

		//If the left block is a collectible, make it a player
		if (x > 0 && IsBlock (x - 1, y, Block.BlockType.collectible)) {
			AddPlayerAtPosition(x-1,y);
			b=true;
		}

		//If the right block is a collectible, make it a player
		if (x < tilesX - 1 && IsBlock (x + 1, y, Block.BlockType.collectible)) {
			AddPlayerAtPosition(x+1,y);
			b=true;
		}
		
		//If the down block is a collectible, make it a player
		if (y > 0 && IsBlock (x, y - 1, Block.BlockType.collectible)) {
			AddPlayerAtPosition(x,y-1);
			b=true;
		}
		
		//If the up block is a collectible, make it a player
		if (y < tilesY - 1 && IsBlock (x, y + 1, Block.BlockType.collectible)) {
			AddPlayerAtPosition(x,y+1);
			b=true;
		}
		
		return b;
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
		return (IsBlock(x,y,Block.BlockType.background) || IsBlock(x,y,Block.BlockType.player) || IsBlock(x,y,Block.BlockType.goal));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
