using UnityEngine;

public class PickableItem : Interactable {
	public ItemData itemData;

	public override void Interact() {
		base.Interact ();

		PickUp ();
	}

	void PickUp(){ //invoked when item is picked up from the ground
		Debug.Log ("picking up " + itemData.name);
		bool pickedUp = PlayerInventory.instance.Add (itemData);
		if (pickedUp) {
			Destroy (gameObject);
		} else {
			Debug.Log ("item was not picked up for some reason");
		}

	}
	void InventoryUse(){ //invoked when item is right-clicked in inventory //TODO - think about if it's supposed to be right clicked and what not - look at other games
		print ("clicked on an item in inventory");
	}
}
