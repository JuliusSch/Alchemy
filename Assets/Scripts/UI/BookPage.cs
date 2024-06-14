using UnityEngine;
using UnityEngine.UI;

public abstract class BookPage : MonoBehaviour {

    public int pageNo;

    [SerializeField] protected Image radar, incompleteRadar;
    [SerializeField] protected GameObject pageNoField;
    [SerializeField] protected Sprite pageLeft, pageRight;

    public abstract void FillContents();

    public abstract void FillIncompleteContents();

    public abstract void Initialise();
}
