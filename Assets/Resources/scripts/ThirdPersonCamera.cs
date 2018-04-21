using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {
	public Transform pivotPoint;
	public float maxRange = 3;
	public float margin = 0.1f;

	void CheckForObstacles(){ //this checks for obstacles in camera path so the camera "collides" with them and then moves closer to the player

	}
	void Update(){
		RaycastHit hit;
		//Debug.DrawRay (pivotPoint.position, transform.forward*maxRange*-1, Color.blue, 0.1f);
		if (Physics.Raycast (pivotPoint.position, transform.forward * -1, out hit, maxRange)) {
			//print (Vector3.Distance (pivotPoint.position, hit.point));
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -Vector3.Distance (pivotPoint.position, hit.point)+margin);
		} else {
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -maxRange);
		}
	}

}