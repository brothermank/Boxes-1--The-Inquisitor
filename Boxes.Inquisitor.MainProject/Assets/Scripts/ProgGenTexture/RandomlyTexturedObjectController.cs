using UnityEngine;
using System.Collections;

public class RandomlyTexturedObjectController : MonoBehaviour{

	protected SpriteRenderer render;
	protected int updatesSinceLastMove = 0;
	protected int updatesSinceLastCleanup = 0;
	protected int requiredUpdatesSinceLastMove = 3;
	protected int requiredUpdatesSinceLastCleanup = 45;
	private GenerateTexture.PalleteType color = GenerateTexture.PalleteType.green;
	private Color[] palleteColors;

	bool flowingTexture = true;
	
	void Start(){
		Initialize ();
	}
	void FixedUpdate(){
		CallOnFixedUpdate ();
	}

	public void SetColor(GenerateTexture.PalleteType color){
		this.color = color;
		palleteColors = GenerateTexture.GetAvailableColors (color);
		render.sprite.texture.SetPixels(GenerateTexture.CreateRandomTexture (palleteColors, 10, 10).GetPixels());
		render.sprite.texture.Apply ();
		if (color == GenerateTexture.PalleteType.sand || color == GenerateTexture.PalleteType.grey) {
			flowingTexture = false;
		}
		else {
			flowingTexture = true;
		}
	}
	public GenerateTexture.PalleteType GetColor(){
		return color;
	}
	public Color[] GetColors(){
		return palleteColors;
	}

	// Use this for initialization
	public void Initialize () {
		palleteColors = GenerateTexture.GetAvailableColors (color);
		render = gameObject.GetComponent<SpriteRenderer> ();
		Texture2D texture = GenerateTexture.CreateRandomTexture (palleteColors, 10, 10);

		render.sprite = Sprite.Create (texture, new Rect (0, 1, texture.width, texture.height), new Vector2 (0.5f, 0.5f), Mathf.Max (new int[2] {
			texture.width,
			texture.height
		}));
	}
	
	// Update is called once per frame
	protected void CallOnFixedUpdate () {
		if (flowingTexture) {
			updatesSinceLastMove++;
			updatesSinceLastCleanup++;
			if (updatesSinceLastMove >= requiredUpdatesSinceLastMove) {
				if (updatesSinceLastCleanup >= requiredUpdatesSinceLastCleanup) {
					GenerateTexture.ProgressTexture (render.sprite.texture, true, palleteColors);
					updatesSinceLastCleanup = Random.Range(0, 20);
				} else
					GenerateTexture.ProgressTexture (render.sprite.texture, false, palleteColors);
				updatesSinceLastMove = 0;
			}
		}
	}
}
