using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class LevelEditorManager : MonoBehaviour {

	public GameObject blockButtonPanel;
	public GameObject savePanel;
	public GameObject overwriteWarningPanel;
	public GameObject createNewLevelPanel;
	public InputField widthInput;
	public InputField heightInput;
	public InputField saveName;
	public DirectoryInfo dInfo;
	public string usedExtensionForSaves = ".txt";
	public GameController gc;
	public ButtonManager buttonPrefab;
	public Button redoButton;

	public static bool isTestingCreatorsAbilities = false;
	public static Block.BlockType currentType;
	public static Stack moves = new Stack();
	public static Stack undoneMoves = new Stack();

	public struct Move{
		public int x;
		public int y;
		public Block.BlockType changedFrom;

		public Move(Block.BlockType changedFrom, int posx, int posy){
			this.x = posx;
			this.y = posy;
			this.changedFrom = changedFrom;
		}
	}
	

	void Start(){
		dInfo = new DirectoryInfo (Application.dataPath + "/Maps/");
		Block.BlockType[] allBlockTypes = Block.GetAllBlockTypes ();
		foreach (Block.BlockType blockType in allBlockTypes) {
			CreateNewBlockButton(blockType);
		}
		gc.handleWin = WinTestLevel;
	}
	void Update(){
		//if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(Input.GetKeyDown(KeyCode.Z)){
				UndoLastMove();
			}
			if(Input.GetKeyDown(KeyCode.Y)){
				Redo();
			}
		//}
		if (undoneMoves.Count <= 0) {
			redoButton.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Undes the last move.
	/// </summary>
	public void UndoLastMove(){
		if (moves.Count > 0) {
			Move lastMove = (Move)moves.Pop ();
			int x = lastMove.x;
			int y = lastMove.y;
			undoneMoves.Push (new Move (gc.LevelData [x, y].getType (), x, y));
			gc.LevelData [lastMove.x, lastMove.y].SetType (lastMove.changedFrom);
			redoButton.gameObject.SetActive(true);
		}
	}
	/// <summary>
	/// Redoes the last move.
	/// </summary>
	public void Redo(){
		if (undoneMoves.Count > 0) {
			Move undoneMove = (Move)undoneMoves.Pop ();
			int x = undoneMove.x;
			int y = undoneMove.y;
			moves.Push (new Move (gc.LevelData [x, y].getType (), x, y));
			gc.LevelData [undoneMove.x, undoneMove.y].SetType (undoneMove.changedFrom);
		}
	}
	/// <summary>
	/// Starts the level in the GameController as test map and sets the handleWin method to WinTestLevel
	/// </summary>	
	public void TestLevel(){
		gc.handleWin = WinTestLevel;
		gc.StartTestMap ();
	}

	/// <summary>
	/// Handles the win without saving score, and loads the tested level (reverting moves made during the test)
	/// </summary>	
	public void WinTestLevel(GameController gc){
		gc.paused = true;
		gc.level.playersBestScore = -1;
		gc.level.creatorsBestScore = -1;
		gc.LoadLevel(gc.level);
	}
	/// <summary>
	/// Handles the win, saving score as Creators Best. Loaded again to revert moves, and then saved.
	/// </summary>	
	public void WinTestLevelAndSave(GameController gc){
		isTestingCreatorsAbilities = false;
		gc.paused = true;
		gc.level.creatorsBestScore = gc.movesThisAttempt;
		gc.LoadLevel (gc.level);
		gc.SaveLevel (gc.level);
		gc.handleWin = WinTestLevel;
		CancelSave ();
	}

	/// <summary>
	/// Creates a button for selecting a tile of the given type, in the tile selection menu (blockButtonPanel)
	/// </summary>	
	private void CreateNewBlockButton(Block.BlockType type){
		ButtonManager newButton = Instantiate(buttonPrefab) as ButtonManager;
		newButton.transform.SetParent (blockButtonPanel.transform);
		newButton.image.sprite = Block.GetSprite (type);
		newButton.text.text = "";
		newButton.button.onClick.AddListener (() => SetType(type));
	}

	/// <summary>
	/// Sets the type to be painted on click.
	/// </summary>	
	public void SetType(Block.BlockType type){
		currentType = type;
	}

	/// <summary>
	/// Opens the save panel
	/// </summary>	
	public void StartSaveProcess(){
		savePanel.SetActive (true);
		createNewLevelPanel.SetActive (false);
		overwriteWarningPanel.SetActive (false);
	}

	/// <summary>
	/// Opens the create new level window and cancel an ongoing save
	/// </summary>	
	public void StartCreateNewProcess(){
		CancelSave ();
		createNewLevelPanel.SetActive (true);
		overwriteWarningPanel.SetActive (false);
		return;
	}

	/// <summary>
	/// Checks whether there is already a file with the diesired save name at the file location. Does SaveLevel() if not, 
	/// and opens overwriteWarningPanel if true.
	/// </summary>	
	public void CheckForOverwriteNameAndHandle(){
		FileInfo[] fInfo = dInfo.GetFiles ();
		foreach (FileInfo file in fInfo) {
			if(file.Name == saveName.text + usedExtensionForSaves){
				overwriteWarningPanel.SetActive(true);
				savePanel.SetActive(false);
				createNewLevelPanel.SetActive(false);
				return;
			}
		}
		SaveLevel ();
	}

	/// <summary>
	/// Starts the level as a map going to be saved (gc.StartSaveMap(saveName.text) and sets gc.handleWin as WinTestLevelAndSave).
	/// </summary>	
	public void SaveLevel(){
		isTestingCreatorsAbilities = true;
		gc.handleWin = WinTestLevelAndSave;
		gc.StartSaveMap (saveName.text);
		CancelSave ();
	}


	/// <summary>
	/// closes panels relevant to save and createNewLevelPanel
	/// </summary>	
	public void CancelSave(){
		saveName.text = "";
		overwriteWarningPanel.SetActive (false);
		savePanel.SetActive (false);
		createNewLevelPanel.SetActive (false);
	}

	/// <summary>
	/// Creates a level with block array of type 0 blocks (background), with given width and height and loads the level in gc.
	/// </summary>	
	public void CreateNewLevel(){
		int width = SaveLoadManager.GetFirstIntInString (widthInput.text);
		int height = SaveLoadManager.GetFirstIntInString (heightInput.text);
		gc.CreateNewLevel (width, height);
		createNewLevelPanel.SetActive (false);
		CancelSave ();
		overwriteWarningPanel.SetActive (false);
	}



}
