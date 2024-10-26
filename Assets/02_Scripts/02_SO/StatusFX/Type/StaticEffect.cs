using MinD.Enums;
using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.StatusFX {

public abstract class StaticEffect : ScriptableObject {

	public StaticEffectType enumId;

	private BaseEntity owner;

	public void OnInstantiate(BaseEntity owner) {

		this.owner = owner;

		if (owner is Player player)
			OnInstantiateAs(player);

		if (owner is Enemy enemy)
			OnInstantiateAs(enemy);

	}

	protected abstract void OnInstantiateAs(Player target);
	protected abstract void OnInstantiateAs(Enemy target);


	public void OnRemove() {

		if (owner is Player player)
			OnRemoveAs(player);

		if (owner is Enemy enemy)
			OnInstantiateAs(enemy);

	}

	protected abstract void OnRemoveAs(Player target);
	protected abstract void OnRemoveAs(Enemy target);
}

}