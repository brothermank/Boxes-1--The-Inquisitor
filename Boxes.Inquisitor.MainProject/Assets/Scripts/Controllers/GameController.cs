using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController MainGC;

	public enum Direction{left, right, up, down, random};

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

	// Use this for initialization
	void Start () {
		MainGC = this;
		score.UpdateScore ();
		handleWin = Win;
		MainGC = this;
		try{
			LoadLevel ();
		}catch(System.UnauthorizedAccessException){}
		try{
			winPanel.SetActive (false);
		}catch(UnassignedReferenceException){}
	}

	/// <summary>
	/// Returns whether it is tru that the player has won, and if it is true, also handles the win. (handleWin())
	/// </summary>
	public bool CheckIfWinning(){
		bool hasWon = lc.HasWon ();
		if (hasWon)
			handleWin (this);
		return hasWon;
	}
	/// <summary>
	/// Default assigned handleWin() action. Pauses the game and activates the Win Panel
	/// </summary>
	public void Win(GameController gc){
		/*gc.level.playersBestScore = movesThisAttempt;
		gc.SaveLevel ();*/
		gc.paused = true;
		gc.winPanel.SetActive (true);
		playSoundNormal ("fanfare");
	}
	/// <summary>
	/// Loads the level again to achieve a restart of the level.
	/// </summary>
	public void restartMap(){
		LoadLevel (level);
	}
	/// <summary>
	/// Starts the currently assigned level as a new level, so records are not saved.
	/// </summary>
	public void StartTestMap(){
		score.gameObject.SetActive (true);
		level = new Level (LevelData, "test map");
		LoadLevel (level);
		paused = false;
	}
	/// <summary>
	/// Creates a new level (no records) with the designated map name, and current LevelData
	/// </summary>
	public void StartSaveMap(string mapName){
		score.gameObject.SetActive (true);
		level = new Level (LevelData, mapName);
		LoadLevel (level);
		paused = false;
	}



	
	/// <summary>
	/// Loads and instantiates the level stored at the GlobalLevelString
	/// </summary>	
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
				LevelData[x,y] = new Block(OriginalLevelData[x,y]);
				LevelData[x,y].SetType(OriginalLevelData[x,y].getType ());
			}
		}
		lc = new LevelController (LevelData);
		DrawContents ();
		
		Camera.main.transform.position = new Vector3 ((float)(tilesX)/2f - 0.5f, (float)(tilesY)/2f - 0.5f, -1.5f);
		CameraController.ResizeMainCamTo (tilesX, tilesY);
	}
	/// <summary>
	/// Loads and instantiates given level
	/// </summary>	
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
				LevelData[x,y] = new Block(OriginalLevelData[x,y]);
				LevelData[x,y].SetType(OriginalLevelData[x,y].getType ());
			}
		}
		lc = new LevelController (LevelData);

		DrawContents ();
		
		Camera.main.transform.position = new Vector3 ((float)(tilesX)/2f - 0.5f, (float)(tilesY)/2f - 0.5f, -1.5f);
		CameraController.ResizeMainCamTo (tilesX, tilesY);
	}
	/// <summary>
	/// Creates a level with only sand blocks (type 0) with the given width and height
	/// </summary>	
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

	public static void playSound(string snd){
		AudioController ac = Camera.main.GetComponent<AudioController> ();
		ac.playSoundRandom (snd);
	}
	
	public static void playSoundNormal(string snd){
		AudioController ac = Camera.main.GetComponent<AudioController> ();
		ac.playSoundNormal (snd);
	}

	/// <summary>
	/// Undoes the last move.
	/// </summary>
	public void UndoLastMove(){
		lc.UndoLastMove ();
		movesThisAttempt--;
		score.UpdateScore ();
	}

	/// <summary>
	/// Moves the player in the direction, using the LevelController.MoveRelatively function.
	/// </summary>	
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
			playSound("click");
			score.UpdateScore();
		}
		return hasMoved;
	}

	/// <summary>
	/// Deletes all visualisers
	/// </summary>	
	public void DeleteContents(){
		Block[,] LEVEL = lc.GetLevel();
		for(int x=0; x<tilesX; x++){
			for(int y=0; y<tilesY; y++){
				LEVEL[x,y].RemoveObjectDisplay();
			}
		}
	}
	/// <summary>
	/// Creates visualers for all blocks in the instantiated level
	/// </summary>	
	public void DrawContents(){
		Block[,] LEVEL = lc.GetLevel();
		for(int x=0; x<tilesX; x++){
			for(int y=0; y<tilesY; y++){
				LEVEL[x,y].DisplayObject(x,y);
			}
		}
	}

	/// <summary>
	/// Saves the given level, using the levels name as filename
	/// </summary>	
	public void SaveLevel(Level level){
		SaveLoadManager.SaveLevel (level);
	}
	/// <summary>
	/// Saves the loaded level, using the levels name as filename
	/// </summary>	
	public void SaveLevel(){
		SaveLoadManager.SaveLevel (level);
	}

	/// <summary>
	/// Goes to the Start Menu scene (Main Menu)
	/// </summary>	
	public void GoToMainMenu(int menuIndex){
		MainMenuManager.activeMenu = (MainMenuManager.Panel)menuIndex;
		Application.LoadLevel (0);
	}

	/// <summary>
	/// Goes to the next level (ordered by filename)
	/// </summary>	
	public void NextLevel(){
		MainMenuManager.LoadNextLevel ();
	}
	/// <summary>
	/// Loads the level again, resulting in a restart
	/// </summary>	
	public void RestartLevel(){
		LoadLevel (level);
		winPanel.SetActive (false);
	}

	/// <summary>
	/// If the game is not paused, checks if the character is in a winning position, and handles win.
	/// </summary>	
	void Update () {
		if (!paused) {
			CheckIfWinning ();
		}
	}
}
