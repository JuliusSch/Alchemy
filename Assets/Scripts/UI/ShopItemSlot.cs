using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemSlot : MonoBehaviour, IInteractable {

    private IInteractable contents;
    private Transform contentsTransform;
    [SerializeField] private GameObject bottlePrefab, zoomTarget;
    private float time;
    private bool boolMouseOver;
    private Vector3 startPos;
    [SerializeField] private TurtleMerchant turtleScript;
    private Tween tween;

    public void AddContents(IngredientSO ing) {
        IngredientBottle bottle = Instantiate(bottlePrefab, transform).GetComponent<IngredientBottle>();
        bottle.Fill(ing);
        contents = bottle.GetComponent<IInteractable>();
        contentsTransform = bottle.transform;

        bottle.transform.parent = transform;
        bottle.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        bottle.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        bottle.transform.localPosition = Vector3.zero;
        bottle.transform.localRotation = Quaternion.identity;
        bottle.transform.Rotate(-90f, 0, -90f);
    }

    public IInteractable RemoveContents() {
        contentsTransform.transform.parent = null;
        contentsTransform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        contentsTransform.gameObject.GetComponent<Rigidbody>().detectCollisions = true;

        contentsTransform = null;
        IInteractable temp = contents;
        contents = null;
        return temp;
    }

    public void AddContents(SeedSO seed) {
        //contents = Instantiate(seed.prefabModel, transform).GetComponent<IInteractable>();
    }

    public void PrimaryInteraction(Transform heldObject, PickUp pickUpScript) {
        if (turtleScript.CanBuy(contents))
            turtleScript.Buy(this);
    }

    public GameObject getGroup() {
        throw new System.NotImplementedException();
    }

    public bool Interact(string key) {
        if (key == "e") {
            if (turtleScript.CanBuy(contents)) {
                turtleScript.Buy(this);
                //  remove from slot
                return true;
            }
        }
        return false;
    }

    public void MouseOver() {
        if (!contentsTransform) return;
        boolMouseOver = true;
        if (tween != null)
        {
            tween = contentsTransform.DOMove(zoomTarget.transform.position + zoomTarget.transform.forward * .5f, .5f);
        }
    }

    void Start() {
        gameObject.layer = 9;
    }

    void Update() {
        if (!contentsTransform) return;
        //if (contentsTransform.localPosition == Vector3.zero) time = 0;
        //if (contentsTransform.localPosition == zoomTarget.transform.position + zoomTarget.transform.forward * .5f) time = 0;
        //if (boolMouseOver) time = .2f;
        //else time -= Time.deltaTime;
        if (boolMouseOver == false) {
            contentsTransform.localPosition = Vector3.Lerp(Vector3.zero, contentsTransform.localPosition, time * 8f);
            //time = 0;
            startPos = contentsTransform.position;
        }
        if (boolMouseOver == true) boolMouseOver = false;
    }
}
