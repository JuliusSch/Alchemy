using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class IngredientPage : BookPage {

    [SerializeField] private IngredientSO ingredient;
    [SerializeField] private GameObject nameField, loreField;

    public override void Initialise()
    {
        if (ingredient) {
            /*if (ingredient.playerDiscovered) */FillContents();
            //else fillIncompleteContents();
        }
        if (pageNo % 2 == 1) gameObject.GetComponent<Image>().sprite = pageLeft;
        else gameObject.GetComponent<Image>().sprite = pageRight;
        pageNoField.GetComponent<TMP_Text>().text = pageNo.ToString();
    }

    public override void FillContents() {
        ingredient.playerDiscovered = true;
        nameField.GetComponent<TMP_Text>().text = ingredient.ingredientName;
        loreField.GetComponent<TMP_Text>().text = ingredient.lore;
        if (radar) radar.GetComponent<JournalChart>().updateChart(ingredient.baseAspectValues);
    }

    public override void FillIncompleteContents() {
        ingredient.playerDiscovered = false;
        nameField.GetComponent<TMP_Text>().text = ingredient.incName;
        loreField.GetComponent<TMP_Text>().text = ingredient.incLore;
    }
}
