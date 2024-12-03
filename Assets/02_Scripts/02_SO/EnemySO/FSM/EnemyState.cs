using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO {

public abstract class EnemyState : ScriptableObject {

	public abstract EnemyState Tick(Enemy self);

}

}