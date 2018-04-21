using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Skills/Skill Data")]
public class SkillData : ScriptableObject {  //TODO - make it so the skill itself has a mastery level like a talent(with additional effect in mastery levels?)
	new public string name = "New Skill";
	public SkillType skillType; 
	public TargetingMode targetingMode;
	public float range;
	public float castPoint = 0;
	public float castCooldown = 0;
	public Sprite UIsprite = null;
	public GameObject skillPrefab;
	public List<GameObject> triggerSkillEffects = new List<GameObject>();
	public List<GameObject> tickSkillEffects = new List<GameObject>();
	public List<GameObject> endSkillEffects = new List<GameObject>(); //TODO - THIS SYSTEM ISN'T BAD BUT CHANGE THIS TO ONTICK, PUSH THIS STUFF TO GENERIC CLASS AND INHERIT, AND THEN PUT A TICK THERE WITH SETUP FROM OBJECTS
	//TODO - add sounds and stuff here, maybe effects or should the go to the prefab? also game effects or should they just go to the skill monobehaviour?

	/*public TargetingMode GetTargetingMode(){
		return targetingMode;
	}*/

	public void Use () {
		GameObject skillObject = Instantiate (skillPrefab, PlayerController.instance.skillOriginator.position, Quaternion.identity);
		skillObject.transform.position = PlayerController.instance.skillOriginator.position;
		Skill skill = skillObject.GetComponent<Skill> ();
		skill.skillData = this;
		skill.triggerSkillEffects = triggerSkillEffects;
		skill.tickSkillEffects = tickSkillEffects;
		skill.endSkillEffects = endSkillEffects;
		skill.castPoint = castPoint;
		skill.castCooldown = castCooldown;
		skill.range = range;
		skill.Fire ();
	}
}
