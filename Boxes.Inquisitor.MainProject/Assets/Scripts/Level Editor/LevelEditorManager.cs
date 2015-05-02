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

	public static Block.BlockType currentType;

	void Start(){
		dInfo = new DirectoryInfo (Application.dataPath + "/Maps/");
		Block.BlockType[] allBlockTypes = Block.GetAllBlockTypes ();
		foreach (Block.BlockType blockType in allBlockTypes) {
			CreateNewBlockButton(blockType);
		}
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
		gc.SaveLevel (saveName.text);
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
