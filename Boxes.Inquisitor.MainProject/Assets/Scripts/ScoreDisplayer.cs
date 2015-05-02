using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplayer : MonoBehaviour {

	public GameObject mainPanel;
	public Text playerBest;
	public Text creatorsBest;
	public Text currentMoves;
	public GameController gc;


	// Use this for initialization
	public void UpdateScore(){
		if (gc.level == null) {
			if(mainPanel.activeSelf){
				mainPanel.SetActive (false);
			}
		} else {
			playerBest.text = "" + gc.level.playersBestScore;
			creatorsBest.text = "" + gc.level.creatorsBestScore;
			currentMoves.text = "" + gc.movesThisAttempt;
		}

	}

	void Update(){

	}
}
