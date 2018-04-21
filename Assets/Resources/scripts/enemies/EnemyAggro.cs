using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAggro : MonoBehaviour, IEnemyAggroBehaviour {
	public bool inControl = false;
	public Transform target;
	public NavMeshAgent navAgent;
	public IEnemyIdleBehaviour enemyIdleBehaviour;

	public virtual void Start(){
		navAgent = GetComponent<NavMeshAgent> ();
		target = PlayerController.instance.gameObject.transform;
		enemyIdleBehaviour = GetComponent<IEnemyIdleBehaviour> ();
	}
	public virtual void Update (){
		//put generic aggro code here
	}
	public virtual void TakeControl(){
		inControl = true;
	}

}
