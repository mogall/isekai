using UnityEngine;

public class PickableRecipe : Interactable {
	public CraftingRecipe recipe;

	public override void Interact() {
		base.Interact ();

		PickUp ();
	}
	
	void PickUp(){
		Debug.Log ("picking up recipe" + recipe.name);
		bool pickedUp = PlayerCrafting.instance.AddRecipeToList (recipe);
		if (pickedUp) {
			Destroy (gameObject);
		} else {
			Debug.Log ("recipe was not picked up for some reason");
		}

	}
}
