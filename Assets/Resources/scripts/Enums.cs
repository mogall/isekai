using UnityEngine;

public enum CameraMode {MOVEMENT, UI};
public enum SkillType {INSTANT, CHANNEL, STATUS, MELEE};
public enum TargetingMode {NOTARGETING, POINT, AOE, GROUNDAOE, ENEMY, SELF, SELFAOE, CONE, LINE} //ADD - add more stuff to this later?
public enum PlayerMovementState {STANDING, WALKING, RUNNING, BACKWALK, BACKRUN, JUMPING, FALLING};
public enum PlayerActionState {NOTHING, CRAFTING, CASTING, INTERACTING, ATTACKING, CHANNELING, TARGETING};

public enum CraftStationType {HAND, COOKINGSTATION, SMELTINGFURNACE, ANVIL, TABLE};

public enum HouseType {EMPTY, SHACK, VILLAGEHOUSE, TOWNHOUSE, MANSION}; //ADD - more house types here, also unique types like a church or a tavern here or in layout type?
public enum HouseLayoutType {SINGLEROOM, INLINE, CROSS, HALLWAY, ENTRANCEHALL, UNIQUE}; //ADD - add more house layouts here, but does it clash with housetype?
public enum HouseRoomType {ENTRANCEHALL, HALLWAY, GENERICROOM, LIVINGROOM, BEDROOM, BATHROOM, KITCHEN, STORAGE}; //ADD - more room types here
public enum HouseTileType {NORTHWALL, SOUTHWALL, EASTWALL, WESTWALL, ENTRANCE, MIDDLE};

//public enum EnemyIdleType {GROUNDAREA, WALLWALKAREA, STATIONARY, RAILS, AIRRAILS, AIRAREA};
//public enum EnemyAggroType{BEELINE, STATIONARY, KEEPDISTANCE, RUNAWAY};
//public enum EnemyAttackType{NONE, MELEE, RANGED, AREA, AURA};

public class Enums : MonoBehaviour {

}
