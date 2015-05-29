using UnityEngine;
using System.Collections;

public class GenerateTexture : MonoBehaviour {

	public struct Pallete {

		public Color[] availableColors;

		public Pallete(Color[] availableColors){
			this.availableColors = availableColors;
		}
	}

	public enum PalleteType{blue, red, grey, green, sand, teal};


	public static PalleteType GetPallete(Block.BlockType blockType){
		switch (blockType) {
		case Block.BlockType.appendage:
			return PalleteType.green;
		case Block.BlockType.background:
			return PalleteType.sand;
		case Block.BlockType.collectible:
			return PalleteType.blue;
		case Block.BlockType.goal:
			return PalleteType.teal;
		case Block.BlockType.hazard:
			return PalleteType.red;
		case Block.BlockType.player:
			return PalleteType.green;
		case Block.BlockType.solid:
			return PalleteType.grey;
		default:
			return PalleteType.sand;
		}
	}
	public static Color[] GetAvailableColors(PalleteType pallete){
		Color[] colors = new Color[1]{new Color (0, 0, 0)};
		switch (pallete) {
		case PalleteType.red:
			colors = new Color[3]{
				new Color(0.8f,0.015f,0.06f),
				new Color(0.75f,0.03f,0.03f),
				new Color(0.68f,0.07f,0.08f)
			};
			break;
		case PalleteType.blue:
			colors = new Color[3]{
				new Color(0.05f,0.25f,0.95f),
				new Color(0.03f,0.025f,0.8f),
				new Color(0.05f,0.05f,0.9f)
			};
			break;
		case PalleteType.grey:
			colors = new Color[3]{
				new Color(0.7f,0.7f,0.7f),
				new Color(0.4f,0.4f,0.4f),
				new Color(0.8f,0.8f,0.8f)
			};
			break;
		case PalleteType.green:
			colors = new Color[3]{
				new Color(0.12f,0.87f,0.1f),
				new Color(0.05f,0.43f,0.06f),
				new Color(0.1f,0.6f,0.1f)
			};
			break;
		case PalleteType.sand:
			colors = new Color[3]{
				new Color(0.95f,0.8f,0.74f),
				new Color(0.93f,0.8f,0.78f),
				new Color(0.8f,0.7f,0.65f)
			};
			break;
		case PalleteType.teal:
			colors = new Color[3]{
				new Color(0.12f,0.87f,0.87f),
				new Color(0.05f,0.72f,0.74f),
				new Color(0.1f,0.78f,0.75f)
			};
			break;
		default:
			break;
		}
		return colors;
	}

	public static Texture2D CreateRandomTexture(Color[] colors, int sizex, int sizey){
		int textureSize = sizex * sizey;
		Color[] textureColors = new Color[textureSize];
		int amountOfColors = colors.Length;
		for (int i = 0; i < textureSize; i++) {
			int thisColor = Random.Range(0, amountOfColors);
			textureColors[i] = colors[thisColor];
		}
		Texture2D texture = new Texture2D(sizex, sizey);
		texture.SetPixels (textureColors);
		if (Mathf.Max (new int[2]{texture.width, texture.height}) > 300) {
			texture.filterMode = FilterMode.Bilinear;
		} else {
			texture.filterMode = FilterMode.Point;	
		}
		texture.Apply ();
		return texture;
	}
	public static Texture2D CreateRandomTexture(PalleteType pallete, int sizex, int sizey){
		Color[] colors = GetAvailableColors (pallete);

		int textureSize = sizex * sizey;
		Color[] textureColors = new Color[textureSize];
		int amountOfColors = colors.Length;
		for (int i = 0; i < textureSize; i++) {
			int thisColor = Random.Range(0, amountOfColors);
			textureColors[i] = colors[thisColor];
		}
		Texture2D texture = new Texture2D(sizex, sizey);
		texture.SetPixels (textureColors);
		if (Mathf.Max (new int[2]{texture.width, texture.height}) > 300) {
			texture.filterMode = FilterMode.Bilinear;
		} else {
			texture.filterMode = FilterMode.Point;	
		}
		texture.Apply ();
		return texture;
	}
	public static Texture2D ProgressTexture(Texture2D source, bool dontTakeOver, Color[] pallette){
		int width = source.width;
		int height = source.height;
		int totalSize = width * height;
		Color[] newTexture = new Color[totalSize];
		if (!dontTakeOver) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					int xQuery = Random.Range (-1, 1);
					int yQuery = Random.Range (-1, 1);
					newTexture [x + y * width] = source.GetPixel ((x + xQuery) % width, (y + yQuery) % height);
				}
			}
		}
		if (dontTakeOver) {
			//Setup
			int[] appearancesOfColor = new int[pallette.Length];
			for(int i = 0; i < appearancesOfColor.Length; i++){
				appearancesOfColor[i] = 0;
			}
			//Register how many pixels are each color
			foreach(Color pixel in newTexture){
				for(int i = 0; i < pallette.Length;i++){
					if(pixel == pallette[i]) appearancesOfColor[i]++;
				}
			}
			//Determine chance of a pixel to be replaced by a given color
			float[] chanceForColorChange = new float[pallette.Length];
			float totalChance = 0;
			for(int i = 0; i < chanceForColorChange.Length; i++){
				float specificChance = ((float)totalSize - appearancesOfColor[i]) / ((float)totalSize * pallette.Length * 2);
				chanceForColorChange[i] = totalChance + specificChance;
				totalChance += specificChance;
			}
			//Go through the array, with a chance to reset each pixel with a color, increased chance for rare colors
			for(int x = 0; x < width; x++){
				for(int y = 0; y < height; y++){
					float randomValue = Random.Range(0f,1f);
					if(randomValue >= chanceForColorChange[chanceForColorChange.Length - 1]){
						int xQuery = Random.Range(-1, 1);
						int yQuery = Random.Range(-1, 1);
						newTexture[x + y * width] = source.GetPixel((x + xQuery) % width, (y + yQuery) % height);
					}
					else{
						for(int j = 0; j < chanceForColorChange.Length; j++){
							if(randomValue < chanceForColorChange[j]){
								newTexture[x + y * width] = pallette[j];
								break;
							}
						}
					}
				}
			}
		}
		source.SetPixels (newTexture);
		source.Apply ();
		return source;
	}


}
