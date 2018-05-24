using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//DONOW - RENAME SKILLS TO ABILITIES
public class PlayerSkills : MonoBehaviour {
	public List<AbilityData> knownSkills = new List<AbilityData>();
	public AbilityData[] skillBar = new AbilityData[4];
	public AbilityData targetingSkill = null;
	GameObject currentTargeter;
	public GameObject pointTargeter;

	public AbilityData mainHandSkill;

	public static PlayerSkills instance;
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}

	void Update(){
		//Debug.DrawRay (ray.origin, ray.direction*10, Color.green, 0.1f);
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			UseSkill (skillBar[0]);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			UseSkill (skillBar[1]);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			UseSkill (skillBar[2]);
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)){
			targetingSkill = skillBar [3];
			if (targetingSkill != null) {
				PlayerController.instance.playerActionState = PlayerActionState.TARGETING;
				TargetingMode mode = targetingSkill.targetingMode;
				EnableTargeting (mode);  //TODO 3 OPTIONS: 1. drop taargeting altogether and just use crosshair, 2 redo skills for targeting, 3 somehow hack it, 4 just leave targeting line and that's it(it looks shite, though)
			} else {
				print ("no skill in slot 4");
			}

			//UseSkill (3);
		}
		if (Input.GetMouseButtonDown (0) && PlayerController.instance.cameraMode == CameraMode.MOVEMENT) { //TODO - check for weapon type here and attack accordingly
			if (PlayerController.instance.playerActionState == PlayerActionState.NOTHING) {//TODO - wouldn't it be better to have a callback when camera mode changes?
				//UseMainHandSkill ();
				MeleeAttack();
			}else if (PlayerController.instance.playerActionState == PlayerActionState.TARGETING) {
				print ("using skill");
				UseSkill (targetingSkill);
				ClearTargeting ();
			}
		}
		if (Input.GetMouseButtonDown(1) && PlayerController.instance.cameraMode == CameraMode.MOVEMENT ) {
			if (PlayerController.instance.playerActionState == PlayerActionState.NOTHING) {//TODO - wouldn't it be better to have a callback when camera mode changes?
				//TODO - use offhand skill here
			}else if (PlayerController.instance.playerActionState == PlayerActionState.TARGETING) {
				ClearTargeting ();
			}
		}
	}
	public void MeleeAttack(){ //DONOW - change the name of this
		//DONOW - implement melee system IMMEDIATELY
		//DONOW - after immplementing weapons, expand this so it gets data from a weapon(attack speed etc), and gets animation from it too.
		//ADD - delay for attack point here
		//ADD - different types of attacks, it's only raycast with range now
		//ADD - animation here
		RaycastHit hit;
		if (Physics.Raycast (PlayerController.instance.cameraBoom.transform.position, PlayerController.instance.cameraBoom.transform.forward, out hit, 1)) { //TODO - all temporary stats
			ITakeDamage enemy = hit.collider.GetComponent<ITakeDamage> (); 
			if (enemy != null) {
				print ("hitting enemy with melee!");
				enemy.TakeDamage (1); 
			} else {
				//do something here if not hit
			}

		}
	}
	/*void UseMainHandSkill(){ //REMOVE - moved to playercombat
		mainHandSkill.Use (); //TODO - this is a bit shit, no checks if there is a skill at all, so change it later, ALSO IT'S BROKEN AFTER REFACTORING SKILLS/SPELLS, FIX
	}*/
	void UseSkill(AbilityData _data){
		//SkillData data = knownSkills[slot];
		AbilityData data = _data;
		if (data != null) {
			data.Use ();
		} else {
			print ("no skill in slot");
		}


	}
	public void SetSkillInSlot(int slot, AbilityData skill){ //this sets the skill in the skill bar slot. it will automatically toggle the gui panel off for the skill choice
		skillBar [slot] = skill;
		PlayerGUI.instance.skillBarImage [slot].sprite = skill.UIsprite;
		PlayerGUI.instance.ToggleGUIPanel (PlayerGUI.instance.skillChoiceWindowWrapper);
		PlayerGUI.instance.skillBarSkillSetSlotNumber = 0;
	}
	void EnableTargeting(TargetingMode mode){
		switch (mode) {
		case TargetingMode.NOTARGETING:
			targetingSkill.Use ();
			break;
		case TargetingMode.POINT:
			currentTargeter = Instantiate (pointTargeter, PlayerController.instance.abilityOriginator.position, Quaternion.identity);
			//currentTargeter.GetComponent<PointTargeter> ().range = targetingSkill.range; //FIX - broken after refactoring skill effects
			break;
		default:
			print ("something wrong with the targeting mode, no targeter selected");
			break;
		}
	}
	void ClearTargeting(){
		targetingSkill = null;
		Destroy (currentTargeter);
		PlayerController.instance.playerActionState = PlayerActionState.NOTHING;
	}


}
