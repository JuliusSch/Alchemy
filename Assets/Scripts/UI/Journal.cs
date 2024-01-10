using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour {

    [SerializeField] private bool isOpen;
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private GameObject camera2;
    private MouseLook mouseMoveScript;

    private void Start() {
        mouseMoveScript = camera2.GetComponent<MouseLook>();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        //if (Input.GetKeyDown(KeyCode.Tab)) isOpen = !isOpen;
        //if (isOpen) setOpen();
        //else setClosed();
    }

    private void setOpen() {
        UIPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        //mouseMoveScript.setInMenu(true);
    }

    private void setClosed() {
        UIPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        //mouseMoveScript.setInMenu(false);
    }
}
