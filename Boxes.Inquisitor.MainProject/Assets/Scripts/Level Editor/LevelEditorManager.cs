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
	public InputField name;
	public DirectoryInfo dInfo;

	public static Block.BlockType currentType;

	void Start(){

	}

	public void SetType(Block.BlockType type){
		currentType = type;
	}

	public void StartSaveProcess(){
		return;
	}

	public void StartCreateNewProcess(){
		return;
	}

	public void CheckForOverwriteNameAndHandle(){
		return;
	}

	public void SaveLevel(){
		return;
	}

	public void CancelSave(){
		return;
	}

	public void CreateNewLevel(){
		return;
	}



}
