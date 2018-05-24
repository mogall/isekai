using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/WeaponData")]
public class WeaponData : ItemData {
	[Header("Weapon Data")]
	public WeaponType weaponType;
	public int damage = 1; //TODO - just temporary
	public float range = 1;
	public float attackPoint = 0;
	public float attackCooldown = 1;
	public bool twoHanded = false;

}
