using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS A SCRIPT FOR AN ENEMY ENTITY - THIS SCRIPT GOVERNS IDLE BEHAVIOUR, WITH ENEMY ENTITY BEING STATIONARY AND ABLE TO ROTATE

public class EnemyIdleStationary : Enemy { //TODO - UPGRADE
	public bool canIdlyRotate = true;
	public Vector2 rotationSpeed;
	public float rotationCooldown;
	Vector2 currentRotationTarget;
	//int xDir = 1;
	//int yDir = 1;
	//Vector2 amount;
	float currentRotationCooldown = 0;
	bool rotating = false;
	// Use this for initialization
	public override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		if (canIdlyRotate && inControl) {
			if (!rotating) {
				currentRotationCooldown += Time.deltaTime;
				if (currentRotationCooldown > rotationCooldown) {
					currentRotationTarget = CalculateRotationTarget ();
					rotating = true;
					currentRotationCooldown = 0;
					//xDir = Random.Range (0, 2) == 0 ? -1 : 1;
					//yDir = Random.Range (0, 2) == 0 ? -1 : 1;
					//amount = new Vector2 (0, 0);

				}
			} else {
				//TODO - this is simple and allright, but pretty crude, check behaviour.
				//TODO - upgrade to include only one axis rotation, or rotational constraints, slerp, etc.
				//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(currentRotationTarget.x, currentRotationTarget.y,0)), rotationSpeed.x * Time.deltaTime);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(currentRotationTarget.x, currentRotationTarget.y,0)), rotationSpeed.x * Time.deltaTime);
				if (Quaternion.Angle(transform.rotation, Quaternion.Euler(new Vector3(currentRotationTarget.x, currentRotationTarget.y,0)) ) < 1) {
					rotating = false;
				}
				/*print (currentRotationTarget.x);
				//transform.Rotate (currentRotationTarget * Time.deltaTime);
				if (amount.y < currentRotationTarget.y) {
					transform.Rotate (0, Time.deltaTime * yDir * rotationSpeed.y, 0);
					amount.y += Time.deltaTime * rotationSpeed.y;
				}
				if (amount.x < currentRotationTarget.x) {
					print ("A");
					transform.Rotate (Time.deltaTime * xDir * rotationSpeed.x, 0, 0);
					amount.x += Time.deltaTime * rotationSpeed.x;
				}
				if (amount.x >= currentRotationTarget.x && amount.y >= currentRotationTarget.y) {
					rotating = false;
				}*/

			}
		}

	}
	Vector2 CalculateRotationTarget(){ //TODO - this is pretty simple, upgrade?
		float x = Random.Range (-90f, 90f);
		float y = Random.Range (0f, 360f);
		return new Vector2(x,y);
	}
}
