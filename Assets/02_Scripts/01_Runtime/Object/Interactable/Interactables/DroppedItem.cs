using System.Collections;
using System.Linq;
using MinD.Enums;
using MinD.Runtime.DataBase;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.UI;
using MinD.SO.Item;
using UnityEngine;

namespace MinD.Runtime.Object.Interactables {

public class DroppedItem : Interactable {

	[Header("[ Item Settings ]")]
	[SerializeField] private Item item;
	[SerializeField] private int itemCount;


	public void Awake() {

		if (item == null)
			Destroy(gameObject);

		switch (item.itemRarity) {

			case (ItemRarityEnum.Common):
				Instantiate(VfxDataBase.Instance.droppedItemCommon, transform);
				break;

			case (ItemRarityEnum.Rare):
				Instantiate(VfxDataBase.Instance.droppedItemRare, transform);
				break;

			case (ItemRarityEnum.Legendary):
				Instantiate(VfxDataBase.Instance.droppedItemLegendary, transform);
				break;
		}

	}



	public override void Interact(Player interactor) {

		if (interactor.inventory.AddItem(item.itemId, itemCount, false)) {
			// ADD ITEM IS CLEARLY WORK ELSE ITEM IS EXCEEDED

			interactor.interaction.RemoveInteractableInList(this);
			interactor.interaction.RefreshInteractableList();
			
			canInteraction = false;

			StartCoroutine(FadeOutDestroy(2));

		} else {
			// IF ADD ITEM IS CANCELED CAUSE ITEM IS EXCEEDED MAX COUNT OF ITEM

			// function when item count is exceeded

		}

	}

	public IEnumerator FadeOutDestroy(float fadeTime) {

		GetComponentInChildren<ParticleSystem>().Stop();

		Light lightFx = GetComponentInChildren<Light>();
		float lightStartIntensity = lightFx.intensity;

		float fadeOutElapsed = 0;

		while (true) {

			fadeOutElapsed += Time.deltaTime;
			if (fadeOutElapsed > 2)
				break;

			lightFx.intensity -= lightStartIntensity / fadeTime;

			yield return null;
		}

		Destroy(gameObject);
	}


	private void OnDrawGizmos() {

		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, 0.3f);

	}
}

}