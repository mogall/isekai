using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {
	public SkillData skillData;
	public SkillType skillType;
	public List<GameObject> triggerSkillEffects = new List<GameObject>();
	public List<GameObject> tickSkillEffects = new List<GameObject>();
	public List<GameObject> endSkillEffects = new List<GameObject>();
	public float castPoint;
	public float castCooldown;
	public float range;
	bool fired = false;

	public virtual void Fire(){
		fired = true;
	}
}
