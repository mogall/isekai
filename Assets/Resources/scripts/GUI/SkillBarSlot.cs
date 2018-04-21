using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarSlot : MonoBehaviour { //this is a controller for the skillbar slot - right now it only responds to click, to open the skill choice window for setting a new skill in the slot
	public int slotNum;

	public void ChooseSkillForThisSlot(){
		PlayerGUI.instance.skillBarSkillSetSlotNumber = slotNum;
		PlayerGUI.instance.ToggleGUIPanel (PlayerGUI.instance.skillChoiceWindowWrapper);
	}
}
