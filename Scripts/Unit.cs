using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public int health = 1;

	void Start() {
		
	}

	void Update() {
		
	}

	// rotate unit body by them move dir
	public void rotateByMoveDir(Vector3 _dir) {
		Vector3 curDir = _dir;

		// if unit dont stand
		if (curDir.x != 0 || curDir.z != 0) {
			Vector3 curEulerAngles = gameObject.transform.eulerAngles;

			float selYAngles = 0;
			// left/right
			if (curDir.x < 0) selYAngles = -90f;
			else if (curDir.x > 0) selYAngles = 90f;

			// up/down
			if (curDir.z > 0) selYAngles = 0f;
			else if (curDir.z < 0) selYAngles = 180f;

			gameObject.transform.eulerAngles = new Vector3(curEulerAngles.x, selYAngles, curEulerAngles.z);
		}
	}
}
