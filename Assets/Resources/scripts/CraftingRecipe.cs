using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject {
	public string recipeName;
	public List<ItemData> recipeItem = new List<ItemData>();
	public List<ItemData> resultItem = new List<ItemData> ();
	public List<ItemData> requiredTool = new List<ItemData>(); //TODO - this is for a required tool in an inventory(doesn't get used in recipe), remember to implement it in recipes and crafting
	public CraftStationType requiredCraftStation = CraftStationType.HAND;
	public float craftTime;
}
