using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
	// THIS NEED TO VISUALIZE FIELDS OF VIEW SECTORS IN EDITOR	
	private void OnSceneGUI() {
		FieldOfView fow = (FieldOfView)target;
		Handles.color = Color.white;
		// draw radius arc
		Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);

		// draw sight sector
		Vector3 viewAngleA = fow.getDirFromAngle(-fow.viewAngle / 2, false);
		Vector3 viewAngleB = fow.getDirFromAngle(fow.viewAngle / 2, false);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

		// draw line between self and visible unit
		Handles.color = Color.red;
		foreach (Transform visibleTarget in fow.visibleTargets) {
			Handles.DrawLine(fow.transform.position, visibleTarget.position);
		}
	}
}
