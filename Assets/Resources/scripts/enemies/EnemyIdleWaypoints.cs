using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//TODO - when spawning this enemy procedurally, make sure to have a method of spawning the waypoints too - currently they're just premade

public class EnemyIdleWaypoints : Enemy {
	List<Vector3> waypoints = new List<Vector3>();
	int currentWaypointID;
	public float moveCooldown;
	float currentMoveCooldown = 0;
	bool isWalking = false;
	public bool pingPongMove = false; //if yes, it'll move 0-1-2-3-2-1-0, if not it'll move 0-1-2-3-0-1-2-3
	bool pingPongReverse = false;


	public override void Start () {
		base.Start ();
		foreach (Transform child in transform) {
			waypoints.Add (child.position);
			Destroy (child.gameObject);
		}
		currentWaypointID = -1;
	}

	public override void Update(){
		base.Update ();
		if (inControl) {
			if (!isWalking) {
				if (currentMoveCooldown < moveCooldown) {
					currentMoveCooldown += Time.deltaTime;
				} else {
					if (pingPongMove) {
						if (!pingPongReverse) {
							if (currentWaypointID == waypoints.Count - 1) {
								pingPongReverse = true;
								currentWaypointID--;
							} else {
								currentWaypointID++;
							}
						} else {
							if (currentWaypointID == 0) {
								pingPongReverse = false;
								currentWaypointID++;
							} else {
								currentWaypointID--;
							}
						}
					} else {
						if (currentWaypointID == waypoints.Count - 1) {
							currentWaypointID = 0;
						} else {
							currentWaypointID++;
						}
					}
					isWalking = true;
					currentMoveCooldown = 0;
				}
			} else { //iswalking == true
				navAgent.SetDestination(waypoints[currentWaypointID]);
				float dist = navAgent.remainingDistance;
				if (dist != Mathf.Infinity && navAgent.pathStatus == NavMeshPathStatus.PathComplete && navAgent.remainingDistance == 0) {
					isWalking = false;
				}
			}
		}
	}

}
