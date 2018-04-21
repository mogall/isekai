using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : Interactable {
	public ItemData itemData;
	Collider coll;
	public bool depleted = false;
	public int gatheringNumber = 1;

	public override void Start(){
		base.Start ();
		coll = GetComponent<Collider> ();
	}

	public override void Interact(){
		base.Interact ();
		Debug.Log ("gathering up " + itemData.name);
		bool pickedUp = PlayerInventory.instance.Add (itemData);
		if (pickedUp) {
			gatheringNumber--;
			if (gatheringNumber < 1) {
				depleted = true;
				coll.enabled = false; //TODO - THIS IS TEMPORARY - CHANGE IT BECAUSE IF YOU DISABLE A COLLIDER, YOU'LL START CLIPPING THROUGH SHIT YOU DON'T WANT TO
			}
		} else {
			Debug.Log ("item was not gathered up for some reason");
		}
	}
}
