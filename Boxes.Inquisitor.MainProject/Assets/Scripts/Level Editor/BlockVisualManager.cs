using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlockVisualManager : MonoBehaviour {

	public Block block;

	void OnMouseOver(){


		if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject(-1) && !LevelEditorManager.isTestingCreatorsAbilities) {
			try{
				block.SetType (LevelEditorManager.currentType);
			}catch(NullReferenceException e){
				return;
			}
		}
	}

}
