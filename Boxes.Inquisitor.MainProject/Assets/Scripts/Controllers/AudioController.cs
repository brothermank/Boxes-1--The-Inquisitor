using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class AudioController {

	private AudioSource src;
	private Dictionary<string, AudioClip> audio;

	private double nextRandom(){
		return UnityEngine.Random.Range (0f, 1f);
	}

	private float nextGaussian(double mean, double stdDev){
		double u1 = nextRandom ();//these are uniform(0,1) random doubles
		double u2 = nextRandom ();
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
			Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
		return (float)(mean + stdDev * randStdNormal); //random normal(mean,stdDev^2)
	}

	private void playSound(string snd, double d){
		playSound (snd, (float)d);
	}

	private void playSound(string snd, float pitch){
		Debug.Log ("1");
		AudioClip a = null;
		Debug.Log ("2");
		if (!audio.TryGetValue (snd, out a)) {
			Debug.LogError("No such sound clip '"+snd+"'.");
			return;
		}
		if (a == null)
			Debug.Log ("wtf");

		src.pitch = pitch;
		Debug.Log ("5");
		src.PlayOneShot (a, 1.0f);
		Debug.Log ("6");
	}

	public void playSoundNormal(string snd){
		playSound (snd, 1.0);
	}
	
	public void playSoundRandom(string snd){
		playSound (snd, nextGaussian(1.0,0.5));
	}

	public AudioController(AudioSource source){

		audio = new Dictionary<string, AudioClip> ();

		//Loading all resources, and adding them to the Dictionary
		DirectoryInfo info = new DirectoryInfo (Application.dataPath + "/Resources/Sounds/");
		FileInfo[] fileInfo = info.GetFiles ();
		foreach (FileInfo fi in fileInfo) {
			string str = getFileName(fi.ToString());
			if(str.EndsWith(".meta"))continue;

			string filename = removeExtension(str);

			AudioClip newAudio = (AudioClip)Resources.Load("Sounds/"+filename, typeof(AudioClip));

			audio.Add(filename, newAudio);
		}

		src = source;

	}

	private string removeExtension(string path){
		return path.Substring (0, path.IndexOf ("."));
	}

	private string getFileName(string fullPath){
		return fullPath.Substring(fullPath.LastIndexOf ("\\")+1).ToLower ();
	}
}
