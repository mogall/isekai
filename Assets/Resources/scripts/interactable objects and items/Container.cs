using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Interactable {
	public string containerName;
	public List<ItemData> items = new List<ItemData> ();

	public int[] itemIDList;
	//TODO - add a loot table/drop table here when you implement it
	//int maxSlots; //ADD

	public override void Interact(){
		PlayerGUI.instance.OpenContainerPanel (items, containerName);
		PlayerController.instance.container = this;
	}
	public override void Start(){ //UPGRADE	 - change this to Init() when you spawn containers, so you have control when you want the container to do something at start
		base.Start();
		foreach (int id in itemIDList) {
			items.Add (ItemManager.instance.GetItemFromDatabase (id));
		}
	}
}
