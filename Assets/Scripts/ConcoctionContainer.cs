using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConcoctionContainer : MonoBehaviour, IInteractable {

    [SerializeField] private ConcoctionSO contentsData;
    [SerializeField] private ConcoctionInstance contents;
    [SerializeField] private TMP_Text label;
    
    private GameObject group;

    public bool filled;

    public struct ConcoctionInstance {

        public ConcoctionSO data;
        public GameObject model;

        public ConcoctionInstance(ConcoctionSO data, GameObject model) {
            this.data = data;
            this.model = model;
        }
    }

    public GameObject getGroup() {
        return group;
    }

    public void Start() {
        label = GetComponentInChildren<TMP_Text>();
        if (contentsData) loadFromSO(contentsData);
        group = GameObject.Find("ConcoctionBottles");
        gameObject.layer = 9;
        foreach (Transform child in gameObject.transform) {
            child.gameObject.layer = 9;
        }
    }

    private void loadFromSO(ConcoctionSO concoctionData) {
        contents = new ConcoctionInstance(concoctionData, Instantiate(concoctionData.prefabModel, transform));
        contents.model.transform.localRotation = Quaternion.identity;
        label.text = contents.data.concoctionName;
        filled = true;
    }

    public virtual bool empty() {
        if (filled) {
            Destroy(contents.model);
            filled = false;
            label.text = "";
            return true;
        }
        return false;
    }

    public virtual bool fill(ConcoctionSO newData) {
        if (filled) return false;
        loadFromSO(newData);
        return true;
    }

    public void PrimaryInteraction(Transform heldObject, PickUp pickUpScript) {
        if (heldObject == null) pickUpScript.PickUpObject(transform.gameObject, this);
    }

    public bool Interact(string key) {
        return true;
    }

    public void MouseOver() {
        //do nothing
    }
}
