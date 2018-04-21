using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTest1 : MonoBehaviour {
	public GameObject testPlot;
	public GameObject testWall;
	public int plotSizeX, plotSizeY;
	public int minPlotPartSize = 3;
	bool isSplit;
	List<PlotPart> finalPlots = new List<PlotPart>(); //this is the list of final plots - the ones that will become rooms, the bottommost ones in the tree
	bool[,] isDoor;

	void Start(){
		isDoor = new bool[plotSizeX+1, plotSizeY+1];
		PlotPart parentPlotPart = new PlotPart (0, 0, plotSizeX, plotSizeY, minPlotPartSize); //this created the topmost plot in the tree that's the size of the house
		isSplit = true;
		//List<PlotPart> workingPlotParts = new List<PlotPart> ();
		//workingPlotParts.Add (parentPlotPart);
		while (isSplit) {
			isSplit = false;
			SplitPlot (parentPlotPart);
			/*foreach (PlotPart part in workingPlotParts) {
				if (part.width > minPlotPartSize || part.height > minPlotPartSize || Random.value > 0.25f) {
					if (part.Split()) {
						workingPlotParts.Add (part.leftChild);
						workingPlotParts.Add (part.rightChild);
						isSplit = true;
					}
				}
			}*/
		}
		GenerateTestPlots ();
		MakeDoorsAndWalls ();
	}
	void Update(){ //TODO - delete this, TEMPORARY ONLY
		if (Input.GetKey (KeyCode.R)) {
			Restart ();
		}
	}
	public void Restart(){ //TODO - delete this, TEMPORARY ONLY
		finalPlots.Clear();
		isDoor = new bool[plotSizeX+1, plotSizeY+1];
		foreach (Transform child in this.transform) {
			Destroy (child.gameObject);
		}
		PlotPart parentPlotPart = new PlotPart (0, 0, plotSizeX, plotSizeY, minPlotPartSize);
		isSplit = true;
		while (isSplit) {
			isSplit = false;
			SplitPlot (parentPlotPart);
		}
		GenerateTestPlots ();
		MakeDoorsAndWalls ();

	}
	void SplitPlot(PlotPart plot){ //this starts with the first plot created and splits it, then callls itself on the split plots that it created and so on, until the limit
		finalPlots.Add (plot);
		if (plot.width > minPlotPartSize || plot.height > minPlotPartSize || Random.value > 0.25f) {
			if (plot.Split ()) {
				SplitPlot (plot.leftChild);
				SplitPlot (plot.rightChild);
				isSplit = true;
			}
		}
	}
	void GenerateTestPlots(){ //generates test room plots from a simple blender prefab
		foreach (PlotPart plot in finalPlots) {
			if (plot.leftChild == null && plot.rightChild == null) {
				GameObject thisPlot = Instantiate(testPlot, transform.position, Quaternion.Euler(-90,0,0));
				thisPlot.transform.parent = this.transform;
				thisPlot.transform.localPosition = new Vector3 (-plot.x, 0, -plot.y);
				thisPlot.transform.localScale = new Vector3 (plot.width, plot.height, thisPlot.transform.localScale.z);
				plot.isRoom = true;
			}

		}
	}
	void MakeDoorsAndWalls(){
		isDoor [0, Random.Range (0, plotSizeY)] = true;
		foreach (PlotPart plot in finalPlots) {
			if (plot.partner != null && !plot.hasDoors) {
				Vector3 position;
				if (plot.x == plot.partner.x) {
					if (plot.y < plot.partner.y) {
						int rand = Random.Range (plot.x + 1, plot.x + plot.width);
						position = new Vector3 (-rand, 0.2f, -plot.y - plot.height + 0.5f);
						isDoor [rand, plot.y + plot.height] = true;
						//doors.Add(new Vector3(-plot.x, 0, -plot.y-plot.height));
					} else {
						int rand = Random.Range (plot.x + 1, plot.x + plot.width);
						position = new Vector3 (-rand, 0.2f, -plot.y + 0.5f);
						isDoor [rand, plot.y] = true;
						//doors.Add(new Vector3(-plot.x, 0, -plot.y));
					}
				} else {
					if (plot.x < plot.partner.x) {
						int rand = Random.Range (plot.y + 1, plot.y + plot.height);
						position = new Vector3 (-plot.x - plot.width + 0.5f, 0.2f, -rand);
						isDoor [plot.x + plot.width, rand] = true;
						//doors.Add(new Vector3(-plot.x-plot.width , 0, -plot.y));
					} else {
						int rand = Random.Range (plot.y + 1, plot.y + plot.height);
						position = new Vector3 (-plot.x + 0.5f, 0.2f, -rand);
						isDoor [plot.x, rand] = true;
						//doors.Add(new Vector3(-plot.x, 0, -plot.y));
					}
				}
				GameObject door = Instantiate (testPlot, transform.position, Quaternion.Euler (-90, 0, 0));
				door.transform.parent = this.transform; 
				door.transform.localPosition = position;
				door.GetComponent<MeshRenderer> ().material.color = Color.red;
				plot.hasDoors = true;
				plot.partner.hasDoors = true;
			}
			if (plot.isRoom) {
				Vector3 position;
				Vector3 rotation = new Vector3 (-90,0,0);
				GameObject wall;
				for (int i = plot.x; i < plot.x+plot.width; i++) {
					if (!isDoor[i,plot.y]) {
						position = new Vector3 (-i, 0, -plot.y);
						wall = Instantiate (testWall, transform.position, Quaternion.identity);
						wall.transform.parent = this.transform;
						wall.transform.localPosition = position;
						wall.transform.rotation = Quaternion.Euler (rotation);
					}
					if (!isDoor[i,plot.y+plot.height]) {
						position = new Vector3 (-i,0, -plot.y-plot.height);
						wall = Instantiate (testWall, transform.position, Quaternion.identity);
						wall.transform.parent = this.transform;
						wall.transform.localPosition = position;
						wall.transform.rotation = Quaternion.Euler (rotation);
					}

				}
				rotation = new Vector3 (-90,90,0);
				for (int i = plot.y; i < plot.y+plot.height; i++) {
					if (!isDoor[plot.x,i]) {
						position = new Vector3 (-plot.x, 0, -i-1);
						wall = Instantiate (testWall, transform.position, Quaternion.identity);
						wall.transform.parent = this.transform;
						wall.transform.localPosition = position;
						wall.transform.rotation = Quaternion.Euler (rotation);
					}
					if (!isDoor[plot.x+plot.width,i]) {
						position = new Vector3 (-plot.x-plot.width,0, -i-1);
						wall = Instantiate (testWall, transform.position, Quaternion.identity);
						wall.transform.parent = this.transform;
						wall.transform.localPosition = position;
						wall.transform.rotation = Quaternion.Euler (rotation);
					}


				}
				//TEST - put interactable objects into a room here - IT'S ACTUALLY SHIT, TODO - CHANGE IT ALL SO A ROOM IS A FINAL FORM THAT'S SEPARATE AND MAKE IT ALL ON TILES OR SOMETHING
				for (int i = plot.x+1; i < plot.x+plot.width; i++) {
					GameObject obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
					obj.transform.localScale *= 0.5f;
					obj.transform.parent = this.transform;
					obj.transform.localPosition = new Vector3 (-i, 0, -plot.y-1);
				}
				for (int i = plot.y; i < plot.y + plot.height; i++) {

				}
			}
		}
	}
	class PlotPart{ //a class for a plot that splits itself into two - ultimately leading to the plot part that serves as a room base... or so it says
		public int x, y, width, height, plotMinSize; //plot part position and size
		public PlotPart leftChild, rightChild; //plot children of this plot
		public PlotPart partner;
		public bool hasDoors = false;
		public bool isRoom = false;


		public PlotPart(int _x, int _y, int _width, int _height, int _plotMinSize){ //constructor
			x = _x;
			y = _y;
			width = _width;
			height = _height;
			plotMinSize = _plotMinSize;
		}
		public bool Split(){ //split function, to split the plot and generate two smaller ones out of it - if new plots can be created according to minplotsize rule
			if (leftChild != null || rightChild != null) {
				return false;
			}
			bool splitH = Random.value > 0.5f ? true: false;
			if (width > height && width / height >= 1.25f) {
				splitH = false;
			} else if (height > width && height / width >= 1.25f) {
				splitH = true;
			}
			int max = (splitH ? height : width) - plotMinSize;
			if (max <= plotMinSize) {
				return false;
			}
			int split = Random.Range (plotMinSize, max + 1);
			if (splitH) {
				leftChild = new PlotPart (x, y, width, split, plotMinSize);
				rightChild = new PlotPart (x, y + split, width, height - split, plotMinSize);
			} else {
				leftChild = new PlotPart (x, y, split, height, plotMinSize);
				rightChild = new PlotPart (x + split, y, width - split, height, plotMinSize);
			}
			leftChild.partner = rightChild;
			rightChild.partner = leftChild;
			return true;
		}
	}
}
//TODO
//FOR DOORS - NEED TO CHECK PARTNERS
//NEED SOME SORT OF SWITCH THAT TELLS THAT ONE OF THE PARTNERS ALREADY GENERATED THE DOORWAY
//IF X OR Y IS THE SAME IN PARTNERS, THAT DETERMINES THE AXIS OF THE DOOR
//THEN CHECK WHICH EDGE IS THE COMMON ONE
//THEN CHECK WHERE YOU CAAN PUT DOORS (PROBABLY BOTH EDGES WILL BE THE SAME LENGTH
//THEN PUT DOOR AT RANDOM SPOT