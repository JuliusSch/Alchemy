using UnityEngine;

public interface ICarryable {

    void pickup();
    void putDown();

    GameObject gameObject { get; }
}

public interface IInteractable {

    void PrimaryInteraction(Transform heldObject, PickUp pickUpScript);
    bool Interact(string key);

    void MouseOver();
    GameObject getGroup();

    GameObject gameObject { get; }

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