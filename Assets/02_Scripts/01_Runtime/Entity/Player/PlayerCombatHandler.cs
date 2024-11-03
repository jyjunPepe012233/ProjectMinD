using System;
using MinD.Runtime.Managers;
using MinD.SO.Item;
using UnityEngine;


namespace MinD.Runtime.Entity {

public class PlayerCombatHandler : MonoBehaviour {

	[HideInInspector] public Player owner;
	
	// LOCKING ON ENTITY
	public BaseEntity target;
	
	
	[HideInInspector] public Magic currentCastingMagic;
	private bool usingMagic;
	
	// WHEN PLAYER GET HIT, CALL THIS ACTION IN 'TakeHealthDamage'
	public Action  getHitAction;
	
	

	public void HandleAllCombatAction() {
		HandleUsingMagic();
	}

	private void HandleUsingMagic() {

		if (currentCastingMagic != null) {
			currentCastingMagic.Tick();
		}


		if (PlayerInputManager.Instance.useMagicInput) {

			// CHECK BASIC FLAGS
			if (usingMagic || owner.isPerformingAction) {
				return;
			}


			Magic useMagic = owner.inventory.magicSlots[owner.inventory.currentMagicSlot];

			if (useMagic == null) {
				return;
			}

			// CANCEL IF PLAYER HASN'T ENOUGH MP OR STAMINA
			if (owner.CurMp < useMagic.mpCost) {
				return;
			}

			if (owner.CurStamina < useMagic.staminaCost) {
				return;
			}


			usingMagic = true;

			owner.CurMp -= useMagic.mpCost;
			owner.CurStamina -= useMagic.staminaCost;

			useMagic.castPlayer = owner;

			useMagic.OnUse();
			currentCastingMagic = useMagic;



		} else if (currentCastingMagic != null) {

			// IF INPUT IS NULL AND DURING CASTING => USE MAGIC INPUT IS END
			currentCastingMagic.OnReleaseInput();

		}
	}

	
	
	public void ExitCurrentMagic() {

		if (currentCastingMagic == null) {
			return;
		}

		usingMagic = false;

		currentCastingMagic.OnExit();
		currentCastingMagic = null;
	}
	
	public void CancelMagicOnGetHit() {

		// CANCEL MAGIC
		if (currentCastingMagic != null) {
			currentCastingMagic.OnCancel();
		}
		
	}



	public void OnInstantiateWarmUpFx() {
		currentCastingMagic.InstantiateWarmupFX();
	}

	public void OnSuccessfullyCast() {
		currentCastingMagic.OnSuccessfullyCast();
	}

}

}