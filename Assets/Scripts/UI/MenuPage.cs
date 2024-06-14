using TMPro;

public class MenuPage : BookPage
{
    public override void FillContents()
    {
    }

    public override void FillIncompleteContents()
    {
    }

    public override void Initialise()
    {
        pageNoField.GetComponent<TMP_Text>().text = pageNo.ToString();
    }
}
