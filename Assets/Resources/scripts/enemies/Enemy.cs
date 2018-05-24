using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ITakeDamage, IEnemyIdleBehaviour { //TODO DONOW? - itakedamage doesn't really make much sense here now - split to master enemy controller class and use it to controll movement and aggro classes?
	public int maxHP = 1;
	public int currentHP = 1;
	public float speed = 0;
	public float maxAngularSpeed = 120;
	public float maxSpeed = 1;
	public float acceleration = 1;

	public Transform target;
	public NavMeshAgent navAgent;
	public IEnemyAggroBehaviour enemyAggroBehaviour;
	public bool inControl = true;
	public float hearingRange = 5; //TODO - make sure that detection doesn't work through walls etc?
	public float hearingStrength = 1;
	public float horizontalFOV = 60; //TODO - rework all detection, fov doesn't exist yet, rework sound - sounds make radius chcecks if anyone that hears it is nearby, every enemy has "suspicion sound bar" that fills - if it fills, they go investigate
	public float verticalFOV = 50;
	public float detectionTick;
	public virtual void OnDrawGizmosSelected (){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, hearingRange);
	}
	public virtual void Start(){
		navAgent = GetComponent<NavMeshAgent> ();
		enemyAggroBehaviour = GetComponent<IEnemyAggroBehaviour> ();
		navAgent.speed = maxSpeed;
		navAgent.angularSpeed = maxAngularSpeed;
		navAgent.acceleration = acceleration;
		target = PlayerController.instance.gameObject.transform;
	}

	public virtual void OnDeath(){
		Destroy (gameObject);
	}
	public virtual void TakeDamage(int damage){
		print (gameObject.name + " took damage");
		currentHP -= damage;
		if (currentHP < 1) {
			OnDeath ();
		}
	}
	public virtual void TakeControl (){
		inControl = true;
	}
	public virtual void HearSound(float soundpercent, Vector3 soundOrigin){ //TODO - this is not ussed yet, think about enemy sound detection (and visual too)
		float soundStrength = soundpercent * hearingStrength;
	}
	public virtual void Update(){
		if (Vector3.Distance(transform.position, target.position) < hearingRange) { //TODO - change this to something less intensive - you'll have 100 enemies and will kill performance with constant checks
			inControl = false;														// also, change it to sound and visual detection, 
			enemyAggroBehaviour.TakeControl ();
			//navAgent.SetDestination (target.position); //both of these make the enemy move towards you, i don't know which one is less resource-intensive
			//navAgent.destination = target.position; 
			return;
		}
	}


}
