using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO - outer doors, outer walls, calculate where the inner doors should be, windows, etc
public class House : MonoBehaviour { 
	public HouseType houseType = HouseType.EMPTY;
	public HouseLayoutType houseLayoutType = HouseLayoutType.SINGLEROOM;
	public Vector2Int size; //IMPORTANT - house x should always be equal or larger than y, for generation simplicity purposes

	void Start(){
		GenerateHouse ();
	}

	void GenerateHouse(){
		switch (houseLayoutType) {

		case HouseLayoutType.SINGLEROOM: //house is a single room - everything that the house needs is in this room
			HouseRoom room = new HouseRoom ();
			room.size = size;
			room.houseRoomType = HouseRoomType.GENERICROOM;
			room.origin = transform.position;
			room.doors.Add (new Vector2Int (Random.Range (1, size.x - 1), 0));
			List<Vector2Int> windows = new List<Vector2Int> ();
			room.windows.AddRange (GenerateWindowLocations (0, 0, size.x-1, 0, true));
			room.windows.AddRange (GenerateWindowLocations (0, size.y-1, size.x-1, 0, true));
			room.windows.AddRange (GenerateWindowLocations (0, 0, 0, size.y-1, false));
			room.windows.AddRange (GenerateWindowLocations (size.x-1, 0, 0, size.y-1, false));
			room.GenerateRoomLayout ();
			break;

		case HouseLayoutType.INLINE: //house is a few rooms in line, how much depends on the length of the house //TODO - make it so the entrance is in the middle(second room maybe?)
			int inlineMinRoomSize = 3; //UPGRADE - actually this whole thing makes stuff in line, upgrade to also make rooms vertically?
			int inlineMaxRoomsize = 6;
			bool done = false;
			int origin = 0;
			Vector2Int lastRoomDoor = Vector2Int.zero; //TODO - for some reason v2int type can't be null, so assigned a zero value for comparison. BEWARE FOR ERRORS.
			while (!done) {
				int rand = Random.Range (inlineMinRoomSize, inlineMaxRoomsize); //TODO - remove the +1 thing, it makes the rooms far apart, for visual purposes now
				if (rand + origin >= size.x-1) { //FIX - this is a hack so it doesn't make doors at the last room, fix this so it works better
					rand = size.x - origin;
					done = true;
				}
				if (rand <= 1) {
					break;
				}
				room = new HouseRoom ();
				room.size = new Vector2Int (rand, size.y);
				if (origin == 0) {
					room.houseRoomType = HouseRoomType.ENTRANCEHALL;
					room.doors.Add (new Vector2Int(Random.Range(1,rand-1),0));
					lastRoomDoor = new Vector2Int (rand-1, Random.Range (1, size.y - 1)); //FIX - this doesn't check if there's only one room(there shouldn't be, but still)
					room.doors.Add (lastRoomDoor);
				} else {
					room.houseRoomType = utils.GetRandomEnum <HouseRoomType>();//TODO - change this so it actually chooses rooms based on some criteria, right now it's just random
					room.doors.Add (new Vector2Int(0, lastRoomDoor.y));
					if (!done) {
						lastRoomDoor = new Vector2Int (rand-1, Random.Range (1, size.y - 1)); //FIX - same as above
						room.doors.Add (lastRoomDoor);

					}

				}
				room.windows.AddRange (GenerateWindowLocations (0, 0, room.size.x-1, 0, true));
				room.windows.AddRange (GenerateWindowLocations (0, room.size.y-1, room.size.x-1, 0, true));
				room.origin = new Vector3(transform.position.x + origin, transform.position.y, transform.position.z);
				room.GenerateRoomLayout ();
				origin += rand;
			}
			break;

		case HouseLayoutType.CROSS: //this only makes 4 rooms because it just splits the plot 2 times
			//done = false;
			int originX = 0;
			int originY = 0;
			int sliceX = (int)(Random.Range (size.x * 0.4f, size.x * 0.6f)); //TODO - upgrade this so it's more of a "how much of a room you want", and not "just slice it mang".
			int sliceY = (int)(Random.Range (size.y * 0.4f, size.y * 0.6f));
			room = new HouseRoom ();  //TODO - THE FOLLOWING CODE ADDING 4 ROOMS IS EXTREMELY SIMPLISTIC, UPGRADE IT
			room.size.x = originX + sliceX;
			room.size.y = originY + sliceY;
			room.houseRoomType = HouseRoomType.ENTRANCEHALL;
			room.origin = new Vector3 (transform.position.x + originX, transform.position.y, transform.position.z + originY);
			room.doors.Add (new Vector2Int (Random.Range (1, room.size.x - 1), 0));
			int spot1 = Random.Range (1, room.size.y - 1);
			room.doors.Add (new Vector2Int (room.size.x - 1, spot1));
			int spot2 = Random.Range (1, room.size.x - 1);
			room.doors.Add (new Vector2Int (spot2, room.size.y - 1));
			room.windows.AddRange (GenerateWindowLocations (0, 0, room.size.x-1, 0, true));
			room.windows.AddRange (GenerateWindowLocations (0, 0, 0, room.size.y-1, false));
			room.GenerateRoomLayout ();
			room = new HouseRoom ();
			room.size.x = size.x - sliceX;
			room.size.y = originY + sliceY;
			room.houseRoomType = utils.GetRandomEnum <HouseRoomType> ();
			room.origin = new Vector3 (transform.position.x + sliceX, transform.position.y, transform.position.z + originY);
			room.doors.Add (new Vector2Int (0, spot1));
			room.windows.AddRange (GenerateWindowLocations (0, 0, room.size.x-1, 0, true));
			room.windows.AddRange (GenerateWindowLocations (room.size.x-1, 0, 0, room.size.y-1, false));
			room.GenerateRoomLayout ();
			room = new HouseRoom ();
			room.size.x = originX + sliceX;
			room.size.y = size.y - sliceY;
			room.houseRoomType = utils.GetRandomEnum <HouseRoomType> ();
			room.origin = new Vector3 (transform.position.x + originX, transform.position.y, transform.position.z + sliceY);
			room.doors.Add (new Vector2Int (spot2, 0));
			int spot3 = Random.Range (1, room.size.y - 1);
			room.doors.Add (new Vector2Int (room.size.x - 1, spot3));
			room.windows.AddRange (GenerateWindowLocations (0, 0, 0, room.size.y-1, false));
			room.windows.AddRange (GenerateWindowLocations (0, room.size.y-1, room.size.x-1, 0, true));
			room.GenerateRoomLayout ();
			room = new HouseRoom ();
			room.size.x = size.x - sliceX;
			room.size.y = size.y - sliceY;
			room.houseRoomType = utils.GetRandomEnum <HouseRoomType> ();
			room.origin = new Vector3 (transform.position.x + sliceX, transform.position.y, transform.position.z + sliceY);
			room.doors.Add (new Vector2Int (0, spot3));
			room.windows.AddRange (GenerateWindowLocations (0, room.size.y-1, room.size.x-1, 0, true));
			room.windows.AddRange (GenerateWindowLocations (room.size.x-1, 0, 0, room.size.y-1, false));
			room.GenerateRoomLayout ();
			break;

		case HouseLayoutType.HALLWAY: //this does a hallway right through the house, and then puts rooms on both sides
			int hallwayPos = (int)(Random.Range (size.x * 0.4f, size.x * 0.6f)); //TODO - make a "is hallway all the way through" thing and then make rooms at the end(or sideways hallway?)
			room = new HouseRoom ();
			room.size = new Vector2Int (1, size.y);
			room.houseRoomType = HouseRoomType.HALLWAY;
			room.origin = new Vector3 (transform.position.x + hallwayPos, transform.position.y, transform.position.z);
			room.doors.Add (new Vector2Int(0, 0));
			room.GenerateRoomLayout ();
			int leftMinRoomSize = 3;
			int leftMaxRoomSize = 5;
			bool leftDone = false;
			origin = 0;
			while (!leftDone) {
				int rand = Random.Range (leftMinRoomSize, leftMaxRoomSize + 1); //TODO - remove the +1 thing, it makes the rooms far apart, for visual purposes now
				if (rand + origin >= size.y-1) {
					rand = size.y - origin;
					leftDone = true;
				}
				if (rand <= 1) {
					break;
				}
				room = new HouseRoom ();
				room.size = new Vector2Int (hallwayPos, rand);
				room.houseRoomType = utils.GetRandomEnum <HouseRoomType>();//TODO - change this so it actually chooses rooms based on some criteria, right now it's just random
				room.origin = new Vector3(transform.position.x, transform.position.y, transform.position.z+origin);
				room.doors.Add (new Vector2Int (room.size.x - 1, Random.Range (1, room.size.y - 1))); //FIX - this only sets doors on the room, not on the hallway, fix this
				room.windows.AddRange (GenerateWindowLocations (0, 0, 0, room.size.y-1, false));
				room.GenerateRoomLayout ();
				origin += rand;
			}
			int rightMinRoomSize = 2;
			int rightMaxRoomSize = 3;
			bool rightDone = false;
			origin = 0;
			while (!rightDone) {
				int rand = Random.Range (rightMinRoomSize, rightMaxRoomSize + 1); //TODO - remove the +1 thing, it makes the rooms far apart, for visual purposes now
				if (rand + origin >= size.y-1) {
					rand = size.y - origin;
					rightDone = true;
				}
				if (rand <= 1) {
					break;
				}
				room = new HouseRoom ();
				room.size = new Vector2Int (size.x-hallwayPos-1, rand);
				room.houseRoomType = utils.GetRandomEnum <HouseRoomType>();//TODO - change this so it actually chooses rooms based on some criteria, right now it's just random
				room.origin = new Vector3(transform.position.x+hallwayPos+1, transform.position.y, transform.position.z+origin);
				room.doors.Add (new Vector2Int (0, Random.Range (1, room.size.y - 1))); //FIX - this only sets doors on the room, not on the hallway, fix this
				room.windows.AddRange (GenerateWindowLocations (room.size.x-1, 0, 0, room.size.y-1, false));
				room.GenerateRoomLayout ();
				origin += rand;
			}
			break;
		
		case HouseLayoutType.ENTRANCEHALL:
			//TODO - finish here
			break;

		default:
			room = new HouseRoom ();
			room.size = size;
			room.houseRoomType = HouseRoomType.GENERICROOM;
			room.GenerateRoomLayout ();
			room.origin = transform.position;
			break;

		}
	}
	List<Vector2Int> GenerateWindowLocations(int x1, int y1, int sizex, int sizey, bool horizontal){ //UPGRADE - this is super rudimentary, upgrade later(or replace completely)
		List<Vector2Int> windowlist = new List<Vector2Int> (); //this function returns a list of "window vectors" in a line along the wall, input starting coordinates in a house and sizes, and select if it's horizonal or vertical
		bool done = false;
		int pos = Random.Range (1, 3);
		if (horizontal) {
			if (pos >= sizex) {
				pos = Random.Range (0, sizex);
				windowlist.Add (new Vector2Int (x1 + pos, y1));
				done = true;
			}
			while (!done) {
				windowlist.Add (new Vector2Int (x1 + pos, y1));
				pos += Random.Range (3, 5);
				if (pos >=sizex) {
					done = true;
				}
			}
		} else {
			if (pos >= sizey) {
				pos = Random.Range (0, sizey);
				windowlist.Add (new Vector2Int (x1, y1 + pos));
				done = true;
			}
			while (!done) {
				windowlist.Add (new Vector2Int (x1, y1 + pos));
				pos += Random.Range (3, 5);
				if (pos >=sizey) {
					done = true;
				}
			}
		}

		return windowlist;
	}
}
