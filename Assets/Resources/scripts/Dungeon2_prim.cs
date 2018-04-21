using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon2_prim : MonoBehaviour {
	public Vector2Int size;
	int[,] weights;
	bool[,] openTile;
	bool[,] floor;
	List<Vector2Int> workingTiles = new List<Vector2Int> ();

	public int maxRoomNumber = 10;
	public int minRoomSize = 5;
	public int maxRoomSize = 10;
	public bool roomsCanOverlap = false;
	List<Rect> roomList = new List<Rect> ();   //TODO - change this into a room struct later, rect is in floats and it causes a lot of unneeded casting all over the place

	[Header("TEMP - REMOVE LATER")] //TODO - remove this later, temporary
	public GameObject floorObj;
	void Start () {
		weights = new int[size.x,size.y];
		floor = new bool[size.x,size.y];

		WeightTheGrid ();
		GenerateRooms ();
		GenerateMaze ();
		MakeRoomEntrances ();
		GenerateMockup ();
	}
	void WeightTheGrid(){
		for (int i = 0; i < size.y; i++) {
			for (int j = 0; j < size.x; j++) {
				weights [i, j] = Random.Range (0, 100);
			}
		}
	}
	void GenerateMaze(){
		floor [0, 0] = true;
		AddSurroundingTilesToWorkngList (new Vector2Int (0, 0));
		while (workingTiles.Count > 0) {
			Vector2Int lowestTile = GetLowestWorkingTileWeight ();
			bool noGo = IsThereMoreThanOneFloorAround (lowestTile);
			if (noGo) {
				workingTiles.Remove (lowestTile);
				//AddSurroundingTilesToWorkngList (lowestTile);
			} else {
				floor [lowestTile.x, lowestTile.y] = true;
				workingTiles.Remove (lowestTile);
				AddSurroundingTilesToWorkngList (lowestTile);
			}
		}
	}
	bool IsThereMoreThanOneFloorAround(Vector2Int v){
		int floorNum = 0;
		if (v.x > 0 && floor [v.x - 1, v.y] == true) {
			floorNum++;
		}
		if (v.x < size.x-1 && floor [v.x + 1, v.y] == true ) {
			floorNum++;
		}
		if (v.y > 0 && floor [v.x, v.y - 1] == true) {
			floorNum++;
		}
		if (v.y < size.y-1 && floor [v.x, v.y + 1] == true) {
			floorNum++;
		}
		if (floorNum > 1) {
			return true;
		} else {
			return false;
		}
	}
	void AddSurroundingTilesToWorkngList(Vector2Int v){
		if (v.x > 0 && floor [v.x - 1, v.y] != true && workingTiles.Contains (new Vector2Int(v.x - 1, v.y)) != true) {
			workingTiles.Add (new Vector2Int(v.x-1,v.y));
		}
		if (v.x < size.x-1 && floor [v.x + 1, v.y] != true && workingTiles.Contains (new Vector2Int(v.x + 1, v.y)) != true) {
			workingTiles.Add (new Vector2Int(v.x+1,v.y));
		}
		if (v.y > 0 && floor [v.x, v.y - 1] != true && workingTiles.Contains (new Vector2Int(v.x, v.y - 1)) != true) {
			workingTiles.Add (new Vector2Int(v.x,v.y-1));
		}
		if (v.y < size.y-1 && floor [v.x, v.y + 1] != true && workingTiles.Contains (new Vector2Int(v.x, v.y + 1)) != true) {
			workingTiles.Add (new Vector2Int(v.x,v.y+1));
		}
	}

	Vector2Int GetLowestWorkingTileWeight(){
		Vector2Int lowestTile = new Vector2Int(0,0);
		int lowestValue = 100;
		foreach (Vector2Int tile in workingTiles) {
			if (weights[tile.x,tile.y] < lowestValue) {
				lowestTile = tile;
				lowestValue = weights [tile.x, tile.y];
			}
		}
		return lowestTile;
	}

	void GenerateRooms(){
		int currentRoomNumber = 0;
		for (int i = 0; i < maxRoomNumber * 5; i++) {  //this goes for 5 times the number of rooms to ensure there's max number of rooms, but doesn't really 100% guarantee it
			Rect room = MakeRoomRect();
			if (roomsCanOverlap) {
				PutRoomOnGrid (room);
				currentRoomNumber++;
			} else {
				bool overlap = false;
				foreach (Rect otherRoom in roomList) { //TODO - this doesn't work for some raisin, fix pls
					if (room.x < otherRoom.x + otherRoom.width && otherRoom.x < (room.x + room.width) && room.y < otherRoom.y + otherRoom.height && otherRoom.y < (room.y + room.height)) {
						overlap = true;
					}
				}
				if (!overlap) { 
					PutRoomOnGrid (room);
					currentRoomNumber++;
				}
			}
			roomList.Add (room);
			if (currentRoomNumber >= maxRoomNumber) {
				break;
			}
		}
	}
	Rect MakeRoomRect(){
		int w = UnityEngine.Random.Range (minRoomSize, maxRoomSize + 1);
		int h = UnityEngine.Random.Range (minRoomSize, maxRoomSize + 1);
		int x = UnityEngine.Random.Range (1, size.x - w);
		int y = UnityEngine.Random.Range (1, size.y - h);
		return new Rect (x,y,w,h);
	}
	void PutRoomOnGrid(Rect rect){
		for (int j = (int)rect.y; j < rect.y + rect.height+1; j++) {
			for (int i = (int)rect.x; i < rect.x + rect.width+1; i++) {
				floor [i, j] = true;
			}
		}
	}
	void MakeRoomEntrances(){
		foreach (Rect room in roomList) {
			int y = (int)(room.y + (room.height / 2));
			bool finished = false;
			if (room.x > size.x / 2) {
				int currentX = (int)room.x-1;
				while (!finished) {
					floor [currentX, y] = true;
					currentX--;
					if (floor[currentX,y] == true) {
						finished = true;
					}
				}

			} else {
				int currentX = (int)(room.x+room.width+1);
				while (!finished) {
					floor [currentX, y] = true;
					currentX++;
					if (floor[currentX,y] == true) {
						finished = true;
					}
				}
			}
		}
	}
	//TODO - delete later, temporary mockup
	void GenerateMockup(){
		for (int i = 0; i < size.y; i++) {
			for (int j = 0; j < size.x; j++) {
				if (floor[i,j]) {
					GameObject curFloor = Instantiate (floorObj, transform.position, Quaternion.Euler(90,0,0));
					curFloor.transform.parent = this.transform;
					curFloor.transform.localPosition = new Vector3 (i,0,j);
				}
			}
		}
	}
}
