using UnityEngine;
using System.Collections;

public class Level {
	public Block[,] LevelData;
	public int creatorsBestScore = -1;
	public int playersBestScore = -1;
	public string levelName;

	public Level(Block[,] levelArray, string levelName){
		LevelData = levelArray;
		this.levelName = levelName;
	}
	public Level(Block[,] levelArray, string levelName, int creatorsBestScore, int playersBestScore){
		LevelData = levelArray;
		this.levelName = levelName;
		this.creatorsBestScore = creatorsBestScore;
		this.playersBestScore = playersBestScore;
	}

}
