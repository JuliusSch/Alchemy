
public class IngredientBottle : IngredientContainer, IInteractable
{
    public string PrefabName;

    public override bool Fill(IngredientSO newIng) {
        if (Full) return false;
        if (!newIng) return false;
        if (newIng.type == IngredientSO.IngredientType.BASE_FLUID) return false;
        LoadFromSO(newIng);
        return true;
    }
}
