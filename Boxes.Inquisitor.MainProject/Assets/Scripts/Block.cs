using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	SpriteRenderer visualiser;
	public enum BlockType{background, player, collectible};
	private BlockType type;

	// Use this for initialization
	void Start () {
		throw new System.NotImplementedException ("The block type is not finished yet");
	}
	
	void DisplayObject(){

	}

	Sprite GetSprite(BlockType type){
		string basePath = "";
		return null;
	}

	void setType(BlockType newType){
		type = newType;
		if(visualiser != null){
			visualiser.sprite = GetSprite(type);
		}
	}


}
