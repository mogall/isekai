using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
	//basic stats
	public Stat HP;
	public Stat MP;
	public Stat stamina;
	public Stat hunger;
	public Stat thirst;

	//TODO - fill this list with magics, cooking and all kinds of talents that will influence skills
	public Talent running;




	public struct Talent{
		public float max;
		public float current;
	}
	public struct Stat{
		public int max;
		public int buffedMax;
		public int current;
	}
}

