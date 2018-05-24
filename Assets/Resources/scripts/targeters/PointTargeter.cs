using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PointTargeter : MonoBehaviour {  //TODO - redo targeters for skills?
	public float range;
	public LineRenderer rend;
	Vector3 targetPoint;
	public Transform targetMarker;

	void Start (){
		rend.SetPosition (0, transform.position);
		transform.parent = PlayerController.instance.abilityOriginator;
	}
	void Update(){
		rend.material.mainTextureOffset = new Vector2 (-Time.time, 0);
		rend.SetPosition (0, transform.position);
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		targetPoint = ray.GetPoint (range);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, targetPoint - transform.position, out hit, range)) {
			rend.SetPosition (1, hit.point);
			targetMarker.position = hit.point;
		}else {
			rend.SetPosition (1, targetPoint);
			targetMarker.position = targetPoint;
		}
	}
}
