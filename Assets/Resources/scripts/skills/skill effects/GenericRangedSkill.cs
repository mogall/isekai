using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRangedSkill : Ability { //TODO - MAKE A BASE SKILL CLASS TO PUT ALL THE "INSTANTIATE EFFECT" STUFF THERE SO YOU DON'T HAVE TO REPEAT IT EVERY TIME, ALSO MAKE IT INTO  A BEHAVIOR, SO YOU GET CONTINUOUS SKILL EFFECT IN SKILL DATA
	public float maxSpeed;
	public float acceleration;
	public float range;
	public Vector3 targetPoint;
	Vector3 startingPosition;
	float speed;
	// Use this for initialization
	void Start () {
		startingPosition = transform.position;
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		targetPoint = ray.GetPoint (range);
	}
	
	// Update is called once per frame
	void Update () {
		if (speed < maxSpeed) {
			speed += acceleration + Time.deltaTime;
		}
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, targetPoint, step);
		if (Vector3.Distance (targetPoint, transform.position) < 0.1f) {
			Destroy (gameObject);
		}
	}
	void OnTriggerEnter(){ //TODO - check for damage here?
		//print("collided");
		Destroy (gameObject);
	}
	void OnDestroy(){
		foreach (GameObject effect in endEffect) {
			Instantiate (effect, transform.position, Quaternion.identity);
		}
	}
}
