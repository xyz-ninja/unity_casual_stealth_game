using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
	void Start() {
		
	}

	void Update() {
		
	}

	// player finish level
	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			var game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
			var enemies = GameObject.FindGameObjectsWithTag("Enemy");

			foreach (var enemy in enemies) enemy.GetComponent<Enemy>().isNeedStop = true;

			game.levelCompleted = true;
		}
	}
}
