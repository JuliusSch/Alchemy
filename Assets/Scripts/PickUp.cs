using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PickUp : MonoBehaviour {

    public Transform holdGuide, placeGuide;
    public Camera camera2;
    public float pickupDst = 1f;
    public bool holding = false;
    public Image cursor;
    public Collider[] RaycastExemptColliders;

    private Transform heldObject;
    private const int interactableLayer = 9, rayCastLayer = 2, defaultLayer = 0;
    private LayerMask rayCastMask = ~(1 << rayCastLayer);
    private MouseLook _mouseLook;

    private void Start()
    {
        _mouseLook = FindObjectOfType<MouseLook>();
    }

    void Update() {
        var allHits = Physics.RaycastAll(camera2.transform.position, camera2.transform.forward, pickupDst, rayCastMask);
        if (allHits != null && allHits.Length > 0) 
        {
            allHits = allHits.OrderBy(h => h.distance).ToArray();
            foreach(var hit in allHits)
            {
                try
                {
                    if (hit.collider != null && RaycastExemptColliders.Contains(hit.collider))
                        continue;
                    else
                    {
                        IInteractable hitInteractable = hit.collider.attachedRigidbody.gameObject.GetComponent<IInteractable>();
                        cursor.color = Color.grey;
                        _mouseLook.SetMouseOver(true);
                        hitInteractable.MouseOver();
                        if (Input.GetButtonUp("Interact")) hitInteractable.PrimaryInteraction(heldObject, this);
                        if (Input.GetButtonUp("Interact")) hitInteractable.Interact("e");
                        return;
                    }
                } catch (NullReferenceException) { break; }
            }
        }
        cursor.color = Color.white;
        _mouseLook.SetMouseOver(false);
        if (Input.GetButtonUp("Interact") && holding)
            DropHeld();
        //Physics.Raycast(camera2.transform.position, camera2.transform.forward, out RaycastHit hit, pickupDst, rayCastMask);
        //try {
        //    IInteractable hitInteractable = hit.collider.attachedRigidbody.gameObject.GetComponent<IInteractable>();
        //    cursor.color = Color.grey;
        //    _mouseLook.SetMouseOver(true);
        //    hitInteractable.MouseOver();
        //    if (Input.GetButtonUp("Interact")) hitInteractable.PrimaryInteraction(heldObject, this);
        //    if (Input.GetButtonUp("Interact")) hitInteractable.Interact("e");
        //} catch (NullReferenceException)
        //{
        //    cursor.color = Color.white;
        //    _mouseLook.SetMouseOver(false);
        //    if (Input.GetButtonUp("Interact") && holding) DropHeld();
        //}
    }

    public void PickUpObject(GameObject target, IInteractable targetInteractable, bool isBasket = false) {
        holding = true;
        heldObject = target.transform;
        heldObject.parent = holdGuide;
        heldObject.GetComponent<Rigidbody>().useGravity = false;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.localPosition = Vector3.zero;
        heldObject.localRotation = Quaternion.identity;
        heldObject.Rotate(-90, 0, 0);
        heldObject.gameObject.layer = defaultLayer;
        if (isBasket)
        {
            heldObject.Rotate(90, 0, 0);
            heldObject.Rotate(0, -60, 0);
            heldObject.localPosition = new Vector3(0, -0.3f, 0);
        }
    }

    public void DropHeld() {
        if (heldObject.GetComponent<Basket>() != null)
        {
            PlaceBasket();
        } else
        {
            holding = false;
            heldObject.parent = placeGuide;
            heldObject.localPosition = Vector3.zero;
            //heldObject.position += Vector3.Scale(heldObject.GetComponentInChildren<Renderer>().bounds.extents, placeGuide.GetComponent<PositionGuide>().GetHitNormal()) * .7f;
            Vector3 targetPostition = new Vector3(camera2.transform.position.x, heldObject.transform.position.y, camera2.transform.position.z);
            heldObject.LookAt(targetPostition);
            heldObject.Rotate(-90f, 180f, 0);
            heldObject.parent = heldObject.GetComponent<IInteractable>().getGroup().transform;
            heldObject.GetComponent<Rigidbody>().useGravity = true;
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.gameObject.layer = interactableLayer;
            heldObject = null;
        }
    }

    private void PlaceBasket()
    {
        holding = false;
        heldObject.parent = placeGuide;
        heldObject.localPosition = Vector3.zero;
        Vector3 targetPostition = new Vector3(camera2.transform.position.x, heldObject.transform.position.y, camera2.transform.position.z);
        heldObject.LookAt(targetPostition);
        heldObject.parent = null;
        heldObject.Rotate(0, 90f, 0);
        heldObject.GetComponent<Rigidbody>().useGravity = true;
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.gameObject.layer = interactableLayer;
        heldObject.GetComponent<Basket>().Drop();
        heldObject = null;
    }

    public void DeleteHeld() {
        holding = false;
        Destroy(heldObject.gameObject);
        heldObject = null;
    }

    private void ExchangeContents(IngredientContainer target) {
        if (heldObject == null) return;
        if (target.Full && !heldObject.GetComponent<IngredientContainer>().Full) heldObject.GetComponent<IngredientContainer>().Fill(target.Empty());
        else if (!target.Full && heldObject.GetComponent<IngredientContainer>().Full) target.Fill(heldObject.GetComponent<IngredientContainer>().Empty());
        else if (target.SemiFull && !heldObject.GetComponent<IngredientContainer>().Full) heldObject.GetComponent<IngredientContainer>().Fill(target.Empty());
    }
}
