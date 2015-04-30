using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	private int currentMoves, bestScore, optimalScore;

	public void addMove(){
		currentMoves++;
	}

	private void OnGUI(){
		GUI.Label (new Rect (10,10,200,200), "<size=48>"+currentMoves+"</size>\n<size=16>Best score: "+bestScore+"\nOptimal score: "+optimalScore+"</size>");
	}

	// Use this for initialization
	void Start () {
		currentMoves=0;
		optimalScore=1;
		bestScore=1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
