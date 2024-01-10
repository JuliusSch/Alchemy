using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Concoction")]
public class ConcoctionSO : ScriptableObject {

    public GameObject prefabModel;
    public int goldValue;
    public bool playerDiscovered = false;
    [TextArea] public string concoctionName, lore, incName, incIng, incLore;
    public IngredientSO[] ingredients;

    [Range(0, 7)] public int[] recipe = new int[IngredientSO.NUMBER_OF_ASPECTS];

    public string getIngredientsList() {
        string list = "";
        foreach (IngredientSO ing in ingredients) {
            list = list + ing.name + "\n";
        }
        return list;
    }
}
