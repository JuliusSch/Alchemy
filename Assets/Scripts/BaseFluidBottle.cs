using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFluidBottle : IngredientBottle, IInteractable {

    public override bool Fill(IngredientSO newIng) {
        if (Full) return false;
        if (!newIng) return false;
        if (newIng.type != IngredientSO.IngredientType.BASE_FLUID) return false;
        LoadFromSO(newIng);
        return true;
    }

    public override bool CanAccept(IngredientSO.IngredientType type) {
        return (type == IngredientSO.IngredientType.BASE_FLUID);
    }

}
