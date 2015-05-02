using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public enum Direction{left, right, up, down};

	public delegate void ActionOnWin(GameController gc);
	public ActionOnWin handleWin;

	private LevelController lc;

	private int tilesX, tilesY;
	public int movesThisAttempt = 0;
	public string LevelString = "";
	public static string GloabalLevelString = "";
	public Level level;
	public Block[,] LevelData; 
	public Block[,] OriginalLevelData{
		get {
			return level.LevelData;
		}
	}
	
	public ScoreDisplayer score;
	public GameObject winPanel;
	public bool paused = false;
	
	public bool CheckIfWinning(){
		bool hasWon = lc.HasWon ();
		if (hasWon)
			handleWin (this);
		return hasWon;
	}
	public void Win(GameController gc){
		gc.paused = true;
		gc.winPanel.SetActive (true);
		
	}

	public void restartMap(){
		LoadLevel (level);
	}
	public void StartTestMap(){
		score.gameObject.SetActive (true);
		level = new Level (LevelData, "test map");
		LoadLevel (level);
		paused = false;
	}
	public void StartSaveMap(string mapName){
		score.gameObject.SetActive (true);
		level = new Level (LevelData, mapName);
		LoadLevel (level);
		paused = false;
	}

	// Use this for initialization
	void Start () {
		LoadLevel ();
		score.UpdateScore ();
		winPanel.SetActive (false);
		handleWin = Win;
	}

	public void LoadLevel(){
		movesThisAttempt = 0;
		if (LevelData != null) {
			foreach (Block block in LevelData) {
				block.RemoveObjectDisplay ();
			}
		}
		if (LevelString == "") {
			level = SaveLoadManager.LoadLevel (GloabalLevelString);
		} else {
			level = SaveLoadManager.LoadLevel (LevelString);
		}

		
		tilesX = OriginalLevelData.GetLength (0);
		tilesY = OriginalLevelData.GetLength (1);
		LevelData = new Block[tilesX, tilesY];
		for (int x = 0; x < tilesX; x++) {
			for(int y = 0; y < tilesY; y++){
				LevelData[x,y] = new Block(OriginalLevelData[x,y].getType());
			}
		}
		lc = new LevelController (LevelData);
		DrawContents ();
		
		Camera.main.transform.position = new Vector3 ((float)(tilesX)/2f - 0.5f, (float)(tilesY)/2f - 0.5f, -1.5f);
		CameraController.ResizeMainCamTo (tilesX, tilesY);
	}
	public void LoadLevel(Level newLevel){
		level = newLevel;
		movesThisAttempt = 0;
		if (LevelData != null) {
			foreach (Block block in LevelData) {
				block.RemoveObjectDisplay ();
			}
		}

		tilesX = OriginalLevelData.GetLength (0);
		tilesY = OriginalLevelData.GetLength (1);
		LevelData = new Block[tilesX, tilesY];
		for (int x = 0; x < tilesX; x++) {
			for(int y = 0; y < tilesY; y++){
				LevelData[x,y] = new Block(OriginalLevelData[x,y].getType());
			}
		}
		lc = new LevelController (LevelData);

		DrawContents ();
		
		Camera.main.transform.position = new Vector3 ((float)(tilesX)/2f - 0.5f, (float)(tilesY)/2f - 0.5f, -1.5f);
		CameraController.ResizeMainCamTo (tilesX, tilesY);
	}
	public void CreateNewLevel (int width, int height){
		Block[,] newLevel = new Block[width, height];
		for (int x = 0; x < width; x++) {
			for(int y = 0; y < height; y++){
				newLevel[x,y] = new Block(Block.BlockType.background);
			}
		}
		level = new Level (newLevel, "test level");
		LoadLevel (level);
	}

	public bool MovePlayer(Direction d){
		bool hasMoved = false;
		if (d == Direction.left)
			hasMoved = lc.MoveRelatively (-1, 0);
		else if (d == Direction.right)
			hasMoved = lc.MoveRelatively (1, 0);
		else if (d == Direction.down)
			hasMoved = lc.MoveRelatively (0, -1);
		else if (d == Direction.up)
			hasMoved = lc.MoveRelatively (0, 1);
		if (hasMoved) {
			movesThisAttempt++;
			score.UpdateScore();
		}
		return hasMoved;
	}

	public void DeleteContents(){
		Block[,] LEVEL = lc.GetLevel();
		for(int x=0; x<tilesX; x++){
			for(int y=0; y<tilesY; y++){
				LEVEL[x,y].RemoveObjectDisplay();
			}
		}
	}
	public void DrawContents(){
		Block[,] LEVEL = lc.GetLevel();
		for(int x=0; x<tilesX; x++){
			for(int y=0; y<tilesY; y++){
				LEVEL[x,y].DisplayObject(x,y);
			}
		}
	}

	public void SaveLevel(Level level){
		SaveLoadManager.SaveLevel (level);
	}
	public void SaveLevel(){
		SaveLoadManager.SaveLevel (level);
	}

	public void GoToMainMenu(int menuIndex){
		MainMenuManager.activeMenu = (MainMenuManager.Panel)menuIndex;
		Application.LoadLevel (0);
	}

	public void NextLevel(){
		MainMenuManager.LoadNextLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!paused) {
			CheckIfWinning ();
		}
	}
}
