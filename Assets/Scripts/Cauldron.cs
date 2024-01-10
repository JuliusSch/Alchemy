using System.Collections.Generic;
using UnityEngine;

public class Cauldron : ProcessingContainer {

    private bool hasBaseFluid;
    private const int MIN_VAL = 0, MAX_VAL = 7;
    [SerializeField, Range(MIN_VAL, MAX_VAL)] private int[] baseAspectValues = new int[IngredientSO.NUMBER_OF_ASPECTS];  //visible for testing only
    [SerializeField] private GameObject[] aspectValueBars = new GameObject[IngredientSO.NUMBER_OF_ASPECTS];
    [SerializeField] private List<ConcoctionSO> concoctionRecipes;
    [SerializeField] private GameObject liquidModel;
    [SerializeField] private PlayerDataManager dataManager;

    public override bool Fill(IngredientSO newIng) {
        if (ingredients.Count >= maxIngredientCount) return false;
        if (newIng.type == IngredientSO.IngredientType.BASE_FLUID) hasBaseFluid = true;
        LoadFromSO(newIng);
        updateAspectValues(newIng);
        updateDisplay();
        return true;
    }

    public override void Update() {
        Full = !(ingredients.Count < maxIngredientCount);
        SemiFull = ingredients.Count > 0;
        if (hasBaseFluid) liquidModel.SetActive(true);
        else liquidModel.SetActive(false);
    }

    public override bool CanAccept(IngredientSO.IngredientType type) {
        if (type == IngredientSO.IngredientType.BASE_FLUID && hasBaseFluid) return false;
        if (type != IngredientSO.IngredientType.BASE_FLUID && !hasBaseFluid) return false;
        return true;
    }

    public override IngredientSO Empty() {
        // empty when completed concoction;
        return null;
    }

    private void updateAspectValues(IngredientSO ing) {
        for (int i = 0; i < IngredientSO.NUMBER_OF_ASPECTS; i++) {
            baseAspectValues[i] += ing.baseAspectValues[i];
            baseAspectValues[i] = Mathf.Clamp(baseAspectValues[i], MIN_VAL, MAX_VAL);
        }
    }

    private void updateDisplay() {
        for(int i = 0; i < IngredientSO.NUMBER_OF_ASPECTS; i++) {
            aspectValueBars[i].transform.localScale = new Vector3(1f, 1f, ((baseAspectValues[i] / (float) (MAX_VAL - MIN_VAL)) + 0.1f));
        }
    }

    public override void PrimaryInteraction(Transform heldObject, PickUp pickUpScript) {
        base.PrimaryInteraction(heldObject, pickUpScript);
        if (heldObject == null) return;
        ConcoctionContainer concoctionBottle = heldObject.GetComponent<ConcoctionContainer>();
        if (concoctionBottle) {
            ConcoctionSO concoction = findConcoction();
            if (concoction && !concoctionBottle.filled) {
                concoctionBottle.fill(concoction);
                emptyCauldron();
                dataManager.addConcoction(concoction);
            }
        }
    }

    private void emptyCauldron() {
        Full = false;
        SemiFull = false;
        hasBaseFluid = false;
        for (int i = 0; i < IngredientSO.NUMBER_OF_ASPECTS; i++) {
            baseAspectValues[i] = 0;
        }
        updateDisplay();
    }

    protected override void craft() {
        ConcoctionSO concoction = findConcoction();
        if (concoction) { } // do
        }

    protected ConcoctionSO findConcoction() {
        foreach (ConcoctionSO concoction in concoctionRecipes) {
            if (getIntArrayAsString(baseAspectValues).Equals(getIntArrayAsString(concoction.recipe))) return concoction;
        }
        return null;
    }

    private string getIntArrayAsString(int[] array) {
        string output = "";
        foreach (int i in array) output = output + array[i];
        return output;
    }
}
