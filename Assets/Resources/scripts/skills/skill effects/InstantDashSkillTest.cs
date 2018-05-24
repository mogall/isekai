using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDashSkillTest : Ability {
	public float range;
	//float elapsedDist;
	Vector3 startPos;
	void Start(){
		PlayerController.instance.playerMovementState = PlayerMovementState.DASHING;
		startPos = PlayerController.instance.transform.position;
	}
	void Update () {
		PlayerController.instance.Dash (Vector2.up, 3);
		print ("dashing");
		float dist = Vector3.Distance (startPos, PlayerController.instance.transform.position);
		if (dist > range) {
			PlayerController.instance.playerMovementState = PlayerMovementState.STANDING;
			Destroy (this.gameObject);
		}
	}
}
