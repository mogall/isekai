using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
	public GameObject defaultItemPrefab;
	public Sprite defaultUISprite;

	public List<ItemData> items = new List<ItemData> ();

	public static ItemManager instance;
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
		CheckItemsForMissingData();
	}
	void Start(){ //TODO - this is really a bit temporary //TODO - add database populating here
		
	}
	/*public bool SpawnItemInWorld(ItemData item){ //DONOW - WAIT FOR NOW
		
	}
	public bool SpawnItemInWorld(int itemID){

	}*/
	void CheckItemsForMissingData(){ //TODO - should this exist later? i think i should have a failsafe in case some item has some fucked up or missing data. push it into db parser later?
		foreach (ItemData item in items) {
			if (item.prefab == null) {
				item.prefab = defaultItemPrefab;
			}
			if (item.uiSprite == null) {
				item.uiSprite = defaultUISprite;
			}
		}
	}
	public ItemData GetItemFromDatabase(int id){
		ItemData item = items.Find (x => x.ID == id);
		return item;
	}


}
