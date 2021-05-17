using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	Unit unit;
	Rigidbody rb;
	FieldOfView fieldOfView;

	public float moveSpeed = 5.0f;
	float moveBufferZone = 0.05f; // buffer zone to disable bugs in move algorythm

	public GameObject pathPointsHolder;
	List<Transform> pathPoints = new List<Transform>();

	//GameObject curMovePathPoint;
	int curMovePathPointIndex = 0;

	Vector3 moveDir;
	Vector3 moveTranslate;

	[HideInInspector]
	public bool isNeedStop = false;

	void Start() {
		unit = GetComponent<Unit>();
		rb = GetComponent<Rigidbody>();
		fieldOfView = GetComponent<FieldOfView>();

		// find all children path points in pathPointsHolder
		foreach (Transform pathPointT in pathPointsHolder.transform) {
			pathPoints.Add(pathPointT);
		}
	}

	void Update() {
		if (!isNeedStop) {
			// MOVE ENEMY UNIT BY PATH
			if (pathPoints.Count > 0) {
				float distToMovePoint = Vector3.Distance(transform.position, pathPoints[curMovePathPointIndex].position);

				// change current move point
				if (distToMovePoint < 1.5f) {
					if (curMovePathPointIndex + 1 > pathPoints.Count - 1) {
						curMovePathPointIndex = 0;
					} else {
						curMovePathPointIndex += 1;
					}
				}

				// move enemy by move points from path holder
				Transform curMovePoint = pathPoints[curMovePathPointIndex];

				if (transform.position.x < curMovePoint.position.x - moveBufferZone) {
					moveDir.x = 1;
					moveTranslate.x = moveSpeed;
				} else if (transform.position.x > curMovePoint.position.x + moveBufferZone) {
					moveDir.x = -1;
					moveTranslate.x = -moveSpeed;
				} else {
					moveTranslate.x = 0;
				}

				if (transform.position.z < curMovePoint.position.z - moveBufferZone) {
					moveDir.z = 1;
					moveTranslate.z = moveSpeed;
				} else if (transform.position.z > curMovePoint.position.z + moveBufferZone) {
					moveDir.z = -1;
					moveTranslate.z = -moveSpeed;
				} else {
					moveTranslate.z = 0;
				}

				// rotate unit by move dir
				unit.rotateByMoveDir(moveDir);
			}

			// CHECK IS PLAYER IN SIGHT SECTOR
			foreach (Transform visibleTarget in fieldOfView.visibleTargets) {
				if (visibleTarget.tag == "Player") {
					// GAME OVER
					GameObject.FindGameObjectWithTag("Game").GetComponent<Game>().isPlayerDead = true;
					isNeedStop = true;
				}
			}
		}
	}

	private void FixedUpdate() {
		if (!isNeedStop) {
			rb.MovePosition(transform.position + moveTranslate * Time.fixedDeltaTime);
			moveTranslate = Vector3.zero;
		}
	}
}
