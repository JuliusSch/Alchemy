using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class Cauldron : ProcessingContainer {

    private bool hasBaseFluid, _isPouring;
    private const int MIN_VAL = 0, MAX_VAL = 7;
    [SerializeField, Range(MIN_VAL, MAX_VAL)] private int[] baseAspectValues = new int[IngredientSO.NUMBER_OF_ASPECTS];  //visible for testing only
    [SerializeField] private GameObject[] aspectValueBars = new GameObject[IngredientSO.NUMBER_OF_ASPECTS];
    [SerializeField] private List<ConcoctionSO> concoctionRecipes;
    [SerializeField] private GameObject liquidModel;
    [SerializeField] private PlayerDataManager dataManager;
    [SerializeField] private Transform _pourPosition;
    public AudioSource BubblingSound, SplashSound;

    public override bool Fill(IngredientSO newIng) {
        if (ingredients.Count >= maxIngredientCount) return false;
        if (newIng.type == IngredientSO.IngredientType.BASE_FLUID) hasBaseFluid = true;
        if (hasBaseFluid && !BubblingSound.isPlaying) BubblingSound.Play();
        LoadFromSO(newIng);
        updateAspectValues(newIng);
        SplashSound.Play();
        //updateDisplay();
        return true;
    }

    public override void Update() {
        Full = !(ingredients.Count < maxIngredientCount);
        SemiFull = ingredients.Count > 0;
        if (hasBaseFluid && !liquidModel.activeInHierarchy) liquidModel.SetActive(true);
        else if (!hasBaseFluid && liquidModel.activeInHierarchy) liquidModel.SetActive(false);
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

    public override void PrimaryInteraction(Transform heldObject, ItemInteraction pickUpScript) {
        if (heldObject == null || _isPouring)
            return;

        _isPouring = true;
        var position = heldObject.localPosition;
        var rotation = heldObject.localEulerAngles;
        heldObject.DORotate(_pourPosition.rotation.eulerAngles.Add(y: -90), .4f);
        heldObject.DOMove(_pourPosition.position, .4f).onComplete = () => {
            base.PrimaryInteraction(heldObject, pickUpScript);
            heldObject.DOLocalMove(position, .4f).onComplete = () => _isPouring = false;
            heldObject.DOLocalRotate(rotation, .4f);
        };
//        ConcoctionContainer concoctionBottle = heldObject.GetComponent<ConcoctionContainer>();
//        if (concoctionBottle) {
//            ConcoctionSO concoction = findConcoction();
//;            if (concoction && !concoctionBottle.filled) {
//                concoctionBottle.fill(concoction);
//                emptyCauldron();
//                dataManager.addConcoction(concoction);
//            }
//        }
    }

    public void EmptyCauldron(ConcoctionSO concoction) {
        Full = false;
        SemiFull = false;
        hasBaseFluid = false;
        BubblingSound.Stop();
        for (int i = 0; i < IngredientSO.NUMBER_OF_ASPECTS; i++) {
            baseAspectValues[i] = 0;
        }
        dataManager.addConcoction(concoction);
        //updateDisplay();
    }

    protected override void craft() {
        ConcoctionSO concoction = FindConcoction();
        if (concoction) { } // do
        }

    public ConcoctionSO FindConcoction() {
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
