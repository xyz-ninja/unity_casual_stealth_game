using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Unit unit;
	Rigidbody rb;

	Camera mainCamera;

	public float movementSpeed = 5.0f;
	Vector3 moveDir;

	public int scoreCount = 0;

	void Start() {
		unit = GetComponent<Unit>();
		rb = GetComponent<Rigidbody>();

		mainCamera = Camera.main;
	}

	void Update() {
		Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(
			Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y)
		);

		// setup move dir of unit by getting axis value
		moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

		//transform.LookAt(mousePos +	Vector3.up * transform.position.y);
		
		// rotate character to move dir
		unit.rotateByMoveDir(moveDir);
	}

	private void FixedUpdate() {
		// move character
		//rb.velocity = moveDir * movementSpeed;
		rb.MovePosition(transform.position + moveDir * movementSpeed * Time.fixedDeltaTime);
	}

	private void OnTriggerEnter(Collider other) {
		// player can collect stars
		if (other.transform.GetComponent<Star>()) {
			scoreCount += other.transform.GetComponent<Star>().scoreCount;
			Destroy(other.gameObject);
		}
	}
}
