using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSingleTargetDamageTest : MonoBehaviour {
	public int damage;
	float lifeTime = 0;
	public float maxLifeTime;
	void Start(){
		//do graphics stuff here
	}
	void Update(){
		lifeTime += Time.deltaTime;
		if (lifeTime > maxLifeTime) {
			Destroy (gameObject);
		}
	}
	void OnTriggerEnter(Collider coll){ //TODO - this fires more than one time because of overlapping coliders on stuff, how to fix?
		//do damage stuff here
		ITakeDamage takeDamage = coll.gameObject.GetComponent<ITakeDamage> ();
		if (takeDamage != null) {
			takeDamage.TakeDamage (damage);
		}
	}
}
