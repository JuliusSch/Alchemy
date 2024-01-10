using UnityEngine;
using TMPro;

public abstract class IngredientContainer : MonoBehaviour, IInteractable {

    public IngredientSO Ingredient;
    public IngredientInstance IngredientInst;
    public bool Full, SemiFull;
    [SerializeField] private TMP_Text _label;

    protected GameObject group;

    public struct IngredientInstance {
        public IngredientSO data;
        public GameObject model;
        public int quantity;

        public IngredientInstance(IngredientSO data, GameObject model, int quantity) {
            this.data = data;
            this.model = model;
            this.quantity = quantity;
        }
    }

    public GameObject getGroup() {
        return group;
    }

    public void Awake() {
        _label = GetComponentInChildren<TMP_Text>();
        if (Ingredient) LoadFromSO(Ingredient);
        gameObject.layer = 9;
        group = GameObject.Find("IngredientBottles");
        if (!group) Debug.Log("no group found!");
    }

    protected virtual void LoadFromSO(IngredientSO newIng) {
        IngredientInst = new IngredientInstance(newIng, Instantiate(newIng.prefabModel, transform), 1);
        IngredientInst.model.transform.localRotation = Quaternion.identity;
        Full = true;
        SemiFull = true;
        _label.text = IngredientInst.data.ingredientName;
    }

    public virtual IngredientSO Empty() {
        if (Full) {
            Destroy(IngredientInst.model);
            IngredientSO returnIng = IngredientInst.data;
            Full = false;
            SemiFull = false;
            _label.text = "";
            return returnIng;
        }
        return null;
    }

    public virtual bool Fill(IngredientSO newIng) {
        if (Full) return false;
        if (!newIng) return false;
        LoadFromSO(newIng);
        return true;
    }

    public virtual bool CanAccept(IngredientSO.IngredientType type) {
        return true;
    }

    public virtual IngredientSO.IngredientType GetIngredientType() {
        if (Full) return IngredientInst.data.type;
        return IngredientSO.IngredientType.STANDARD;
    }

    public virtual void PrimaryInteraction(Transform heldObject, PickUp pickUpScript) {
        if (heldObject == null) pickUpScript.PickUpObject(transform.gameObject, this);
    }

    public bool Interact(string key) {
        if (key == "processingContainer") {
            //do
            return true;
        }
        return false;
    }

    public void MouseOver() {
    }
}