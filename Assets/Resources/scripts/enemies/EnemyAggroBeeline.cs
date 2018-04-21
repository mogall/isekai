using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAggroBeeline : EnemyAggro {
	public bool stopAtRange;
	public float minRange;
	public override void Start(){
		base.Start ();
	}
	public override void Update(){
		base.Update ();
		if (!inControl) {
			return;

		}
		navAgent.SetDestination (target.position);
		float dist = navAgent.remainingDistance;
		//print (dist);
		if (stopAtRange == true && dist < minRange) {
			print ("inrange");
			navAgent.isStopped = true;
		} else {
			print ("going");
			navAgent.isStopped = false;
			navAgent.SetDestination (target.position); //both of these make the enemy move towards you, i don't know which one is less resource-intensive
		}

		//TODO - this is pretty simple, upgrade, and make it so it doesn't set destination all the time, it's too expensive
		//TODO - also try using transform.haschanged to update the path
	}
}
