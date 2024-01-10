using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientObjectManager : MonoBehaviour, ISaveable
{
    private List<IngredientBottle> _ingredientBottles;
    private List<IngredientSO> _ingredientSOs;
    public List<GameObject> BottlePrefabs;

    public void Load(SaveData data)
    {
        _ingredientBottles = new List<IngredientBottle>();
        _ingredientSOs = HelpfulFunctions.FindAssetsByType<IngredientSO>();

        if (data.IngredientBottles == null)
            return;

        foreach (var bottle in data.IngredientBottles)
        {
            var ingredientSO = _ingredientSOs.Where(i => i.ingredientName == bottle.IngredientName).FirstOrDefault();
            var instance = Instantiate(BottlePrefabs.Where(bp => bp.GetComponent<IngredientBottle>().PrefabName == bottle.BottlePrefabName).FirstOrDefault(), bottle.BottlePosition, bottle.BottleRotation, transform).GetComponent<IngredientBottle>();
            _ingredientBottles.Add(instance);
            
            if (ingredientSO != null)
                instance.Fill(ingredientSO);
        }
    }

    public void Save(SaveData data)
    {
        var bottles = GetComponentsInChildren<IngredientBottle>();
        var saveData = bottles.Select(b => new IngredientBottleData(b.IngredientInst.data?.ingredientName, b.transform.position, b.transform.rotation, b.PrefabName));
        data.IngredientBottles = saveData.ToList();
    }

    void Update()
    {
        
    }

    public void CreateIngredientContainer()
    {
        // Create blank and instantiate in world
    }
}
