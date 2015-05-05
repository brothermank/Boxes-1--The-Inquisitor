using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MainMenuManager : MonoBehaviour {

	public enum Panel{mainMenu, selectScene};

	public static Panel activeMenu = Panel.mainMenu;

	public GameObject mainMenuPanel;
	public GameObject loadLevelPanel;
	public GameObject loadLevelContentPanel;
	public ButtonManager buttonPrefab;
	private static DirectoryInfo dInfo;
	public static int lastLevelIndex = -1;
	public int desiredScene = 0;


	/// <summary>
	/// Gets the directory info where maps are loaded from
	/// </summary>	
	void Start () {
		dInfo = new DirectoryInfo (Application.dataPath + "/Maps/");
		SetPanelToActive (activeMenu);
	}

	/// <summary>
	/// Sets the active panel to given panel.
	/// </summary>	
	private void SetPanelToActive(Panel panel){
		if (panel == Panel.mainMenu) {
			mainMenuPanel.SetActive (true);
			loadLevelPanel.SetActive (false);
		} else if (panel == Panel.selectScene) {
			GoToLevelSelect();
		}
	}
	
	/// <summary>
	/// Goes to the level select screen and creates a button for each level in the directory. The button would load the level in the mainScene.
	/// </summary>	
	public void GoToLevelSelect(){
		mainMenuPanel.SetActive (false);
		loadLevelPanel.SetActive (true);
		FileInfo[] fInfo =  dInfo.GetFiles ();
		int currentIndex = 0;
		foreach (FileInfo file in ReturnValidFiles (fInfo)) {
			CreateLoadMapButton(file, currentIndex);
			currentIndex++;
		}
		desiredScene = 1;
	}
	/// <summary>
	/// Goes to the level select screen and creates a button for each level in the director. The button would load the level in the MapEditor.
	/// </summary>	
	public void GoToLevelSelectForEditor(){
		mainMenuPanel.SetActive (false);
		loadLevelPanel.SetActive (true);
		FileInfo[] fInfo =  dInfo.GetFiles ();
		int currentIndex = 0;
		foreach (FileInfo file in ReturnValidFiles (fInfo)) {
			CreateLoadMapButton(file, currentIndex);
			currentIndex++;
		}
		desiredScene = 2;
	}
	/// <summary>
	/// Goes to the mainMenu panel and sets other panels as inactive.
	/// </summary>	
	public void GoToMainMenu(){
		loadLevelPanel.SetActive (false);
		mainMenuPanel.SetActive (false);
	}

	/// <summary>
	/// Creates a button that sets starts the level at the given file.
	/// </summary>	
	private void CreateLoadMapButton(FileInfo file, int levelIndex){
		ButtonManager newButton = Instantiate (buttonPrefab) as ButtonManager;
		newButton.text.text = file.Name.Substring (0, file.Name.IndexOf( GetNameExtension (file.Name)));
		newButton.button.onClick.AddListener (() => StartLevel(file.Name, levelIndex));
		newButton.transform.SetParent (loadLevelContentPanel.transform);
	}

	/// <summary>
	/// Starts the level by setting the GlobalLevelString and going to the desired scene (desiredScene)
	/// </summary>	
	private void StartLevel(string levelFileName, int levelIndex){
		lastLevelIndex = levelIndex;
		GameController.GloabalLevelString = levelFileName;
		Application.LoadLevel (desiredScene);
	}

	/// <summary>
	/// Goes to the scene with the given index
	/// </summary>	
	public void GoToScene(int index){
		Application.LoadLevel (index);
	}

	/// <summary>
	/// Gets the extension of a string (what comes after '.')
	/// </summary>	
	private static string GetNameExtension(string name){
		string extension = "";
		char[] a = name.ToCharArray ();
		int i = 0;
		foreach (char c in a) {
			if(c == '.') break;
			i++;
		}
		for (int j = i; j < a.Length; j++) {
			extension += a[j];
		}
		return extension;
	}


	/// <summary>
	/// Returns array of files with accepted extensions
	/// </summary>	
	private static FileInfo[] ReturnValidFiles(FileInfo[] allFiles){
		List<FileInfo> validFiles = new List<FileInfo> ();
		foreach (FileInfo file in allFiles) {
			if(GetNameExtension(file.Name) == ".map"){
				validFiles.Add(file);
			}
			else if(GetNameExtension(file.Name) == ".txt"){
				validFiles.Add(file);
			}
		}
		return validFiles.ToArray();

	}

	/// <summary>
	/// Loads the level next in the level array
	/// </summary>	
	public static void LoadNextLevel(){
		FileInfo[] fInfo =  ReturnValidFiles( dInfo.GetFiles ());
		lastLevelIndex = (lastLevelIndex + 1) % fInfo.Length;
		GameController.GloabalLevelString = fInfo[lastLevelIndex].Name;
		Application.LoadLevel (1);
	}
}
