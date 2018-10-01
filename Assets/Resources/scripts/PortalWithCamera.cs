using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalWithCamera : MonoBehaviour {
	public Camera portalCam;
	PortalCamera portalCamController; //use this to change portalcam vars around
	public MeshRenderer portalViewPlane;
	public Transform portalExit;
	// Use this for initialization
	void Start () {
		if (portalCam.targetTexture != null) {
			portalCam.targetTexture.Release ();
		}
		portalCam.targetTexture = new RenderTexture (Screen.width, Screen.height, 24);
		portalViewPlane.material.mainTexture = portalCam.targetTexture;
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			TeleportPlayer ();
		}
	}
	void TeleportPlayer(){
		print ("teleporting");
		PlayerController.instance.transform.position = portalExit.transform.position;
	}

}
