using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Interactable {
	public string containerName;
	public List<ItemData> items = new List<ItemData> ();
	//int maxSlots; //ADD

	public override void Interact(){
		PlayerGUI.instance.OpenContainerPanel (items, containerName);
		PlayerController.instance.container = this;
	}
}
