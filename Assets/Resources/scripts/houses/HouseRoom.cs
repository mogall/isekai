using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO - doors, what should calculate tile types(neaar wall etc), this or the house script? this should be pretty easy, no?
public class HouseRoom : MonoBehaviour {
	public Vector2Int size;
	public HouseRoomType houseRoomType = HouseRoomType.GENERICROOM;
	public HouseTileType[,] houseTileTypes;
	public List<Vector2Int> doors = new List<Vector2Int>();
	public List<Vector2Int> windows = new List<Vector2Int> ();
	public List<Vector2Int> tilesNearWalls = new List<Vector2Int> ();
	public Vector3 origin;


	public void GenerateRoomLayout(){
		foreach (Vector2Int door in doors) { //UPGRADE - this is just visualization of where doors are, upgrade this when placing proper doors
			GameObject doorObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			doorObj.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			doorObj.transform.localPosition = new Vector3 (origin.x+door.x,origin.y,origin.z+door.y);
			doorObj.GetComponent<MeshRenderer> ().material.color = Color.red;
		}
		foreach (Vector2Int window in windows) { //UPGRADE - this is just visualization of where doors are, upgrade this when placing proper doors
			GameObject doorObj = GameObject.CreatePrimitive(PrimitiveType.Cube);  //TODO - check is window is placed on doors and if so, don't place it(and remove from list)
			doorObj.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			doorObj.transform.localPosition = new Vector3 (origin.x+window.x,origin.y,origin.z+window.y);
			doorObj.GetComponent<MeshRenderer> ().material.color = Color.white;
		}
		for (int j = 0; j < size.y; j++) {//REMOVE - temporary visualization of room type and layout, delete later and replace with proper floor making
			for (int i = 0; i < size.x; i++) {
				GameObject floorTile = GameObject.CreatePrimitive (PrimitiveType.Quad);
				floorTile.transform.localPosition = new Vector3 (origin.x+i, origin.y, origin.z+j);
				floorTile.transform.rotation = Quaternion.Euler(new Vector3 (90, 0, 0));
				MeshRenderer rend = floorTile.GetComponent<MeshRenderer> ();
				switch (houseRoomType) { 
				case HouseRoomType.ENTRANCEHALL:
					rend.material.color = Color.grey;
					break;
				case HouseRoomType.HALLWAY:
					rend.material.color = Color.white;
					break;
				case HouseRoomType.LIVINGROOM:
					rend.material.color = Color.blue;
					break;
				case HouseRoomType.BEDROOM:
					rend.material.color = Color.green;
					break;
				case HouseRoomType.BATHROOM:
					rend.material.color = Color.cyan;
					break;
				case HouseRoomType.KITCHEN:
					rend.material.color = Color.yellow;
					break;
				case HouseRoomType.STORAGE:
					rend.material.color = Color.magenta;
					break;
				case HouseRoomType.GENERICROOM:
					rend.material.color = Color.black;
					break;
				default:
					rend.material.color = Color.red;
					break;
				}
			}
		}
	}
}
