using UnityEngine;
using System.Collections;

public class Block {

	private SpriteRenderer visualiser;
	private static SpriteRenderer visualiserPrefab = (Resources.Load ("Prefabs/Block Visual") as GameObject).GetComponent<SpriteRenderer> ();
	public enum BlockType{background, player, collectible};
	private BlockType type;
	private Transform parent;

	public Block(BlockType type){
		this.type = type;
	}

	/// <summary>
	/// Displays a block with the blocks type at position x,y. If the block is already displayed, the previous displayer will be destroyed  	
	/// </summary>
	public void DisplayObject(int x, int y){
		MonoBehaviour. Destroy (visualiser);
		SpriteRenderer renderer = MonoBehaviour.Instantiate(visualiserPrefab) as SpriteRenderer;
		renderer.transform.position = new Vector3 ((float)x, (float)y, 0);
		Sprite sprite = GetSprite (type);
		renderer.sprite = sprite;
		if (parent == null) {
			parent = new GameObject().transform;
			parent.gameObject.name = "Blocks";
		}
		renderer.transform.SetParent (parent);
		visualiser = renderer;
	}

	private Sprite GetSprite(BlockType type){
		string basePath = "Art/Blocks/";
		string objectName = "";
		switch (type) {
		case BlockType.background:
			objectName = "Background";
			break;
		case BlockType.player:
			objectName = "Player";
			break;
		case BlockType.collectible:
			objectName = "Collectible";
			break;
		default:
			return null;
		}
		Texture2D texture =  Resources.Load (basePath + objectName) as Texture2D;
		return Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f), texture.height);
	}
	
	
	/// <summary>
	/// Sets the type of the block, and updates it's display (if any) to reflect the new type
	/// </summary>
	public void SetType(BlockType newType){
		type = newType;
		if(visualiser != null){
			visualiser.sprite = GetSprite(type);
		}
	}
	public BlockType getType(){
		return type;
	}

	/// <summary>
	/// Removes the display and sets the private visualiser variable to null
	/// </summary>
	public void RemoveObjectDisplay(){
		if (visualiser != null) {
			MonoBehaviour.Destroy(visualiser);
			visualiser = null;
		}
	}
}
