using System;

[Serializable]
public class IngredientBottleData
{
    public string IngredientName;
    public SVector3 BottlePosition;
    public SQuaternion BottleRotation;
    public string BottlePrefabName;

    public IngredientBottleData(string ingredientName, SVector3 bottlePosition, SQuaternion bottleRotation, string bottlePrefabIndex)
    {
        IngredientName = ingredientName;
        BottlePosition = bottlePosition;
        BottleRotation = bottleRotation;
        BottlePrefabName = bottlePrefabIndex;
    }
}
