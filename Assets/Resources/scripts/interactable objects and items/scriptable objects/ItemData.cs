using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject {
	public int ID;
	new public string name = "New Item";
	public Sprite uiSprite = null;
	public GameObject prefab; //TODO - this is crude, change to something else, also shits itself if nothing is assigned because there's no default prefab, fix
	public bool stackable = false;
	public int stackSize = 1;

}
