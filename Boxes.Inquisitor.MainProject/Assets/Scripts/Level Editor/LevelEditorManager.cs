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

	public static bool isTestingCreatorsAbilities = false;
	public static Block.BlockType currentType;

	void Start(){
		dInfo = new DirectoryInfo (Application.dataPath + "/Maps/");
		Block.BlockType[] allBlockTypes = Block.GetAllBlockTypes ();
		foreach (Block.BlockType blockType in allBlockTypes) {
			CreateNewBlockButton(blockType);
		}
		gc.handleWin = WinTestLevel;
	}

	public void TestLevel(){
		gc.StartTestMap ();
	}

	public void WinTestLevel(GameController gc){
		gc.paused = true;
		gc.level.playersBestScore = -1;
		gc.level.creatorsBestScore = -1;
		gc.LoadLevel(gc.level);
	}
	public void WinTestLevelAndSave(GameController gc){
		isTestingCreatorsAbilities = false;
		gc.paused = true;
		gc.level.creatorsBestScore = gc.movesThisAttempt;
		gc.SaveLevel (gc.level);
		gc.LoadLevel (gc.level);
		gc.handleWin = WinTestLevel;
		CancelSave ();
	}

	private void CreateNewBlockButton(Block.BlockType type){
		ButtonManager newButton = Instantiate(buttonPrefab) as ButtonManager;
		newButton.transform.SetParent (blockButtonPanel.transform);
		newButton.image.sprite = Block.GetSprite (type);
		newButton.text.text = "";
		newButton.button.onClick.AddListener (() => SetType(type));
	}

	public void SetType(Block.BlockType type){
		currentType = type;
	}

	public void StartSaveProcess(){
		savePanel.SetActive (true);
		createNewLevelPanel.SetActive (false);
		overwriteWarningPanel.SetActive (false);
	}

	public void StartCreateNewProcess(){
		CancelSave ();
		createNewLevelPanel.SetActive (true);
		overwriteWarningPanel.SetActive (false);
		return;
	}

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

	public void SaveLevel(){
		isTestingCreatorsAbilities = true;
		gc.handleWin = WinTestLevelAndSave;
		gc.StartSaveMap (saveName.text);
		CancelSave ();
	}

	public void CancelSave(){
		saveName.text = "";
		overwriteWarningPanel.SetActive (false);
		savePanel.SetActive (false);
		createNewLevelPanel.SetActive (false);
	}

	public void CreateNewLevel(){
		int width = SaveLoadManager.GetFirstIntInString (widthInput.text);
		int height = SaveLoadManager.GetFirstIntInString (heightInput.text);
		gc.CreateNewLevel (width, height);
		createNewLevelPanel.SetActive (false);
		CancelSave ();
		overwriteWarningPanel.SetActive (false);
	}



}
