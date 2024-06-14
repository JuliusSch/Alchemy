using UnityEngine;

public interface ICarryable {

    void pickup();
    void putDown();

    GameObject gameObject { get; }
}

public interface IInteractable {

    void PrimaryInteraction(Transform heldObject, ItemInteraction pickUpScript);
    bool Interact(string key);

    void MouseOver();
    GameObject getGroup();

    GameObject gameObject { get; }

}

// needed or can use buttons?
public interface IInteractableUI
{
    void Hover();
    void Interact();
}

public interface ISaveable
{
    void Save(SaveData data);
    void Load(SaveData data);
}

public interface IGetTimeNotifications
{
    void Notify(int time);
}