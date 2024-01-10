/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageContainer : MonoBehaviour, IInteractable {

    [SerializeField] private const int WIDTH = 4, HEIGHT = 5;
    private const float SLOT_WIDTH = 0.2f, SLOT_HEIGHT = 0.3f, TOP_LEFT_OFFSET_X = 1.3f, TOP_LEFT_OFFSET_Y = 1.8f;
    [SerializeField] private IngredientSO[] inventory = new IngredientSO[WIDTH * HEIGHT];
    private bool[] occupiedSlots = new bool[WIDTH * HEIGHT];

    protected void loadFromSO(IngredientSO newIng, int slotNumber) {
       *//* ingredient = new IngredientInstance(newIng, Instantiate(newIng.prefabModel, transform), 1);
        ingredient.model.transform.localRotation = Quaternion.identity;
        ingredient.model.transform.localPosition = getSlotPosition(slotNumber);
        Debug.Log(ingredient.model.transform.localPosition);*//*
    }

    private Vector3 getSlotPosition(int slotNumber) {
        float x = (slotNumber % WIDTH) * -SLOT_WIDTH + TOP_LEFT_OFFSET_X;
        float y = 0f;
        float z = (slotNumber / WIDTH) * -SLOT_HEIGHT + TOP_LEFT_OFFSET_Y;
        return new Vector3(x, y, z);
    }

    public bool fill(IngredientSO newIng) {
        int fillSlot = getFirstAvailableSlot();
        if (fillSlot == -1) return false;
        else {
            occupiedSlots[fillSlot] = true;
            inventory[fillSlot] = newIng;
            loadFromSO(newIng, fillSlot);
            return true;
        }
    }

    private int getFirstAvailableSlot() {
        for(int i = 0; i < WIDTH * HEIGHT; i++) {
            if (!occupiedSlots[i]) return i;
        }
        return -1;
    }

    public void click(Transform heldObject, PickUp pickUpScript) {
        if (heldObject == null) return;
        int openSlot = getFirstAvailableSlot();
        if (openSlot == -1) return;
        pickUpScript.dropHeld();
        heldObject.parent = this.transform;
        heldObject.GetComponent<Rigidbody>().useGravity = false;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.localRotation = Quaternion.identity;
        heldObject.localPosition = getSlotPosition(openSlot);
        occupiedSlots[openSlot] = true;
    }

    public void interact(string key) {
        throw new System.NotImplementedException();
    }

    bool IInteractable.interact(string key) {
        throw new System.NotImplementedException();
    }
}
*/