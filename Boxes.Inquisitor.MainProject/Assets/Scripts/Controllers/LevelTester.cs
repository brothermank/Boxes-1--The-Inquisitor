using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelTester {

	private Block[,] data;

	public LevelTester(Block[,] levelData){
		data = levelData;
	}

	private static List<LevelController> getPermutations(Block[,] state){

		List<LevelController> perms = new List<LevelController> ();

		//--------RIGHT
			LevelController r = new LevelController (state);

			//Attempt to move right
			if (r.MoveRelatively (1, 0))
				//If possible, add it to the list of permutations
				perms.Add (r);

		//--------LEFT
			LevelController l = new LevelController (state);
		
			//Attempt to move left
			if (l.MoveRelatively (-1, 0))
				//If possible, add it to the list of permutations
			perms.Add (l.GetLevel ());

		//--------UP
			LevelController u = new LevelController (state);
			
			//Attempt to move up
			if (u.MoveRelatively (0, -1))
				//If possible, add it to the list of permutations
				perms.Add (u.GetLevel ());
		
		//--------DOWN
			LevelController d = new LevelController (state);
			
			//Attempt to move up
			if (d.MoveRelatively (0, 1))
				//If possible, add it to the list of permutations
				perms.Add (d.GetLevel ());

		return perms;
	}

	public bool isPlayable(){
		List<LevelController> perms = getPermutations (data);

		for (int i=0; i<100000; i++) {
		
		}
		Debug.LogError ("Command timed out: isPlayable();");
		return false;

	}

}
