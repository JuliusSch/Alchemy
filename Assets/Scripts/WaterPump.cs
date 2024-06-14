using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPump : MonoBehaviour, IInteractable {

    [SerializeField] private IngredientSO waterIngredient;

    public void PrimaryInteraction(Transform heldObject, ItemInteraction pickUpScript) {
        IInteractable interactableObject = heldObject.GetComponent<IInteractable>();
        Debug.Log("so far");
        IngredientContainer waterBottle = heldObject.GetComponent<IngredientContainer>();
        if (waterBottle) {
            if (!waterBottle.Full) waterBottle.Fill(waterIngredient);
        }
    }

    public GameObject getGroup() {
        throw new System.NotImplementedException();
    }

    public bool Interact(string key) {
        return false;
    }

    public void MouseOver() {
        //shop tooltip e.g. press [right-click] to collect water.
    }
}
