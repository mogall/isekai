using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
	Animator animator;
	PlayerController playercontroller;
	float speedPercent;
	const float moveAnimSmoothTime = .1f;


	public static PlayerAnimator instance;
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
	void Start () {
		animator = GetComponent<Animator> ();
		playercontroller = GetComponent<PlayerController> ();
	}
	void Update () {
		speedPercent = playercontroller.currentSpeed / playercontroller.runSpeed;
		animator.SetFloat ("speedPercent", speedPercent, moveAnimSmoothTime, Time.deltaTime);
	}
	public void SwingWeapon(){
		animator.SetTrigger ("attack");
	}
}
