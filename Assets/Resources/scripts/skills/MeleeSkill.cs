using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 //REMOVE - this is unused as of now
public class MeleeSkill : MonoBehaviour { //TODO - this is a simple skill that just has a cube hitbox in front enabled for given amount of time and that's it.
	public AbilityData skillData;
	public float range; //TODO - this is for simple hitbox scaling
	public BoxCollider hitCollider;
	public float collisionStartTime;
	public float collisionEndTime;
	public float skillEndTime;
	float timer = 0;

	void Start(){
		print ("i'm alive");
		this.transform.parent = PlayerController.instance.transform;
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y, 0.5f);
		hitCollider = GetComponent<BoxCollider> ();
		hitCollider.enabled = false;
		PlayerAnimator.instance.SwingWeapon ();
	}
	void Update(){
		timer += Time.deltaTime;
		if (timer > collisionStartTime) {
			hitCollider.enabled = true;
		}
		if (timer > collisionEndTime) {
			hitCollider.enabled = false;
		}
		if (timer > skillEndTime) {
			Destroy (this.gameObject);
		}
	}
	void OnTriggerEnter(Collider other){
		foreach (GameObject endEffect in skillData.endEffect) {
			Instantiate (endEffect, other.transform.position, Quaternion.identity);
		}
		Destroy (this.gameObject);
	}
}
