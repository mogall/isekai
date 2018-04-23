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
	public void Craft(){ //DONOW - update crafting so it takes amounts of crafting elements, in accordance to inventory stack update
		if (selectedRecipe != null && PlayerInventory.instance.HasItems (selectedRecipe.recipeItem)) {
			foreach (ItemData item in selectedRecipe.recipeItem) {
				PlayerInventory.instance.RemoveOnID (item.ID, 1);
			}
			foreach (ItemData item in selectedRecipe.resultItem) {
				PlayerInventory.instance.Add (item);
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

}
