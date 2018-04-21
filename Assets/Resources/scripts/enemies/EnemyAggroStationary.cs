using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroStationary : EnemyAggro {
	public float lookSpeed;
	// Use this for initialization
	public override void Start(){
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Update(){
		base.Update ();
		if (!inControl) {
			return;
		}
		Vector3 pos = target.position - transform.position;
		Quaternion rot = Quaternion.LookRotation (pos);
		transform.rotation = Quaternion.Lerp (transform.rotation, rot, lookSpeed * Time.deltaTime);
	}
}
