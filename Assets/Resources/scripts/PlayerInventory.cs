using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	public delegate void OnInventoryChanged ();
	public OnInventoryChanged onInventoryChangedCallback;

	public List<ItemData> inventoryItems = new List<ItemData>(); //TODO - change this along with the gui method so the player can move the inventory items around
	//DONOW - make stacks of items

	public static PlayerInventory instance;
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
	public bool Add (ItemData item){
		inventoryItems.Add (item);
		if (onInventoryChangedCallback != null) {
			onInventoryChangedCallback.Invoke ();
		}
		return true;
	}
	public void Remove (ItemData item){
		inventoryItems.Remove (item);
		if (onInventoryChangedCallback != null) {
			onInventoryChangedCallback.Invoke ();
		}
	}
	public void Drop(ItemData item){  //TODO - this only takes itemdata, is this safe? will i make new item data sets that will fuck this up? monitor when spawning new items
		Remove (item);
		GameObject droppedItem = Instantiate (item.prefab, transform.position, Quaternion.identity); //TODO - figure out a way to drop items without this crude method, and with everything nnicely attached
		PickableItem data = droppedItem.GetComponent<PickableItem> ();
		data.itemData = item;
	}
	public bool HasItem(ItemData data){
		if(inventoryItems.Contains(data)){
			return true;
		}else{
			return false;
		}
	}
	public bool HasItems(List<ItemData> data){
		foreach (ItemData dataItem in data) {
			if (!inventoryItems.Contains(dataItem)) {
				return false;
			}
		}
		return true;
	}
}
