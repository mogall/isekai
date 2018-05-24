using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GenericChanneledRaySkill : Ability {
	public LineRenderer rend;
	public Vector3 targetPoint;
	public float range;
	public float damagePeriod = 0;
	float damageTick = 0;
	public int damage;

	void Start(){
		PlayerController.instance.playerActionState = PlayerActionState.CHANNELING;
		rend.SetPosition (0, transform.position);
		transform.parent = PlayerController.instance.abilityOriginator;
		damageTick = damagePeriod;
	}

	void Update (){
		damageTick += Time.deltaTime;
		if (Input.GetMouseButton (0)) {
			rend.SetPosition (0, transform.position);
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			targetPoint = ray.GetPoint (range);
			RaycastHit hit;
			if (Physics.Raycast (transform.position, targetPoint - transform.position, out hit, range)) {
				rend.SetPosition (1, hit.point);
				foreach (GameObject effect in tickEffect) {
					Instantiate (effect, hit.point, Quaternion.identity);
				}
				ITakeDamage takeDamage = hit.transform.GetComponent<ITakeDamage> ();
				if (takeDamage != null && damageTick >= damagePeriod) {
					print ("hitting enemy!");
					takeDamage.TakeDamage (damage);
					damageTick = 0;
				}
			} else {
				rend.SetPosition (1, targetPoint);
			}


		}
		if (Input.GetMouseButtonUp (0)) {
			PlayerController.instance.playerActionState = PlayerActionState.NOTHING;
			Destroy (this.gameObject);
		}

	}
}
