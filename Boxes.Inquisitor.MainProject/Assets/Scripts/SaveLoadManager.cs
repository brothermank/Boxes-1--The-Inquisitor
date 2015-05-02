using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveLoadManager {

	
	/// <summary>
	/// Loads a two dimensional block array from a file
	/// </summary>

	private static string sanitizeString(string s){
		if (s == null)
			return null;
		return s.Replace(".","0").Replace("X","1").Replace("O","2").Replace("M","3").Replace("S","3").Replace("G","4").Replace("H","6");
	}

	public static Level LoadLevel(string fileName){
		Debug.Log ("loading level");
		List<List<Block>> blocks = new List<List<Block>> ();
		System.IO.StreamReader file;
		file = new System.IO.StreamReader (Application.dataPath + "/Maps/" + fileName);
		string line;
		line = sanitizeString(file.ReadLine ());
		char[] chars = line.ToCharArray ();
		int y = 0;
		bool fileCorrupted = false;
		bool highscoresMissing = false;
		
		//Analyse first line...  Get upper left corner coordinates from first two numbers in file
		List<int> intsInFirstLine = getIntsInString (line);
		List<Block> blocksInLine;
		if (intsInFirstLine.Count < 3) {
			Debug.Log("Missing highscore information. Highscores set to -1. When trying too load: " + fileName);
			highscoresMissing = true;
		}
		int expectedWidth = intsInFirstLine [0];
		int expectedHeight = intsInFirstLine [1];
		
		while ((line = sanitizeString(file.ReadLine ())) != null) {
			blocks.Add(new List<Block>());
			blocksInLine = getBlocksInString(sanitizeString(line));
			if(blocksInLine.Count != expectedWidth){
				Debug.Log("Amount of blocks in line: " + y + " when trying to load: " + fileName);	
				fileCorrupted = true;
			}
			for(int x = 0; x < blocksInLine.Count; x++){
				blocks[y].Add(blocksInLine[x]);
			}
			y++;
		}
		if(blocks.Count != expectedHeight){
			Debug.Log("File does not meet the expected amount of lines, when trying to load: " + fileName);
			fileCorrupted = true;
		}
		if (fileCorrupted) {
			Debug.Log("File was corrupted, and null returned, when trying to load from: " + fileName);
			return null;
		}
		Block[,] blockA = new Block[expectedWidth, expectedHeight];
		int xa = 0;
		int ya = 0;
		foreach (List<Block> list in blocks) {
			foreach(Block block in list){
				blockA[xa,ya] = blocks[expectedHeight - ya - 1][xa];
				xa++;
			}
			xa = 0;
			ya++;
		}
		Level level;
		if (!highscoresMissing) {
			 level = new Level (blockA, fileName, intsInFirstLine [2], intsInFirstLine [3]);
		} else {
			 level = new Level (blockA, fileName);
		}
		return level;
	}

	public static int GetFirstIntInString(string line){
		List<int> ints = getIntsInString (line);
		if(ints.Count> 0) return ints[0];
		return -1;
	}
	
	/// <summary>
	/// Returns a list of all ints in the line seperated by all other characters. If a '-' is infront of a row of numbers, the number is considered negative
	/// </summary>
	private static List<int> getIntsInString(string line){
		List<int> allIntsInString = new List<int>();
		char[] characters = line.ToCharArray();
		char nextChar = ' ';
		List<int> currentNumber = new List<int>();
		bool isNegative = false;
		char previousChar = ' ';
		for(int i = 0; i < characters.Length; i++){
			nextChar = characters[i];
			if (nextChar >= '0' && nextChar <= '9') {
				currentNumber.Add((int)nextChar - 48);
				if(previousChar == '-') isNegative = true;
			}
			else if(currentNumber.Count > 0){
				int nextInt = 0;
				for(int j = 0; j < currentNumber.Count; j++){
					nextInt += currentNumber[currentNumber.Count - j - 1] * (int)Mathf.Pow(10,j);
				}
				if(isNegative) nextInt *= -1;
				allIntsInString.Add(nextInt);
				currentNumber = new List<int>();
				isNegative = false;
			}
			else{
				isNegative = false;
			}
			previousChar = nextChar;
		}
		if(currentNumber.Count > 0){
			int nextInt = 0;
			for(int j = 0; j < currentNumber.Count; j++){
				nextInt += currentNumber[currentNumber.Count - j - 1] * (int)Mathf.Pow(10,j);
			}
			if(isNegative) nextInt *= -1;
			allIntsInString.Add(nextInt);
		}
		return allIntsInString;
	}

	
	/// <summary>
	/// Returns a list of all the blocks interpretted from the line
	/// </summary>
	private static List<Block> getBlocksInString(string line){
		List<Block> allBlocksInString = new List<Block>();
		char[] characters = line.ToCharArray();
		char nextChar = ' ';
		List<int> currentNumber = new List<int>();
		bool isNegative = false;
		char previousChar = ' ';
		bool addingObjects = false;
		for(int i = 0; i < characters.Length; i++){
			nextChar = characters[i];
			if(nextChar == '/' && previousChar == '/')
				break;
			//if(!addingObjects){
				if (nextChar >= '0' && nextChar <= '9') {
					currentNumber.Add((int)nextChar - 48);
					if(previousChar == '-') isNegative = true;
				}
				else if(currentNumber.Count > 0){
					int nextInt = 0;
					for(int j = 0; j < currentNumber.Count; j++){
						nextInt += currentNumber[currentNumber.Count - j - 1] * (int)Mathf.Pow(10,j);
					}
					if(isNegative) nextInt *= -1;
						allBlocksInString.Add(new Block((Block.BlockType)nextInt));
					currentNumber = new List<int>();
					/*if(nextChar == ':'){
						addingObjects = true;
					}*/
					isNegative = false;
				}
				else{
					isNegative = false;
				}
			//}
			/*else{ //If we are adding an object - aka, if the character after a number is :
				if (nextChar >= '0' && nextChar <= '9') { //If we have to add a cifer to the next number
					currentNumber.Add((int)nextChar - 48);
					if(previousChar == '-') isNegative = true;
				}
				else if(currentNumber.Count > 0){ //If we dont have to add a cifer. Stop looking for objects if the next character is not ','
					int nextInt = 0;
					for(int j = 0; j < currentNumber.Count; j++){
						nextInt += currentNumber[currentNumber.Count - j - 1] * (int)Mathf.Pow(10,j);
					}
					if(isNegative) nextInt *= -1;
					allTilesInString[allTilesInString.Count - 1].AddObject((WorldObjectStats.ObjectType)nextInt);
					currentNumber = new List<int>();
					isNegative = false;
					if(nextChar != ',')
						addingObjects = false;
				}
				else addingObjects = false;
			}*/
			previousChar = nextChar;
		}
		if(currentNumber.Count > 0){
			if(!addingObjects){
				int nextInt = 0;
				for(int j = 0; j < currentNumber.Count; j++){
					nextInt += currentNumber[currentNumber.Count - j - 1] * (int)Mathf.Pow(10,j);
				}
				if(isNegative) nextInt *= -1;
				allBlocksInString.Add(new Block((Block.BlockType)nextInt));
			}
			/*else{
				int nextInt = 0;
				for(int j = 0; j < currentNumber.Count; j++){
					nextInt += currentNumber[currentNumber.Count - j - 1] * (int)Mathf.Pow(10,j);
				}
				if(isNegative) nextInt *= -1;
				allTilesInString[allTilesInString.Count - 1].AddObject((WorldObjectStats.ObjectType)nextInt);
				Debug.Log(nextInt);
			}*/
		}
		return allBlocksInString;
	}

	
	/// <summary>
	/// Saves a two dimensional block array
	/// </summary>
	public static void SaveLevel(Level level){
		Block[,] blocks = level.LevelData;
		List<string> lines = new List<string> ();
		int width = blocks.GetLength(0);
		int height = blocks.GetLength(1);
		string nextLine = "" + width + " " + height + " " + level.creatorsBestScore + " " + level.playersBestScore;
		lines.Add (nextLine);
		for (int y = 0; y < height; y++) {
			nextLine = "";
			for(int x = 0; x < width; x++){
				Block nextBlock = blocks[x,y];
				nextLine += (int)nextBlock.getType();
				/*foreach(WorldObjectStats o in nextTile.objects){
					nextLine += ":" + (int)o.type;
				}*/
				nextLine += " ";
			}
			lines.Add(nextLine);
		}
		System.IO.File.WriteAllLines (Application.dataPath + "/Maps/" + level.levelName + ".txt", lines.ToArray ());
	}
}
