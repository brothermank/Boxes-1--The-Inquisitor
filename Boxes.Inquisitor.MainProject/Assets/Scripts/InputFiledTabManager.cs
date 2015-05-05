using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InputFiledTabManager : MonoBehaviour {

	InputField[] inputFields;
	int currentInputfieldIndex = -1;

	void Start(){
		inputFields = GetComponentsInChildren<InputField> ();
		Debug.Log(inputFields.Length);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			currentInputfieldIndex = (currentInputfieldIndex + 1) % inputFields.Length;
			EventSystem.current.SetSelectedGameObject(inputFields[currentInputfieldIndex].gameObject, null);
			inputFields[currentInputfieldIndex].OnPointerClick (null);
		}
	}
}
