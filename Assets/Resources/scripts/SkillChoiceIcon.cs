using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChoiceIcon : MonoBehaviour { //this is the skill choice icon script in the skill choice window - every skill icon has one and this is called when the button on the skill icon is pressed
	public AbilityData skillData;

	public void SetThisSkillInSkillSlot(){
		PlayerSkills.instance.SetSkillInSlot (PlayerGUI.instance.skillBarSkillSetSlotNumber, skillData);
	}
}
