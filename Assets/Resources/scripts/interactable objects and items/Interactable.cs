using UnityEngine;
using cakeslice;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour {
	public float interactionRadius = 2f;
	private Outline outline;
	public float interactionDelay = 0;

	void OnDrawGizmosSelected (){
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, interactionRadius);
	}
	public virtual void Start(){
		outline = GetComponent<Outline> ();
		DisableHighlight ();
	}
	public void EnableHighlight(){
		outline.enabled = true;
	}
	public void DisableHighlight(){
		outline.enabled = false;
	}
	public virtual void Interact(){
		//Debug.Log ("Interacted with" + transform.name);
	}
}
