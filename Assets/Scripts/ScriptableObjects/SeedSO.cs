using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Seed")]
public class SeedSO : ScriptableObject {

    public GameObject plantModel, plantModelGrowing, plantModelFruiting;
    public int growthTimeSec, fruitTimeSec;
    public IngredientSO produce;

}
