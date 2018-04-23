using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	public delegate void OnInventoryChanged ();
	public OnInventoryChanged onInventoryChangedCallback;

	//public List<ItemData> inventoryItems = new List<ItemData>(); //TODO - change this along with the gui method so the player can move the inventory items around

	public struct InventorySlot
	{
		public ItemData item;
		public int stackSize;
	}
	public int inventorySize = 10;
	public InventorySlot[] inventory;

	//private Dictionary<ItemData, int> inventorySlot;
	//public InventorySlot[] inventory; //DONOW - inventory is a "list" of inventoryslots. it's an array so you can drag and drop(with list it gets shortened and whatnot.
	//DONOW - to add - search for first and if stackable, add to it. if not, add to first free. add drag and drop, dropping from inventory etc(add "droppable" property?)
	//DONOW - CHANGE OF PLANS - MAKE A STRUCT INVENTORYSLOT, THEN A SINGLE DIMENSION ARRAY OF THE STRUCTS
	//DONOW - OR ACTUALLY NOW, JUST DO AN ARRAY OF DICTIONARY STUFF
	//or maybe try dictionaries, like that dude wrote, just try to understandit with the gethashcode override
	//>Dictionary<Position, ItemSlot> itemSlots
	//>Dictionary<Item, List<ItemSlot>> itemsToItemSlots - that's what he had

	public static PlayerInventory instance;
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
	void Start(){
		inventory = new InventorySlot[inventorySize]; //TODO - make a method for changing inventory size when you pick up an new backpack or something
		PlayerGUI.instance.UpdateGUIInventorySlotNumber(inventorySize); 
	}
	public bool Add (ItemData item){
		int firstempty = -1;
		if (item.stackable) {
			for (int i = 0; i < inventory.Length; i++) {
				if (inventory [i].stackSize > 0 && inventory [i].item.ID == item.ID) {
					inventory [i].stackSize += item.stackSize;
					UpdateInventoryUI ();
					return true;
				} else if (inventory [i].stackSize == 0 && firstempty == -1) {
					firstempty = i;
				}
			}
			if (firstempty > -1) {
				inventory [firstempty].item = item;
				inventory [firstempty].stackSize = item.stackSize;
				UpdateInventoryUI ();
				return true;
			}
		} else {
			for (int i = 0; i < inventory.Length; i++) {
				if (inventory[i].stackSize == 0) {
					inventory [i].item = item;
					inventory [i].stackSize = 1;
					UpdateInventoryUI ();
					return true;
				}
			}
		}
		return false;
	}
	public void RemoveOnSlot(int i, int amount){
		if (amount > 0) {
			inventory [i].stackSize -= amount;
			if (inventory [i].stackSize <= 0) {
				inventory [i] = new InventorySlot ();
			}
		} else {
			inventory [i] = new InventorySlot ();
		}
		UpdateInventoryUI ();
	}
	public bool RemoveOnID(int id, int amount){
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i].item.ID == id) {
				if (amount > 0) {
					inventory [i].stackSize -= amount;
					if (inventory [i].stackSize <= 0) {
						inventory [i] = new InventorySlot ();
					}
				} else {
					inventory [i] = new InventorySlot ();
				}
				UpdateInventoryUI ();
				return true;
			}
		}
		return false;
	}
	public void MoveItemFromToSlot(int fromslot, int toslot){
		ItemData data = inventory [fromslot].item;
		int stackSize = inventory [fromslot].stackSize;
		if (inventory[toslot].stackSize > 0) {
			print ("MoveItemFromToSlot tried to move to a nonempty slot no." + toslot + ". Aborting");
			return;
		}
		inventory [toslot].item = data;
		inventory [toslot].stackSize = stackSize;
		inventory [fromslot] = new InventorySlot ();
		UpdateInventoryUI ();
	}
	public void SwitchItemsBetweenSlots(int slot1, int slot2){
		ItemData data1 = inventory [slot1].item;
		int stack1 = inventory [slot1].stackSize;
		inventory [slot1].item = inventory [slot2].item;
		inventory [slot1].stackSize = inventory [slot2].stackSize;
		inventory [slot2].item = data1;
		inventory [slot2].stackSize = stack1;
		UpdateInventoryUI ();
	}
	public void Drop(ItemData item, int amount){  //TODO - this only takes itemdata, is this safe? will i make new item data sets that will fuck this up? monitor when spawning new items
		bool removed = RemoveOnID (item.ID, amount);
		if (removed) {
			GameObject droppedItem = Instantiate (item.prefab, transform.position, Quaternion.identity); //TODO - figure out a way to drop items without this crude method, and with everything nnicely attached
			PickableItem data = droppedItem.GetComponent<PickableItem> ();
			data.itemData = item;
			data.itemData.stackSize = amount; //TODO - check if this is working correctly
		}
	}
	public bool HasItem(ItemData data){
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i].item.ID == data.ID) {
				return true;
			}
		}
		return false;
	}
	public bool HasItems(List<ItemData> data){
		foreach (ItemData dataItem in data) {
			if (!HasItem(dataItem)) {
				return false;
			}
		}
		return true;
	}
	void UpdateInventoryUI(){
		if (onInventoryChangedCallback != null) {
			onInventoryChangedCallback.Invoke ();
		}
	}
}
