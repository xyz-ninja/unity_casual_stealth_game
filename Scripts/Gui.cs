using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gui : MonoBehaviour
{
	Transform Background;

	public GameObject GameOverTab;
	public GameObject CompletedTab;

	public Text scoreLabel;

	Game game;

	void Start() {
		game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();

		hide();
	}

	public void show() {
		gameObject.SetActive(true);
		transform.Find("Background").gameObject.SetActive(true);
	}

	public void hide() {
		gameObject.SetActive(false);
	}

	public void showCompletedTab() {
		show();

		GameOverTab.SetActive(false);
		CompletedTab.SetActive(true);

		scoreLabel.text = (
			game.getPlayer().GetComponent<Player>().scoreCount.ToString() + "/" + game.getCurrentLevel().GetComponent<Level>().getStarsCountOnLevel()
		);
	}

	public void showGameOverTab() {
		show();

		CompletedTab.SetActive(false);
		GameOverTab.SetActive(true);
		
	}

	// BUTTONS EVENTS
	public void completeLevel() {
		game.LoadLevel(game.curGameLevel + 1);
		hide();
	}

	public void restartLevel() {
		game.LoadLevel(game.curGameLevel);
		hide();
	}


}
