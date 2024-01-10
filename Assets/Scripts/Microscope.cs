using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microscope : MonoBehaviour, IInteractable {

    IngredientSO sampledIngredient;

    public void PrimaryInteraction(Transform heldObject, PickUp pickUpScript) {
        if (sampledIngredient) sampledIngredient = null;
        else {
            if (heldObject == null) return;
            IInteractable interactableObject = heldObject.GetComponent<IInteractable>();
            IngredientContainer ingredientBottle = heldObject.GetComponent<IngredientContainer>();
            if (ingredientBottle && ingredientBottle.Full) sampledIngredient = ingredientBottle.IngredientInst.data;
        }
    }

    public GameObject getGroup() {
        throw new System.NotImplementedException();
    }

    public bool Interact(string key) {
        if (key == "e" && sampledIngredient) {
            Debug.Log("scan ingredient");
            return true; // reveal ingredient chart
        }
        return false;
    }

    public void MouseOver() {
        //  tooltip
    }

    void Update() {

    }
}
