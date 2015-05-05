using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AudioController {

	private Dictionary<string, AudioClip> audio;

	public AudioController(){

		audio = new Dictionary<string, AudioClip> ();

		//Loading all resources, and adding them to the Dictionary
		DirectoryInfo info = new DirectoryInfo (Application.dataPath + "/Resources/Sounds/");
		FileInfo[] fileInfo = info.GetFiles ();
		foreach (FileInfo fi in fileInfo) {
			string str = getFileName(fi.ToString());
			if(str.EndsWith(".meta"))continue;

			AudioClip newAudio = Resources.Load("Sounds/"+str) as AudioClip;

			audio.Add(removeExtension(str), newAudio);
		}
	}

	private string removeExtension(string path){
		return path.Substring (0, path.IndexOf ("."));
	}

	private string getFileName(string fullPath){
		return fullPath.Substring(fullPath.LastIndexOf ("\\")+1).ToLower ();
	}
}
