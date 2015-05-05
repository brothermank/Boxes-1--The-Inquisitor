using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlockVisualManager : MonoBehaviour {

	public Block block;
	public int posx;
	public int posy;

	/// <summary>
	/// If the mouse is over the block, checks if the mouse is also clickeded, and whether there is a GUI element in the way. If not, sets the block to the
	/// type specified in LevelEditorManager.currentType), unless it is already the same type.
	/// </summary>	
	void OnMouseOver(){
		if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject(-1) && !LevelEditorManager.isTestingCreatorsAbilities) {
			try{
				if(LevelEditorManager.currentType != block.getType()){
				LevelEditorManager.moves.Push(new LevelEditorManager.Move(block.getType(), posx, posy));
				LevelEditorManager.undoneMoves = new Stack();
				block.SetType (LevelEditorManager.currentType);
				}
			}catch(NullReferenceException e){
				return;
			}
		}
	}

}
