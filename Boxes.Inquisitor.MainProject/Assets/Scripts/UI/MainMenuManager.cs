using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MainMenuManager : MonoBehaviour {

	public GameObject mainMenuPanel;
	public GameObject loadLevelPanel;
	public GameObject loadLevelContentPanel;
	public ButtonManager buttonPrefab;
	private static DirectoryInfo dInfo;
	public static int lastLevelIndex = -1;

	// Use this for initialization
	void Start () {
		dInfo = new DirectoryInfo (Application.dataPath + "/Maps/");
		loadLevelPanel.SetActive (false);
		mainMenuPanel.SetActive (true);
	}
	
	public void GoToLevelSelect(){
		mainMenuPanel.SetActive (false);
		loadLevelPanel.SetActive (true);
		FileInfo[] fInfo =  dInfo.GetFiles ();
		int currentIndex = 0;
		foreach (FileInfo file in fInfo) {
			if(GetNameExtension(file.Name) == ".map"){
				CreateLoadMapButton(file, currentIndex);
			}
			else if(GetNameExtension(file.Name) == ".txt"){
				CreateLoadMapButton(file, currentIndex);
			}
			currentIndex++;
		}
	}
	public void GoToMainMenu(){
		loadLevelPanel.SetActive (false);
		mainMenuPanel.SetActive (false);
	}

	private void CreateLoadMapButton(FileInfo file, int levelIndex){
		ButtonManager newButton = Instantiate (buttonPrefab) as ButtonManager;
		newButton.text.text = file.Name.Substring (0, file.Name.IndexOf( GetNameExtension (file.Name)));
		newButton.button.onClick.AddListener (() => StartLevel(file.Name, levelIndex));
		newButton.transform.SetParent (loadLevelContentPanel.transform);
	}

	private void StartLevel(string levelFileName, int levelIndex){
		lastLevelIndex = levelIndex;
		GameController.GloabalLevelString = levelFileName;
		Application.LoadLevel (1);
	}

	private string GetNameExtension(string name){
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

	public static void LoadNextLevel(){
		FileInfo[] fInfo =  dInfo.GetFiles ();
		lastLevelIndex = (lastLevelIndex + 1) % fInfo.Length;
		GameController.GloabalLevelString = fInfo[lastLevelIndex].Name;
		Application.LoadLevel (1);
	}
}
