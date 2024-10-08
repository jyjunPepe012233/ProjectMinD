using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(Animator))]
public class PlayerInteractionHandler : MonoBehaviour {

	[HideInInspector] public Player owner;

	[SerializeField] private List<Interactable> currentInteractables = new List<Interactable>();
	

	public void AddInteractableInList(Interactable interactable) {
		
		if (!currentInteractables.Contains(interactable))
			currentInteractables.Add(interactable);
	}

	public void RemoveInteractableInList(Interactable interactable) {
		
		if (currentInteractables.Contains(interactable))
			currentInteractables.Remove(interactable);
	}

	public void RefreshInteractableList() {

		if (currentInteractables.Count == 0)
			return;
		
		for (int i = currentInteractables.Count-1; i < -1; i--) /*REVERSE FOR*/ {

			Interactable interactable = currentInteractables[i];
			
			// IS INTERACTION IS DESTROYED
			// OR INTERACTION CAN'T INTERACTION BY PARAMETER
			if (interactable == null || !interactable.canInteraction)
				currentInteractables.Remove(interactable);

			if (currentInteractables.Count == 0)
				break;
		}

		// refresh popup
	}

	public void Interact() {

		if (currentInteractables.Count == 0)
			return;

		if (currentInteractables[0] == null)
			return;
		
		if (currentInteractables[0].canInteraction)
			currentInteractables[0].Interact(owner);
	}
}