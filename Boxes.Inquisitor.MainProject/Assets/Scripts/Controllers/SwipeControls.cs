using UnityEngine;
using System.Collections;

public class SwipeControls : MonoBehaviour {

	Vector2 startPositionOfTouch;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			if(Input.GetTouch(0).phase == TouchPhase.Began){
				startPositionOfTouch = Input.GetTouch(0).position;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended){
				Block testBlock = new Block(Block.BlockType.collectible);
				Vector2 endPosition = Input.GetTouch(0).position;
				float dx = endPosition.x - startPositionOfTouch.x;
				float dy = endPosition.y - startPositionOfTouch.y;
				if(Mathf.Abs(dx) > Mathf.Abs(dy)){
					if(dx > 0){
						//Go right
						testBlock.SetType(Block.BlockType.background);
					}
					else if(dx < 0){
						//Go left
						testBlock.SetType(Block.BlockType.player);
					}
				}
				else{
					if(dy > 0){
						//Go up
						testBlock.SetType(Block.BlockType.collectible);
					}
					else if(dx < 0){
						//Go down
					}
				}
				testBlock.DisplayObject(0,0);
			}
		}
	}
}
