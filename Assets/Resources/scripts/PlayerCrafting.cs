using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafting : MonoBehaviour {
	public List <CraftingRecipe> knownRecipes = new List<CraftingRecipe>();
	public CraftingRecipe selectedRecipe;

	public delegate void OnCraftingListChanged ();
	public OnCraftingListChanged onCraftingListChangedCallback;


	public static PlayerCrafting instance;
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
	void Start(){
		//selectedRecipe = knownRecipes [0]; //TODO -wow this is a fucking hack, fix this so when you open a window you don't have to hard code a first recipe the first time
	}
	public void Craft(){ 
		if (selectedRecipe != null && CheckRecipeIngredientsAvailability(selectedRecipe.recipeItems) == true && CheckRecipeToolAvailability(selectedRecipe.requiredTool) == true) {
			foreach (RecipeItem item in selectedRecipe.recipeItems) {
				PlayerInventory.instance.RemoveOnID (item.itemData.ID, item.amount);
			}
			foreach (RecipeItem item in selectedRecipe.resultItems) {
				if (item.itemData.stackable == false) {
					for (int i = 0; i < item.amount; i++) {
						PlayerInventory.instance.Add (item.itemData);
					}
				} else {
					item.itemData.stackSize = item.amount; 
					PlayerInventory.instance.Add (item.itemData); 
				}

			}
			print ("item crafted!");
			PlayerGUI.instance.SetSelectedRecipe (); //TODO - make a proper gui update when you finish crafting, calling this method is a hack
		} else {
			print ("can't craft!");
		}
	}
	public void SetSelectedRecipe(CraftingRecipe recipe){
		selectedRecipe = recipe;
		PlayerGUI.instance.SetSelectedRecipe ();
		//TODO - set the recipe window here... or maybe in the UI? and just call the method here
	}
	public bool AddRecipeToList(CraftingRecipe recipe){
		if (knownRecipes.Contains (recipe) == false) {
			knownRecipes.Add (recipe);
			if (onCraftingListChangedCallback != null) {
				onCraftingListChangedCallback.Invoke ();
			}
			return true;
		} else {
			print ("recipe already known!");
			return false;
		}

	}
	public bool RemoveRecipeFromList(CraftingRecipe recipe){
		if (selectedRecipe == recipe) {
			selectedRecipe = null;
		}
		knownRecipes.Remove (recipe);
		if (onCraftingListChangedCallback != null) {
			onCraftingListChangedCallback.Invoke (); //
		}
		return true;
	}
	bool CheckRecipeIngredientsAvailability(List<RecipeItem> recipeList){ //UPGRADE - as written in inventory, this seems to be very slow, iterating through whole inventory every item
		foreach (RecipeItem item in recipeList) { 
			if (PlayerInventory.instance.HasItem (item.itemData, item.amount) == false) {
				return false;
			}
		}
		return true;
	}
	bool CheckRecipeToolAvailability(List<ItemData> toolList){  //TODO - does this check/run/return true if the list is empty(i.e. no tools required)?
		foreach (ItemData tool in toolList) {
			if (PlayerInventory.instance.HasItem(tool) == false) {
				return false;
			}
		}
		return true;
	}

}
