using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// THIS IS A SCRIPT FOR AN ENEMY ENTITY - THIS SCRIPT GOVERNS IDLE BEHAVIOUR, WITH ENEMY ENTITY WALKING AROUND A PREDETERMINED CIRCULAR AREA

public class EnemyIdleArea : Enemy { //TODO - UPGRADE
	Vector3 spawnPos;
	public float areaRadius;
	public float moveCooldown;
	float currentMoveCooldown = 0;
	Vector3 currentWaypoint;
	bool isWalking = false;
	// Use this for initialization
	public override void OnDrawGizmosSelected (){
		base.OnDrawGizmosSelected ();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (spawnPos, areaRadius);
	}
	public override void Start () {
		base.Start ();
		spawnPos = transform.position;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		if (inControl) {
			//print ("i'm idle");
			if (!isWalking) {
				if (currentMoveCooldown < moveCooldown) {
					currentMoveCooldown += Time.deltaTime;
				} else {
					currentWaypoint = GetNewWaypoint ();
					isWalking = true;
					currentMoveCooldown = 0;
				}
			} else {
				navAgent.SetDestination (currentWaypoint);
				float dist = navAgent.remainingDistance;
				if (dist != Mathf.Infinity && navAgent.pathStatus == NavMeshPathStatus.PathComplete && navAgent.remainingDistance == 0) {
					//print ("i arrived at a waypoint, proceeding to another one");
					isWalking = false;
				}
			}
		}
	}
	Vector3 GetNewWaypoint(){
		Vector3 randomDir = Random.insideUnitSphere * areaRadius;
		randomDir += spawnPos;
		NavMeshHit hit;
		NavMesh.SamplePosition (randomDir, out hit, areaRadius, 1);
		return hit.position;
	}
}
