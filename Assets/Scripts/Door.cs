using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

    private bool doorOpen;
    private float time;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float angle = -90;
    [SerializeField] private Vector3 vectorAround = Vector3.back;
    private Quaternion openPos, closePos;
    private Coroutine coroutine;

    void Start() {
        closePos = transform.localRotation;
        openPos = Quaternion.AngleAxis(angle, vectorAround);
    }

    public void PrimaryInteraction(Transform heldObject, ItemInteraction pickUpScript) {
        //do nothing
    }

    public GameObject getGroup() {
        //do nothing
        return null;
    }

    public bool Interact(string key) {
        if (key == "e") {
            doorOpen = !doorOpen;
            if (coroutine != null) StopCoroutine(coroutine);
            if (doorOpen) coroutine = StartCoroutine(Open());
            else coroutine = StartCoroutine(Close());
        }
        return false;
    }

    private IEnumerator Open() {
        time = 0;
        while (time < 1) {
            time += Time.deltaTime * speed;
            transform.Rotate(vectorAround, Time.deltaTime * speed * angle);
            yield return null;
        }
        coroutine = null;
        time = 0;
    }

    private IEnumerator Close() {
        time = 0;
        while (time < 1) {
            time += Time.deltaTime * speed;
            transform.Rotate(vectorAround, Time.deltaTime * speed * -angle);
            yield return null;
        }
        coroutine = null;
        time = 0;
    }

    public void MouseOver() {
        // do nothing
    }
}
