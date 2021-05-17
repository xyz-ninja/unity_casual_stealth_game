using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	// return count of all stars on level
	public int getStarsCountOnLevel() {
		int starCount = 0;
		foreach(Transform star in transform.Find("Stars")) {
			starCount++;
		}
		return starCount;
	}

	public GameObject getPlayer() {
		return GameObject.Find("Player"); 
	}
}
