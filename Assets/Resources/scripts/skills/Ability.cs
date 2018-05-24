using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {
	//public AbilityData abilityData;
	//public AbilityType abilityType;
	public List<GameObject> startEffect = new List<GameObject>();
	public List<GameObject> tickEffect = new List<GameObject>(); 
	public List<GameObject> endEffect = new List<GameObject>(); 
	//ADD - rework skill effect to add onhit etc.
	//public float castPoint;
	//public float castCooldown;
	//bool fired = false;

	/*public virtual void Fire(){
		fired = true;
	}*/ //REMOVE - this is not needed anymore, ability class is essentially an effect now, spawning new effects if necessary
}
