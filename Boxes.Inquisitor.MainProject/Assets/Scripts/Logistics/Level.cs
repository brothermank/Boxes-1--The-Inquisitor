using UnityEngine;
using System.Collections;

public class Level {
	public Block[,] LevelData;
	public int creatorsBestScore = -1;
	public int playersBestScore = -1;

	public Level(Block[,] levelArray){
		LevelData = levelArray;
	}
	public Level(Block[,] levelArray, int creatorsBestScore, int playersBestScore){
		LevelData = levelArray;
		this.creatorsBestScore = creatorsBestScore;
		this.playersBestScore = playersBestScore;
	}

}
