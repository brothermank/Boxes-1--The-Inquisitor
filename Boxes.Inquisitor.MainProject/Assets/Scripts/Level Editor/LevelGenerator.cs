using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator {

	private LevelController lc;
	private Block[,] start;

	public LevelGenerator(Block[,] initial){
		start = (Block[,])initial.Clone ();
		lc = new LevelController (initial);

	}

	public Block[,] getContents(){
		return start;
	}

	public void process(){
		//Check if there already is a goal in the level; if so, then error.
		if (lc.hasGoal ()) {
			Debug.LogError("Error: Can't process a map that already has a goal.");
			return;
		}

		//Move randomly around
		for (int i=0; i<1000; i++) {
			lc.MovePlayer(GameController.Direction.random);
			if(!lc.hasPlayer())
				lc.UndoLastMove();
		}

		//Set goals in the original level where the player is now
		List<Point> player = lc.getPlayerPositions ();
		foreach(Point p in player){
			start[p.x,p.y].isAlsoGoal = true;
		}
	}
}
