using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplayer : MonoBehaviour {

	public GameObject mainPanel;
	public Text playerBest;
	public Text creatorsBest;
	public Text currentMoves;
	public GameController gc;


	/// <summary>
	/// Updates the displayed values so they corresponds with the values in gc.
	/// </summary>	
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
