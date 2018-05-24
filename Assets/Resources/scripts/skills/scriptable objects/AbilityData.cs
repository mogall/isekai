using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//DONOW - new concept
// ability data stores basics of every ability, like name, costs etc
//ability can spawn variouss effects but the main prefab is the start effect(so a fireball, or a tick field, or something else)
//those effect spawn their own effects in turn(like damage on hit for fireball, tick effect for tick aoe field etc)
//effects have a target, so they appear in the place(like player, enemy, player weapon, vector3, currentpos, etc)
//MELEE IS HERE TOO
//make a bunch of methods related to moving in the player controller - dash, blink, push back, step forward etc, so you can have effects control the character and make stuff like dash attacks or teleports)
[CreateAssetMenu(fileName = "New Ability Data", menuName = "Abilities/Ability Data")]
public class AbilityData : ScriptableObject {  //TODO - make it so the skill itself has a mastery level like a talent(with additional effect in mastery levels?)
	new public string name = "New Ability";
	public AbilityType abilityType; 
	public TargetingMode targetingMode;
	public float castPoint = 0;
	public float castCooldown = 0;
	public Sprite UIsprite = null;
	public List<GameObject> startEffect = new List<GameObject>();
	public List<GameObject> tickEffect = new List<GameObject>();
	public List<GameObject> endEffect = new List<GameObject>(); 
	//TODO - add start(?) sounds and stuff here, maybe effects or should the go to the prefab? also game effects or should they just go to the skill monobehaviour?

	/*public TargetingMode GetTargetingMode(){
		return targetingMode;
	}*/

	public void Use () {
		foreach (GameObject effect in startEffect) {
			GameObject abilityObject = Instantiate (effect, PlayerController.instance.abilityOriginator.position, Quaternion.identity);
			abilityObject.transform.position = PlayerController.instance.abilityOriginator.position; //FIX - if i'm setting the position above, do i need to set it here?
			//Ability ability = abilityObject.GetComponent<Ability> ();//TODO - is this needed?
			//ability.Fire (); //TODO - is this needed?
		}

	}
}
