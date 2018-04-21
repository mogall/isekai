using UnityEngine;

public interface ITakeDamage{ //use this on an entity that can receive damage(like an enemy, a monster)
	void TakeDamage (int damage);
}
public interface IEnemyIdleBehaviour{ //use this on a script that defines idle enemy behaviour - walking around etc, when the enemy does not have a detected/specified target
	void TakeControl();
}
public interface IEnemyAggroBehaviour{ //use this on a script that defines behaviour of an enemy that has detected a target and is actively "pursuing" it.
	void TakeControl();
}

/*public interface ISkillBarUseable{ //use this on an element that can be placed on a skill bar and used (like a spell or a consumable)
	void Use();
}*/




public class Interfaces : MonoBehaviour {
	
}
