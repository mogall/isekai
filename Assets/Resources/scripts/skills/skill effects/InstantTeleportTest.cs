using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantTeleportTest : Ability {
	public float range;

	void Start(){
		PlayerController.instance.TeleportToCursor (range);
		Destroy (this.gameObject);
	}
}
