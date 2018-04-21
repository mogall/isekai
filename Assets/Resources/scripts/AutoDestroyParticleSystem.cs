using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestroyParticleSystem : MonoBehaviour {
	public ParticleSystem ps;

	void Update(){
		if (!ps.IsAlive()) {
			Destroy (this.gameObject);
		}
	}
}
