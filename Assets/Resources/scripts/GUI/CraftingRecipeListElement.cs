using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeListElement : MonoBehaviour {
	public CraftingRecipe recipe;

	public void SetSelectedRecipe(){
		PlayerCrafting.instance.SetSelectedRecipe (recipe);
	}
}