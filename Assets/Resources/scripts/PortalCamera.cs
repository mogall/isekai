using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {
	Transform cam;
	public Transform portal;
	public Transform otherPortal;

	void Start(){
		cam = Camera.main.transform;
	}
	void LateUpdate(){
		Vector3 playerOffsetFromPortal = cam.position - otherPortal.position;
		transform.position = portal.position + playerOffsetFromPortal;

		float angularDiff = Quaternion.Angle (portal.rotation, otherPortal.rotation);
		Quaternion rotDiff = Quaternion.AngleAxis (angularDiff, Vector3.up);
		Vector3 camDir = rotDiff * cam.forward;
		transform.rotation = Quaternion.LookRotation (camDir, Vector3.up);
	}
}
