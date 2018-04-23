using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {

	PlayerInventory inventory; //DONOW - spawn gui inventory slots according to the inventory size(right now they're just there statically)
	PlayerCrafting crafting;
	PlayerController playerController;

	int openPanelsNumber = 0;
	List<GameObject> openPanels = new List<GameObject>();
	public GameObject inventoryWrapper;
	public GameObject inventoryGUIPanel;
	List<InventorySlot> inventorySlot = new List<InventorySlot>(); //TODO - why is this a list?
	public GameObject itemDragImage;
	public GameObject inventorySlotPrefab;
	public GameObject ContainterPanelWrapper;
	public Text containerPanelName;
	public GameObject progressBar;
	public Image progressBarFill;

	public GameObject skillBarWrapper;
	public GameObject skillChoiceWindowWrapper;
	public GameObject skillChoiceWindowIconPrefab;
	public int skillBarSkillSetSlotNumber = 0;
	public Image[] skillBarImage = new Image[4]; //TODO - make this use some sort of a global var for skillbar size(not that it would get dynamically changed or something, but for clarity)

	public GameObject craftingPanelWrapper;
	public GameObject recipeList;
	public GameObject recipeListElement;
	public GameObject ingredientList;
	public GameObject resultList;
	public GameObject ingredientListElement;


	public static PlayerGUI instance;
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
	// Use this for initialization
	void Start () {
		inventory = PlayerInventory.instance;
		playerController = PlayerController.instance;
		crafting = PlayerCrafting.instance;
		inventory.onInventoryChangedCallback += UpdateInventorySlots;
		crafting.onCraftingListChangedCallback += UpdateRecipeList;

		/*foreach (Transform child in inventoryGUIPanel.transform) {
			inventorySlot.Add (child.GetComponent<InventorySlot>());
		}*/
		//inventorySlot = inventoryGUIPanel.GetComponentsInChildren<Image> (); 
		progressBarFill.fillAmount = 0;
		progressBar.SetActive (false);
		itemDragImage.SetActive (false);
		for (int i = 0; i < skillBarImage.Length; i++) {
			skillBarImage [i] = skillBarWrapper.transform.GetChild (i).GetComponent<Image> ();
		}
		PopulateGUISkillLists ();
		UpdateRecipeList ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.I)){
			ToggleGUIPanel (inventoryWrapper);
		}
		if(Input.GetKeyDown(KeyCode.K)){
			OpenRecipePanel ();
		}
	}
	public void UpdateGUIInventorySlotNumber(int j){
		for (int i = 0; i < j; i++) { //TODO - pool this
			GameObject slot = Instantiate(inventorySlotPrefab, transform.position, Quaternion.identity);
			slot.transform.parent = inventoryGUIPanel.transform;
			InventorySlot invSlot = slot.GetComponent<InventorySlot> ();
			invSlot.slotID = i;
			inventorySlot.Add (invSlot);
		}
	}
	void UpdateInventorySlots(){
		Debug.Log("updating inventory slots");
		for (int i = 0; i < inventory.inventorySize; i++) {
			if (inventory.inventory [i].stackSize < 1) {
				inventorySlot [i].ClearSlotData ();
			} else {
				inventorySlot [i].AddItemInSlot (inventory.inventory [i].item, inventory.inventory[i].stackSize);
			}
		}
	}
	public void OpenContainerPanel(List<ItemData> items, string containerName){
		CleanContainerGUIData ();
		ContainterPanelWrapper.SetActive (true);
		openPanelsNumber++;
		openPanels.Add (ContainterPanelWrapper);
		playerController.SetCameraMode (CameraMode.UI);
		Transform slotWrapper = ContainterPanelWrapper.transform.GetChild (0);
		for (int i = 0; i < items.Count; i++) {
			slotWrapper.GetChild (i).GetComponent<ContainerSlot> ().AddItemInSlot (items [i], items[i].stackSize);
		}
		containerPanelName.text = containerName;
	}
	void CleanContainerGUIData(){
		Transform slotWrapper = ContainterPanelWrapper.transform.GetChild (0);
		foreach (Transform child in slotWrapper.transform) {
			child.GetComponent<ContainerSlot> ().CleanContainterSlotData ();
		}
	}
	public void ToggleGUIPanel(GameObject target){ //TODO - this is pretty crude and temporary - it only closes panels without checking what is really going on, if the player is doing any action, changing skills, etc. upgrade it
		//Debug.Log (target.name);
		if (target.activeSelf) {
			target.SetActive (false);
			openPanels.Remove (target);
			openPanelsNumber--;
			if (openPanelsNumber == 0) {
				playerController.SetCameraMode (CameraMode.MOVEMENT);
			}
		} else {
			target.SetActive (true);
			if (openPanelsNumber == 0) {
				playerController.SetCameraMode (CameraMode.UI);
			}
			openPanels.Add (target);
			openPanelsNumber++;
		}
	}
	public void ClearAllGUIPanels(){ //TODO - this is pretty crude and temporary - it only closes panels without checking what is really going on, if the player is doing any action, changing skills, etc. upgrade it
		foreach (GameObject panel in openPanels) {
			panel.SetActive (false);
		}
		openPanelsNumber = 0;
		openPanels.Clear ();
	}
	public void SetProgressBarFillAmount(float maxVal, float amount, bool active){
		float value = (amount / maxVal);
		progressBarFill.fillAmount = value;
		progressBar.SetActive (active);
	}
	public void PopulateGUISkillLists(){ //this populated the skill lists that need a list of skills(icons and such) from the knownSkills list
		for (int i = 0; i < PlayerSkills.instance.knownSkills.Count; i++) {
			GameObject icon = Instantiate (skillChoiceWindowIconPrefab, transform.position, Quaternion.identity);
			icon.transform.parent = skillChoiceWindowWrapper.transform;
			icon.GetComponent<Image> ().sprite = PlayerSkills.instance.knownSkills [i].UIsprite;
			icon.GetComponent<SkillChoiceIcon> ().skillData = PlayerSkills.instance.knownSkills [i];
		}
	}//TODO - make a function for adding to the lists when character learns a new skill AND MAKE IT A DELEGATE LIKE WITH THE INVENTORY CALLBACK THING

	public void UpdateRecipeList(){ //TODO - make a callback when learning new recipe and subscribe this to it, so it can auto update the gui when learning something new(like with the inventory)
		print("updating recipe list");
		foreach (Transform child in recipeList.transform) {
			Destroy (child.gameObject); //TODO - pool this later and refactor, now it's ugly
		}
		foreach (CraftingRecipe recipe in PlayerCrafting.instance.knownRecipes) {
			GameObject listItem = Instantiate (recipeListElement, transform.position, Quaternion.identity);
			listItem.transform.parent = recipeList.transform;
			listItem.transform.localScale = new Vector3 (1, 1, 1);
			listItem.GetComponent<CraftingRecipeListElement> ().recipe = recipe;
			listItem.transform.GetChild (0).GetComponent<Image> ().sprite = recipe.resultItem [0].uiSprite;
			listItem.transform.GetChild (1).GetComponent<Text> ().text = recipe.recipeName;
		}
	}
	public void SetSelectedRecipe(){
		ClearRecipeWindow ();
		CraftingRecipe recipe = PlayerCrafting.instance.selectedRecipe;
		if (recipe != null) {
			foreach (ItemData recipeItem in recipe.recipeItem) {
				GameObject ingredient = Instantiate (ingredientListElement, transform.position, Quaternion.identity);
				ingredient.transform.parent = ingredientList.transform;
				ingredient.transform.localScale = new Vector3 (1, 1, 1);
				Image img = ingredient.GetComponent<Image> ();
				img.sprite = recipeItem.uiSprite;
				if (!PlayerInventory.instance.HasItem (recipeItem)) {
					img.color = Color.red;
				}
			}
			foreach (ItemData resultItem in recipe.resultItem) {
				GameObject result = Instantiate (ingredientListElement, transform.position, Quaternion.identity);
				result.transform.parent = resultList.transform;
				result.transform.localScale = new Vector3 (1, 1, 1);
				result.GetComponent<Image> ().sprite = resultItem.uiSprite;
			}
		}
	}
	void ClearRecipeWindow(){
		foreach (Transform ingredient in ingredientList.transform) {
			Destroy (ingredient.gameObject);
		}
		foreach (Transform result in resultList.transform) {
			Destroy (result.gameObject);
		}
	}
	public void OpenRecipePanel(){
		if (!craftingPanelWrapper.activeSelf) {
			SetSelectedRecipe ();
			craftingPanelWrapper.SetActive (true);
			openPanelsNumber++;
			openPanels.Add (craftingPanelWrapper);
			playerController.SetCameraMode (CameraMode.UI);
		} else {
			ToggleGUIPanel (craftingPanelWrapper);
		}

	}
}
