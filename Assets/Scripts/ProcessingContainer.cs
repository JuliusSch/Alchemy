using System.Collections.Generic;
using UnityEngine;

public abstract class ProcessingContainer : IngredientContainer, IInteractable {

    public LinkedList<IngredientInstance> ingredients = new LinkedList<IngredientInstance>();
    public List<IngredientSO> startingContents;
    public List<RecipeSO> recipes;
    public int maxIngredientCount = 1;

    public void Start() {
        foreach (IngredientSO ing in startingContents) {
            if (ingredients.Count < maxIngredientCount) LoadFromSO(ing);
        }
        group = null;
    }

    public virtual void Update() {
        Full = !(ingredients.Count < maxIngredientCount);
        SemiFull = ingredients.Count > 0;
    }

    protected override void LoadFromSO(IngredientSO newIng) {
        IngredientInstance ingredient = new IngredientInstance(newIng, Instantiate(newIng.prefabModel, transform), 1);
        ingredient.model.transform.localRotation = Quaternion.identity;
        ingredients.AddFirst(ingredient);
    }

    public override bool CanAccept(IngredientSO.IngredientType type) {
        if (type == IngredientSO.IngredientType.BASE_FLUID) return false;
        return true;
    }

    public override IngredientSO.IngredientType GetIngredientType() {
        if (SemiFull) return ingredients.First.Value.data.type;
        return IngredientSO.IngredientType.STANDARD;
    }

    public override IngredientSO Empty() {
        if (ingredients.Count > 0) {
            IngredientInstance emptiedIng = ingredients.First.Value;
            ingredients.RemoveFirst();
            IngredientSO returnIng = emptiedIng.data;
            Destroy(emptiedIng.model);
            return returnIng;
        }
        return null;
    }

    public override bool Fill(IngredientSO newIng) {
        LoadFromSO(newIng);
        return true;
    }

    public override void PrimaryInteraction(Transform heldObject, PickUp pickUpScript) {
        if (heldObject == null) return;
        IInteractable interactableObject = heldObject.GetComponent<IInteractable>();
        IngredientContainer ingredientBottle = heldObject.GetComponent<IngredientContainer>();
        if (ingredientBottle) {
            if (ingredientBottle.Full && !Full && CanAccept(ingredientBottle.GetIngredientType())) Fill(ingredientBottle.Empty());
            if (!ingredientBottle.Full && SemiFull && ingredientBottle.CanAccept(GetIngredientType())) ingredientBottle.Fill(Empty());
        }
    }

    public new bool Interact(string key) {
        if (key == "e") craft();
        return true;
    }

    protected virtual void craft() {
        RecipeSO recipeToCraft = getValidRecipe();
        if (recipeToCraft) {
            while (ingredients.Count > 0) Empty();
            foreach (IngredientSO output in recipeToCraft.outputs) Fill(output);
        }
    }

    protected virtual RecipeSO getValidRecipe() {
        RecipeSO returnedRecipe;
        bool match;
        foreach (RecipeSO recipe in recipes) {
            returnedRecipe = recipe;
            if (ingredients.Count != recipe.inputs.Count) break;
            match = true;
            foreach (IngredientInstance ing in ingredients) {
                if (!recipe.inputs.Contains(ing.data)) match = false;
            }
            if (match) return returnedRecipe;
        }
        return null;
    }

}
