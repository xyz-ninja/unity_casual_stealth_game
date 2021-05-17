using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : MonoBehaviour
{
	Gui GUI;

	public int curGameLevel = 1;
	
	[HideInInspector]
	public GameObject curLevel;

	// changes from finish.cs and player.cs
	public bool levelCompleted = false;
	public bool isPlayerDead = false;

	void Start() {
		GUI = transform.Find("GUI").GetComponent<Gui>();

		LoadLevel(curGameLevel);
	}

	void Update() {
		if (levelCompleted) {
			GUI.showCompletedTab();

			Time.timeScale = 0;
		} else {
			if (isPlayerDead) {
				GUI.showGameOverTab();

				Time.timeScale = 0;
			}
		}
	}

	// load level by num
	public void LoadLevel(int _levelNum) {
		levelCompleted = false;
		isPlayerDead = false;

		if (curLevel != null) {
			Destroy(curLevel);
		}

		curGameLevel = _levelNum;

		// load level from file
		string levelPrefabPath = "";
		if (curGameLevel == 1) levelPrefabPath = "Levels/Level1";
		else if (curGameLevel == 2) levelPrefabPath = "Levels/Level2";
		else if (curGameLevel == 3) levelPrefabPath = "Levels/Level3";
		else Application.Quit();

		var loadedPrefabResource = LoadPrefabFromFile(levelPrefabPath);
		curLevel = Instantiate(loadedPrefabResource, Vector3.zero, Quaternion.identity) as GameObject;
		curLevel.transform.parent = transform;

		Time.timeScale = 1;
		//GUI.hide();
	}

	// just load prefab from folder
	private static UnityEngine.Object LoadPrefabFromFile(string _filepath) {
		Debug.Log("Trying to load LevelPrefab from file (" + _filepath + ")...");
		//var loadedObject = Resources.Load("Levels/" + filename);
		var loadedObject = Resources.Load(_filepath);
		if (loadedObject == null) {
			throw new FileNotFoundException("...no file found - please check the configuration");
		}
		return loadedObject;
	}
	
	public GameObject getCurrentLevel() {
		return curLevel;
	}

	public GameObject getPlayer() {
		if (getCurrentLevel() == null) {
			return null;
		} else {
			return getCurrentLevel().GetComponent<Level>().getPlayer();
		}
	}

}
