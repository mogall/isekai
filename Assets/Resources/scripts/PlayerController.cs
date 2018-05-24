using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	//public enum CameraMode {MOVEMENT, UI};
	public CameraMode cameraMode;
	public PlayerMovementState playerMovementState;
	public PlayerActionState playerActionState;

	public float walkSpeed = 2;
	public float runSpeed = 6;
	public float gravity = 12;
	public float jumpHeight = 1;
	[Range(0,1)]
	public float airControlPercent;
	[Range(0,1)]
	public float backwardsMoveReduction;

	public float turnSmoothTime = 0.2f; //TODO - apply smoothing to turns
	public float turnRate = 1f;
	public float pitch;
	public Vector2 pitchMinMax = new Vector2 (-40, 85);
	public GameObject cameraBoom;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	public float currentSpeed;
	bool walkingForward = true;

	float velocityY;

	float mouseSensitivity; //TODO - use this later for player options
	float interactRayLength = 10f;

	Interactable focusInteractable;
	bool interacting = false;
	float interactTime = 0;

	public Container container;
	public bool openingContainter = false;

	Transform cameraT;
	CharacterController controller;
	PlayerAnimator animator;
	public Transform abilityOriginator;

	  //TODO - make this use some sort of a global var for skillbar size(not that it would get dynamically changed or something)

	public static PlayerController instance;
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
	void Start () {
		SetCameraMode (CameraMode.MOVEMENT);
		cameraT = Camera.main.transform;
		controller = GetComponent<CharacterController> ();
		animator = GetComponent<PlayerAnimator> ();
	}

	void Update () {
		DetectObjectInFront (); //TODO - this is super basic, upgrade
		if(Input.GetButtonDown("Interact")){
			if (focusInteractable != null && !interacting) {
				print ("starting interaction");
				interacting = true;
				//focusInteractable.Interact ();
			} else {
				ClearInteractionData ();
			}
				
		}
		if (interacting) {
			if (focusInteractable == null) {
				ClearInteractionData ();
			} else {
				if (interactTime >= focusInteractable.interactionDelay) {
					focusInteractable.Interact ();
					ClearInteractionData ();
					print ("interacting with object");
				} else {
					interactTime += Time.deltaTime;
					PlayerGUI.instance.SetProgressBarFillAmount (focusInteractable.interactionDelay, interactTime, true);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			ToggleEscape ();
		}
		if (Input.GetMouseButtonDown (0)) {
			//print ("dupa");
			//animator.SwingWeapon ();
		}
		if (cameraMode == CameraMode.MOVEMENT && playerMovementState != PlayerMovementState.DASHING) {
			Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized;
			bool running = Input.GetKey (KeyCode.LeftShift);
			Rotate ();
			Move (input, running);
		}


		if (Input.GetKeyDown (KeyCode.Space)) {
			Jump ();
		}
	}
	void DetectObjectInFront(){  //TODO - this is pretty shit, only detects if the object is small because of the hit transform possition thing(center of large objects is too far away). fix with colliders later?
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		Debug.DrawRay (ray.origin, ray.direction*interactRayLength, Color.yellow, 0.1f);
		//Debug.DrawRay (cameraBoom.transform.position, cameraBoom.transform.forward *1.5f, Color.yellow, 0.1f);
		RaycastHit hit;
		if (Physics.Raycast (cameraBoom.transform.position, cameraBoom.transform.forward, out hit, interactRayLength)) { 
			Interactable interactable = hit.collider.GetComponent<Interactable> ();
			if (interactable != null && (hit.transform.position - transform.position).magnitude < interactable.interactionRadius) { //TODO - when you check, make a var that takes an interactable and make it so if it'ss the same one, it doesn't do stuff again and again(like highlighting) to save cycles
				if (focusInteractable == null) {
					focusInteractable = interactable;
					focusInteractable.EnableHighlight ();
				} else if (focusInteractable != interactable) {
					focusInteractable.DisableHighlight ();
					focusInteractable = interactable;
					focusInteractable.EnableHighlight ();
				}
				//interactable.Interact();
			} else if(focusInteractable != null){
				focusInteractable.DisableHighlight ();
				focusInteractable = null;
			}

		}
	}
	void Rotate(){
		Vector2 lookInput = new Vector2 (Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); 
		pitch -= lookInput.y * turnRate;
		pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);
		transform.Rotate (0, lookInput.x*turnRate, 0);
		cameraBoom.transform.localEulerAngles = new Vector3 (pitch, 0, 0);
	}

	void Move(Vector2 inputDir, bool running) {
		/*if (inputDir != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}*/
		Vector3 localMoveVector = transform.InverseTransformDirection (controller.velocity);
		float targetSpeed = 0;
		if (System.Math.Round(localMoveVector.z,2) >= 0) {  //are we walking forward or backward?
			walkingForward = true;
			targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		}else if(System.Math.Round(localMoveVector.z,2) < 0){
			walkingForward = false;
			targetSpeed = ((running) ? runSpeed * backwardsMoveReduction : walkSpeed * backwardsMoveReduction) * inputDir.magnitude;
		}

		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
		velocityY += Time.deltaTime * -gravity;
		//Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
		//Vector3 dir = new Vector3 (inputDir.x, velocityY, inputDir.y );
		Vector3 dir = transform.TransformDirection(new Vector3(inputDir.x * currentSpeed, velocityY, inputDir.y * currentSpeed));
		Debug.DrawRay (transform.position, dir, Color.red, 0.1f);
		
		controller.Move (dir * Time.deltaTime);  //TODO - APPLY SLOWDOWN, ALSO WHEN JUMPING
		currentSpeed = new Vector2 (controller.velocity.x, controller.velocity.z).magnitude;
		if (controller.isGrounded) {
			velocityY = 0;
		}
	}

	void Jump() {
		if (controller.isGrounded) {
			float jumpVelocity = Mathf.Sqrt (-2 * -gravity * jumpHeight);
			velocityY = jumpVelocity;
		}
	}

	float GetModifiedSmoothTime(float smoothTime) {
		if (controller.isGrounded) {
			return smoothTime;
		}

		if (airControlPercent == 0) {
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}
	public void SetCameraMode(CameraMode mode){
		if (mode == CameraMode.MOVEMENT) {
			print ("switched to movement mode");
			Cursor.lockState = CursorLockMode.Locked;
			cameraMode = CameraMode.MOVEMENT;
		}else if (mode == CameraMode.UI) {
			print ("switched to UI mode");
			Cursor.lockState = CursorLockMode.None;
			cameraMode = CameraMode.UI;
		}		
	}
	public void FlipCameraMode(){
		if (cameraMode == CameraMode.MOVEMENT) {
			SetCameraMode (CameraMode.UI);
		}else if (cameraMode == CameraMode.UI) {
			SetCameraMode (CameraMode.MOVEMENT);
		}		
	}
	void ToggleEscape(){
		if (cameraMode == CameraMode.MOVEMENT) {
			SetCameraMode (CameraMode.UI);
		}else if (cameraMode == CameraMode.UI) {
			SetCameraMode (CameraMode.MOVEMENT);
			PlayerGUI.instance.ClearAllGUIPanels ();
		}
	}
	void ClearInteractionData(){
		PlayerGUI.instance.SetProgressBarFillAmount (0, 0, false);
		interacting = false;
		interactTime = 0;
	}
	public void TeleportToCursor(float range){ //should these be here or moved to individual skill functionality?
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		Vector3 targetPoint = ray.GetPoint (range);
		RaycastHit hit;
		if (Physics.Raycast (transform.position+Vector3.up, targetPoint - transform.position, out hit, range)) { //FIX - this vector3up is temporary fix. transPos fires from feet so it doesn't really work, fix this
			targetPoint = hit.point;
			Debug.DrawLine (transform.position, hit.point, Color.red, 1f, false);
		}
		TeleportToLocation (targetPoint);
	}
	public void TeleportToLocation(Vector3 location){ //is this really needed? looks like a glorified transform.position thing
		//TODO - set movement/action state flags properly here?
		transform.position = location;
	}
	public void Dash(Vector2 dir, float speedMult){
		currentSpeed = runSpeed * speedMult;
		Move (dir, false);
	}
}