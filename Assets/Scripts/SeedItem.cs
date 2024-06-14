using UnityEngine;

public class SeedItem : MonoBehaviour, IInteractable {

    public SeedSO seedType;
    private GameObject group;

    public void Start() {
        group = GameObject.Find("Seeds");
    }

    public void PrimaryInteraction(Transform heldObject, ItemInteraction pickUpScript) {
        if (heldObject == null) pickUpScript.PickUpObject(gameObject, this);
    }

    public GameObject getGroup() {
        return group;
    }

    public bool Interact(string key) {
        Debug.Log("Seed item interacted with.");
        return true;
    }

    public void MouseOver() {
    }
}
