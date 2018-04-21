using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csDelaunay;

public class VoronoiGenerator : MonoBehaviour {
	List<Vector2f> points = new List<Vector2f>();
	List<Edge> edges = new List<Edge>();
	public GameObject testObj;
	void Start () {
		Bounds bnds = testObj.GetComponent<MeshRenderer> ().bounds;
		Rectf bounds = new Rectf (bnds.min.x, bnds.min.y, bnds.size.x, bnds.size.y);
		for (int i = 0; i < 200; i++) {
			Vector2f curpoint = new Vector2f(Random.Range(bnds.min.x, bnds.max.x), Random.Range(bnds.min.z, bnds.max.z));

			Vector3 pos = new Vector3 (curpoint.x, 100, curpoint.y);
			RaycastHit hit;
			if (Physics.Raycast (pos, Vector3.down, out hit, 55)) {
				
				if (hit.collider.gameObject == testObj) {
					//print ("hit on target");
					points.Add (curpoint);
				}
			} else {
				//print ("missed on target");
			}
		}
		Voronoi voronoi = new Voronoi (points, bounds, 2);
		print (voronoi.Edges.Count);
		edges = voronoi.Edges;
		foreach (Edge edge in edges) {
			print ("lining");
			GameObject empty = new GameObject ();
			empty.transform.position = new Vector3 (edge.LeftSite.x, 60, edge.LeftSite.y );
			LineRenderer line = empty.AddComponent<LineRenderer> ();
			line.positionCount = 2;
			line.SetPosition (0, new Vector3 (edge.LeftSite.x, 60, edge.LeftSite.y));
			line.SetPosition (1, new Vector3 (edge.RightSite.x, 60, edge.RightSite.y));
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
