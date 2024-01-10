using UnityEngine;

[CreateAssetMenu(menuName = "Ingredient")]
public class IngredientSO : ScriptableObject {

    public GameObject prefabModel;
    public int amount;
    public const int NUMBER_OF_ASPECTS = 12;
    public string ingredientName, lore, incName, incLore;
    public bool playerDiscovered;
    public int shopValue;
    public IngredientType type = IngredientType.STANDARD;

    [Range(0, 7)] public int[] baseAspectValues = new int[NUMBER_OF_ASPECTS];
    
    public enum Aspect {
        WATER, FIRE, EARTH, ORDER, DECAY, LIFE, CONSCIOUSNESS, ELDRITCH, FORTUNE, ten, eleven, twelve
    }

    public enum IngredientType {
        BASE_FLUID, STANDARD, MULTIPLIER, INVERTER, SEED
    }
}
