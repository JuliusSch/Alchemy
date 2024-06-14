using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour, IInteractable
{
    private List<Rigidbody> itemsContained;
    private bool held = false;

    public List<GameObject> BodyParts;


    private void Start()
    {
        itemsContained = new List<Rigidbody>();
    }

    public void PrimaryInteraction(Transform heldObject, ItemInteraction pickUpScript)
    {
        if (heldObject == null)
        {
            foreach (var item in itemsContained)
                Settle(item);
            held = true;
            pickUpScript.PickUpObject(gameObject, this, true);
        } else
        {
            pickUpScript.PlaceHeld();
        }
    }

    public void Drop()
    {
        foreach (var item in itemsContained)
            Unsettle(item);

        held = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (held)
            return;
        IInteractable item = other.GetComponentInParent<IInteractable>();
        if (item != null && (item is IngredientContainer || item is ConcoctionContainer || item is SeedItem) && !BodyParts.Contains(other.gameObject))
            itemsContained.Add(item.gameObject.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (held)
            return;
        Rigidbody item;
        if ((item = other.GetComponentInParent<Rigidbody>()) != null && !BodyParts.Contains(other.gameObject))
            itemsContained.Remove(item.GetComponent<Rigidbody>());
    }

    private void Settle(Rigidbody rb)
    {
        rb.transform.parent = transform;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void Unsettle(Rigidbody rb)
    {
        rb.transform.parent = null;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public GameObject getGroup()
    {
        return null;
    }

    public bool Interact(string key)
    {
        return false;
    }

    public void MouseOver()
    {
    }
}
