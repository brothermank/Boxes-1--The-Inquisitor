using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public enum Direction{left, right, up, down};

	private LevelController lc;

	private int tilesX, tilesY;
	public string LevelString = "";
	public static string GloabalLevelString = "";
	public Level level;
	public Block[,] LevelData; 
	public Block[,] OriginalLevelData{
		get {
			return level.LevelData;
		}
	}

	public GameObject winPanel;
	public bool paused = false;
	private bool testingMap = false;

	public bool CheckIfWinning(){
		bool hasWon = lc.HasWon ();
		if (hasWon)
			Win ();
		return hasWon;
	}
	public void Win(){
		if (!testingMap) {
			paused = true;
			winPanel.SetActive (true);
		} else {
			paused = true;
			LoadLevel(level);
			testingMap = false;
		}
	}

	public void restartMap(){
		LoadLevel (level);
	}
	public void StartTestMap(){
		level = new Level (LevelData);
		LoadLevel (level);
		paused = false;
		testingMap = false;
	}

	// Use this for initialization
	void Start () {
		LoadLevel ();
		winPanel.SetActive (false);
	}

	private void LoadLevel(){
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
	private void LoadLevel(Level newLevel){
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
		level = new Level (newLevel);
		LoadLevel (level);
	}

	public bool MovePlayer(Direction d){
		if (d == Direction.left)
			return lc.MoveRelatively (-1, 0);
		if (d == Direction.right)
			return lc.MoveRelatively (1, 0);
		if (d == Direction.down)
			return lc.MoveRelatively (0, -1);
		if (d == Direction.up)
			return lc.MoveRelatively (0, 1);
		return false;
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

	public void SaveLevel(string saveName){
		SaveLoadManager.SaveLevel (LevelData ,saveName);
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
		CheckIfWinning ();
	}
}
