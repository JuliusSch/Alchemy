using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConcoctionPage : BookPage
{
    public ConcoctionSO concoction;
    [SerializeField] private GameObject nameField, ingField, loreField;

    public override void Initialise()
    {
        if (concoction)
        {
            if (concoction.playerDiscovered) FillContents();
            else FillIncompleteContents();
        }
        if (pageNo % 2 == 1) gameObject.GetComponent<Image>().sprite = pageLeft;
        else gameObject.GetComponent<Image>().sprite = pageRight;
        pageNoField.GetComponent<TMP_Text>().text = pageNo.ToString();
    }

    public override void FillContents()
    {
        concoction.playerDiscovered = true;
        nameField.GetComponent<TMP_Text>().text = concoction.concoctionName;
        ingField.GetComponent<TMP_Text>().text = concoction.getIngredientsList();
        loreField.GetComponent<TMP_Text>().text = concoction.lore;
        if (radar) radar.GetComponent<JournalChart>().updateChart(concoction.recipe);
    }

    public override void FillIncompleteContents()
    {
        concoction.playerDiscovered = false;
        nameField.GetComponent<TMP_Text>().text = concoction.incName;
        ingField.GetComponent<TMP_Text>().text = concoction.incIng;
        loreField.GetComponent<TMP_Text>().text = concoction.incLore;
        if (radar) radar.GetComponent<JournalChart>().updateChart(concoction.recipe);
    }
}