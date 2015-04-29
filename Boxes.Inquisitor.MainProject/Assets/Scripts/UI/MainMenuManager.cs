using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MainMenuManager : MonoBehaviour {

	public GameObject mainMenuPanel;
	public GameObject loadLevelPanel;
	public ButtonManager buttonPrefab;
	private DirectoryInfo dInfo;

	// Use this for initialization
	void Start () {
		dInfo = new DirectoryInfo (Application.dataPath + "/Maps/");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GoToLevelSelect(){
		mainMenuPanel.SetActive (false);
		FileInfo[] fInfo =  dInfo.GetFiles ();
		foreach (FileInfo file in fInfo) {
			if(GetNameExtension(file.Name) == ".map"){
				CreateLoadMapButton(file);
			}
			else if(GetNameExtension(file.Name) == ".txt"){
				CreateLoadMapButton(file);
			} 
		}
	}

	private void CreateLoadMapButton(FileInfo file){
		ButtonManager newButton = Instantiate (buttonPrefab) as ButtonManager;
		newButton.text.text = file.Name.Substring (0, file.Name.IndexOf( GetNameExtension (file.Name)));
		newButton.button .onClick.AddListener (() => SaveLoadManager.LoadMap (file.Name));
		newButton.transform.SetParent (loadLevelPanel.transform);
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
}
