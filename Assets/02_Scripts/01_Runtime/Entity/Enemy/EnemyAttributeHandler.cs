using MinD.SO.EnemySO;
using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyAttributeHandler : BaseEntityAttributeHandler<Enemy> {
	
	[SerializeField] private EnemyAttribute attributeSO;

	public override int MaxHp {
		get => attributeSO.maxHp;
		set {
			return;
		}
	}

	public override DamageNegation DamageNegation {
		get => attributeSO.damageNegation;
		set {
			return;
		}
	}

	public override int PoiseBreakResistance {
		get => attributeSO.poiseBreakResistance;
		set {
			return;
		}
	}

	public float speed {
		get => attributeSO.speed;
	}
}

}