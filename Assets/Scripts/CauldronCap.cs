using UnityEngine;

public class CauldronCap : MonoBehaviour, IInteractable
{
    public Cauldron Cauldron;

    public GameObject getGroup()
    {
        return null;
        //throw new System.NotImplementedException();
    }

    public bool Interact(string key)
    {
        return false;
        //throw new System.NotImplementedException();
    }

    public void MouseOver()
    {
        //throw new System.NotImplementedException();
    }

    public void PrimaryInteraction(Transform heldObject, ItemInteraction pickUpScript)
    {
        if (heldObject == null)
            return;

        ConcoctionContainer concoctionBottle = heldObject.GetComponent<ConcoctionContainer>();
        if (concoctionBottle)
        {
            ConcoctionSO concoction = Cauldron.FindConcoction();
            ; if (concoction && !concoctionBottle.filled)
            {
                concoctionBottle.fill(concoction);
                Cauldron.EmptyCauldron(concoction);
            }
        }
        // if is empty bottle and has valid concoction(?)
        // do cauldron.empty();
    }
}
