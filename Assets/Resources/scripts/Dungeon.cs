using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dungeon : MonoBehaviour {
	public int sizeX = 64;
	public int sizeY = 64;
	public int maxRoomNumber = 15;
	public int minRoomSize = 5;
	public int maxRoomSize = 10;
	public bool roomsCanOverlap = false;
	public int randomRoomConnectionsNumber = 1;
	public int randomRoomSpurs = 3;

	int[,] dungeonArray;  //0 - solid, 1 - wall, 2 - walkable?
	List<Room> roomList;
	List<Corridor> corridorList;

	//TODO - TEMPORARY SHIT, REMOVE LATER
	[Header("TEMP - REMOVE LATER")]
	public GameObject floor;

	void Start(){
		
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.G)) {
			DestroyDungeonMockup ();
			GenerateDungeon ();
			MakeDungeonMockup ();
		}
	}
	void GenerateDungeon(){
		dungeonArray = new int[sizeX,sizeY];
		roomList = new List<Room> ();
		corridorList = new List<Corridor> ();
		for (int i = 0; i < maxRoomNumber * 5; i++) {  //this goes for 5 times the number of rooms to ensure there's max number of rooms, but doesn't really 100% guarantee it
			Room room = GenerateRoom ();
			if (roomsCanOverlap) {
				roomList.Add (room);
			} else {
				if (!IsRoomOverlapping(room)) { //is this going to break when the list is empty?
					roomList.Add (room);
				}
			}
			if (roomList.Count >= maxRoomNumber) {
				break;
			}
		}
		for (int i = 0; i < roomList.Count-1; i++) {
			ConnectRooms (roomList [i], roomList [i + 1]);
		}
		//TODO - random connections here
		//TODO - spurs here
	}
	void MakeDungeonMockup(){ //TODO - this is temporary shit, delete later
		foreach (Room room in roomList) {
			for (int i = 0; i < room.width; i++) {
				for (int j = 0; j < room.height; j++) {
					GameObject curFloor = Instantiate (floor, transform.position, Quaternion.Euler(90,0,0));
					curFloor.transform.parent = this.transform;
					curFloor.transform.localPosition = new Vector3 (room.x + i,0,room.y + j);
				}
			}
		}
		foreach (Corridor corridor in corridorList) {
			//print ("corridor starts at " + corridor.start.x + ", " + corridor.start.y + ", ends at " + corridor.end.x + ", " + corridor.end.y);
			if (corridor.start.x == corridor.end.x) {
				//print ("cor length is :" + (corridor.end.y - corridor.start.y + 1));
				for (int i = 0; i < corridor.end.y - corridor.start.y+1; i++) {
					GameObject curFloor = Instantiate (floor, transform.position, Quaternion.Euler (90, 0, 0));
					curFloor.transform.parent = this.transform;
					curFloor.transform.localPosition = new Vector3 (corridor.start.x, 0, corridor.start.y + i);

				}
			} else {
				for (int i = 0; i < corridor.end.x - corridor.start.x+1; i++) {
					GameObject curFloor = Instantiate (floor, transform.position, Quaternion.Euler (90, 0, 0));
					curFloor.transform.parent = this.transform;
					curFloor.transform.localPosition = new Vector3 (corridor.start.x + i, 0, corridor.start.y);

				}
			}
		}
	}
	void DestroyDungeonMockup(){
		foreach (Transform item in transform) {
			Destroy (item.gameObject);
		}
		dungeonArray = new int[sizeX,sizeY];
		roomList = new List<Room> ();
	}
	Room GenerateRoom(){
		int w = UnityEngine.Random.Range (minRoomSize, maxRoomSize + 1);
		int h = UnityEngine.Random.Range (minRoomSize, maxRoomSize + 1);
		int x = UnityEngine.Random.Range (1, sizeX - w - 1);
		int y = UnityEngine.Random.Range (1, sizeY - h - 1);
		return new Room (x,y,w,h);
	}
	void GenerateCorridor(int x1,int y1,int x2,int y2, bool topConn = true){
		if (x1 == x2 || y1 == y2) { // 1 straight corridor
			Corridor corridor = new Corridor();
			corridor.start = new Vector2Int (x1, y1);
			corridor.end = new Vector2Int (x2, y2);
			corridorList.Add (corridor);
		} else { //curved corridor, so 2 straight corridors joined at the ends
			if (topConn) {
				Corridor corridor = new Corridor();
				corridor.start = new Vector2Int (x1, y1);
				corridor.end = new Vector2Int (x2, y1);
				corridorList.Add (corridor);
				corridor.start = new Vector2Int (x2, y1); //TODO add offset here so there's only one tile at the intersection
				corridor.end = new Vector2Int (x2, y2);
				corridorList.Add (corridor);
			} else {
				Corridor corridor = new Corridor();
				corridor.start = new Vector2Int (x1, y1);
				corridor.end = new Vector2Int (x1, y2);
				corridorList.Add (corridor);
				corridor.start = new Vector2Int (x1, y2); //TODO add offset here so there's only one tile at the intersection
				corridor.end = new Vector2Int (x2, y2);
				corridorList.Add (corridor);
			}
		}
	}
	void ConnectRooms(Room room1, Room room2){  
		if (room1.x > room2.x) {
			Room temp = room2;
			room2 = room1;
			room1 = temp;
		}
		if (room1.x < room2.x + room2.width && room2.x < room1.x + room1.width) { //overlapping on x?
			int jx1 = UnityEngine.Random.Range(room2.x, room1.x + room1.width);
			int jx2 = jx1;
			int[] tmpY = new int[] { room1.y, room2.y, room1.y + room1.height - 1, room2.y + room2.height - 1 };
			Array.Sort (tmpY);
			int jy1 = tmpY [1] + 1;
			int jy2 = tmpY [2] - 1;
			GenerateCorridor (jx1, jy1, jx2, jy2);
		} else if (room1.y < room2.y + room2.height && room2.y < room1.y + room1.height) { //overlapping on y?
			int jy1, jy2;
			if (room2.y > room1.y) {
				jy1 = UnityEngine.Random.Range (room2.y, room1.y + room1.height);
				jy2 = jy1;
			} else {
				jy1 = UnityEngine.Random.Range (room1.y, room2.y + room2.height);
				jy2 = jy1;
			}
			int[] tmpX = new int[] { room1.x, room2.x, room1.x + room1.width - 1, room2.x + room2.width - 1 };
			Array.Sort (tmpX);
			int jx1 = tmpX [1] + 1;
			int jx2 = tmpX [2] - 1;
			GenerateCorridor (jx1, jy1, jx2, jy2);
		} else { //no overlap, make a curve
			bool topConn = true;
			if (UnityEngine.Random.value > 0.5f) {
				topConn = false;
			}
			if (topConn) {
				if (room2.y > room1.y) {
					int jx1 = room1.x + room1.width - 1;
					int jy1 = UnityEngine.Random.Range (room1.y, room1.y + room1.height);
					int jx2 = UnityEngine.Random.Range (room2.x, room2.x + room2.width);
					int jy2 = room2.y + 1;
					GenerateCorridor (jx1, jy1, jx2, jy2, false); //is this supposed to be false, really?
				} else {
					int jx1 = UnityEngine.Random.Range (room1.x, room1.x + room1.width);
					int jy1 = room1.y + 1;
					int jx2 = room2.x + 1;
					int jy2 = UnityEngine.Random.Range (room2.y, room2.y + room2.height);
					GenerateCorridor (jx1, jy1, jx2, jy2, true);
				}
			} else {
				if (room2.y > room1.y) {
					int jx1 = UnityEngine.Random.Range (room1.x, room1.x + room1.width);
					int jy1 = room1.y + room1.height - 1;
					int jx2 = room2.x + 1;
					int jy2 = UnityEngine.Random.Range (room2.y, room2.y + room2.height);
					GenerateCorridor (jx1, jy1, jx2, jy2, true);
				} else {
					int jx1 = room1.x + room1.width - 1;
					int jy1 = UnityEngine.Random.Range (room1.y, room1.y + room1.height);
					int jx2 = UnityEngine.Random.Range (room2.x, room2.x + room2.width);
					int jy2 = room2.y + room2.height - 1;
					GenerateCorridor (jx1, jy1, jx2, jy2, false);
				}
			}
		}
	}
	bool IsRoomOverlapping(Room room){
		foreach (Room otherRoom in roomList) {
			if (room.x < otherRoom.x + otherRoom.width && otherRoom.x < (room.x + room.width) && room.y < otherRoom.y + otherRoom.height && otherRoom.y < (room.y + room.height)) {
				return true;
			}
		}
		return false;
	}
	public struct Corridor{
		public Vector2Int start;
		public Vector2Int end;
		public bool isVertical;

		public Corridor( Vector2Int start, Vector2Int end){
			this.start = start;
			this.end = end;
			if (start.x == end.x) { //TODO - this doesn't seem to be working, delete later
				isVertical = true;
			}else{
				isVertical = false;
			}
		}
	}
	struct Room{
		public int x;
		public int y;
		public int width;
		public int height;

		public Room(int x, int y, int width, int height){
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;

		}
	}
}
