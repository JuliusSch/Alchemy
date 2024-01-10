using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour, IInteractable
{
    private Coroutine current;
    private Rigidbody currentSettler;
    private List<Rigidbody> itemsContained;
    private bool held = false;

    public List<GameObject> BodyParts;


    private void Start()
    {
        itemsContained = new List<Rigidbody>();
    }

    public void PrimaryInteraction(Transform heldObject, PickUp pickUpScript)
    {
        if (heldObject == null)
        {
            foreach (var item in itemsContained)
                Settle(item);
            held = true;
            pickUpScript.PickUpObject(gameObject, this, true);
        } else
        {
            //foreach (var item in itemsContained)
            //    Unsettle(item);

            pickUpScript.DropHeld();
            //held = false;
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
        IInteractable item;
        if ((item = other.GetComponent<IInteractable>()) != null && !BodyParts.Contains(other.gameObject))
            itemsContained.Add(item.gameObject.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (held)
            return;
        Rigidbody item;
        if ((item = other.GetComponent<Rigidbody>()) != null && !BodyParts.Contains(other.gameObject))
            itemsContained.Remove(item.GetComponent<Rigidbody>());
    }

    //private void PutObjectInBasket(Transform obj, PickUp pickUpScript)
    //{
    //    pickUpScript.DropHeld();
    //    CheckIfSettled();
    //    currentSettler = obj.GetComponent<Rigidbody>();
    //    current = StartCoroutine(InanimateWhenSettled());
    //}

    //private void CheckContents()
    //{
    //    foreach (var item in itemsContained)
    //    {
    //        item.gameObject.GetComponent<Rigidbody>();
    //    }
    //}

    //private void CheckIfSettled()
    //{
    //    if (current != null)
    //    {
    //        StopCoroutine(current);
    //        Settle(currentSettler);
    //        current = null;
    //        currentSettler = null;
    //    }
    //}

    private void Settle(Rigidbody rb)
    {
        //if (GetComponent<BoxCollider>().Contains(currentSettler.transform.position))
        //{
            rb.transform.parent = transform;
            rb.useGravity = false;
            rb.isKinematic = true;
        //}
    }

    private void Unsettle(Rigidbody rb)
    {
        rb.transform.parent = null;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    //private IEnumerator InanimateWhenSettled()
    //{
    //    float time = 0f;
    //    while (time < 3 || currentSettler.velocity.magnitude > 0.01)
    //    {
    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //    Settle(currentSettler);

    //    current = null;
    //    currentSettler = null;
    //}

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
