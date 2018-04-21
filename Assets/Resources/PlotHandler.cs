using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotHandler : MonoBehaviour {
	public List<GameObject> plots = new List<GameObject>();
	// Use this for initialization
	void Start () {
		plots.AddRange(GameObject.FindGameObjectsWithTag("housebase"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
