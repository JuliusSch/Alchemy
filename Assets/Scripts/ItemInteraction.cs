using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteraction : MonoBehaviour {

    public Transform holdGuide, placeGuide;
    public Camera camera2;
    public float pickupDst = 1f;
    public bool holding = false;
    public Image cursor;
    public Collider[] RaycastExemptColliders;
    public bool IsBusy;

    private Transform heldObject;
    private const int interactableLayer = 9, rayCastLayer = 2, defaultLayer = 0;
    private LayerMask rayCastMask = ~(1 << rayCastLayer);
    private MouseLook _mouseLook;
    private Tween activeTween;

    private void Start()
    {
        _mouseLook = FindObjectOfType<MouseLook>();
    }

    void Update() {
        if (IsBusy || PauseMenu.IsPaused) return;

        var allHits = Physics.RaycastAll(camera2.transform.position, camera2.transform.forward, pickupDst, rayCastMask);
        if (allHits != null && allHits.Length > 0) 
        {
            allHits = allHits.OrderBy(h => h.distance).ToArray();
            foreach(var hit in allHits)
            {
                try
                {
                    if ((hit.collider != null && RaycastExemptColliders.Contains(hit.collider)) || hit.transform.IsChildOf(holdGuide))
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
            PlaceHeld();
    }

    public void SetAsBusy(float time)
    {
        IsBusy = true;
        CallAfterDelay.Create(time, () => IsBusy = false);
    }

    public void PickUpObject(GameObject target, IInteractable targetInteractable, bool isBasket = false) {
        if (activeTween != null && activeTween.active) return;

        holding = true;
        heldObject = target.transform;
        heldObject.parent = holdGuide;
        heldObject.gameObject.layer = defaultLayer;
        heldObject.GetComponent<Rigidbody>().useGravity = false;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        if (isBasket)
        {
            activeTween = heldObject.DOLocalMove(new Vector3(0, -.3f, 0), .4f);
            heldObject.DOLocalRotate(new Vector3(0, 120, 0), .4f, RotateMode.Fast);
        } else
        {
            activeTween = heldObject.DOLocalMove(Vector3.zero, .3f);
            heldObject.DOLocalRotate(new Vector3(-90, 0, 0), .3f, RotateMode.Fast);
        }
    }

    public void PlaceHeld() {
        if (activeTween != null && activeTween.active) return;

        if (heldObject.GetComponent<Basket>())
        {
            var rotationPoint = placeGuide.transform.rotation.eulerAngles
                .Add(y: 90)
                .Add(camera2.transform.rotation.eulerAngles.With(x: 0));
            heldObject.DOMove(placeGuide.transform.position, .3f).onComplete = FinalisePlaceBasket;
            activeTween = heldObject.DORotate(rotationPoint, .3f);
        } else
        {
            var rotationPoint = placeGuide.transform.rotation.eulerAngles
                .Add(x: -90)
                .Add(camera2.transform.rotation.eulerAngles.With(x: 0));
            heldObject.DOMove(placeGuide.transform.position, .1f).onComplete = FinalisePlace;
            activeTween = heldObject.DORotate(rotationPoint, .1f);
        }
    }

    private void FinalisePlace()
    {
        holding = false;
        heldObject.parent = heldObject.GetComponent<IInteractable>().getGroup().transform;
        heldObject.GetComponent<Rigidbody>().useGravity = true;
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.gameObject.layer = interactableLayer;
        heldObject = null;
    }

    private void FinalisePlaceBasket()
    {
        holding = false;
        heldObject.parent = null;
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
