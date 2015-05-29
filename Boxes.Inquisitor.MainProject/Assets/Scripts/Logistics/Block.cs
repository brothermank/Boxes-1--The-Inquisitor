using UnityEngine;
using System;
using System.Collections;

public class Block {

	private SpriteRenderer visualiser;
	private static SpriteRenderer visualiserPrefab = (Resources.Load ("Prefabs/Block Visual") as GameObject).GetComponent<SpriteRenderer> ();
	public enum BlockType{background, player, collectible, solid, goal, appendage, hazard};
	public enum SpecialCondition{isAlsoGoal};
	private BlockType type;
	private static Transform parent;
	public bool isAlsoGoal = false;

	public Block(BlockType type){
		SetType (type);
	}
	public Block(Block source){
		visualiser = source.visualiser;
		isAlsoGoal = source.isAlsoGoal;
		SetType (source.type);
	}

	/// <summary>
	/// Sets a special condition.
	/// </summary>
	/// <param name="condition">Condition.</param>
	public void SetSpecialCondition(SpecialCondition condition){
		switch (condition) {
		case SpecialCondition.isAlsoGoal:
			isAlsoGoal = true;
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// Gets an array containing an instance of each block type
	/// </summary>	
	public static BlockType[] GetAllBlockTypes(){
		System.Array a = System.Enum.GetValues (typeof(BlockType));
		BlockType[] blocks = new BlockType[a.Length];
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i] = (BlockType)a.GetValue(i);
		}
		return blocks;
	}

	/// <summary>
	/// Displays a block with the blocks type at position x,y. If the block is already displayed, the previous displayer will be destroyed  	
	/// </summary>
	public void DisplayObject(int x, int y){
		if(visualiser == null){
			SpriteRenderer renderer = MonoBehaviour.Instantiate(visualiserPrefab) as SpriteRenderer;
			renderer.transform.position = new Vector3 ((float)x, (float)y, 0);
			if (parent == null) {
				parent = new GameObject().transform;
				parent.gameObject.name = "Blocks";
			}
			if (type == BlockType.player)
				renderer.name = "PLAYER";
			renderer.transform.SetParent (parent);
			BlockVisualManager manager = renderer.GetComponent<BlockVisualManager>();
			manager.block = this;
			manager.posx = x;
			manager.posy = y;
			
			visualiser = renderer;
			Sprite sprite = GetSprite (type);
			visualiser.sprite = sprite;
		
			visualiser.gameObject.GetComponent<RandomlyTexturedObjectController>().Initialize();
			visualiser.gameObject.GetComponent<RandomlyTexturedObjectController> ().SetColor (GenerateTexture.GetPallete (type));
		}
	}

	/// <summary>
	/// Gets the sprite used for the given type
	/// </summary>	
	public static Sprite GetSprite(BlockType type){
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
		case BlockType.solid:
			objectName = "Solid";
			break;
		case BlockType.goal:
			objectName = "Goal";
			break;
		case BlockType.appendage:
			objectName = "Appendage";
			break;
		case BlockType.hazard:
			objectName = "Hazard";
			break;
		default:
			return null;
		}
		Texture2D texture =  GenerateTexture.CreateRandomTexture(GenerateTexture.GetAvailableColors(GenerateTexture.GetPallete(type)), 10, 10);
		return Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f), Mathf.Max(new int[2]{texture.height, texture.width}));
	}
	
	
	/// <summary>
	/// Sets the type of the block, and updates it's display (if any) to reflect the new type
	/// </summary>
	public void SetType(BlockType newType){
		if (newType != type) {
			if (isAlsoGoal && newType == BlockType.background) {
				type = BlockType.goal;
				if (visualiser != null) {
					visualiser.sprite = GetSprite (BlockType.goal);
				}
				return;
			}
			type = newType;
			if (visualiser != null) {
				visualiser.sprite = GetSprite (type);
				visualiser.gameObject.GetComponent<RandomlyTexturedObjectController> ().SetColor (GenerateTexture.GetPallete (type));
			}
		}
	}
	/// <summary>
	/// Gets the block's type
	/// </summary>	
	public BlockType getType(){
		return type;
	}

	/// <summary>
	/// Removes the display and sets the private visualiser variable to null
	/// </summary>
	public void RemoveObjectDisplay(){
		if (visualiser != null) {
			MonoBehaviour.Destroy(visualiser.gameObject);
			visualiser = null;
		}
	}
}
