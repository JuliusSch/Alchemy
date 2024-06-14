using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour, ISaveable {

    public float mouseSensitivity = 200f;
    public Transform playerBody;

    private float mouseX, mouseY, xRotation = 0f;
    private float _currentMouseSensitivity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    PointerEventData m_PointerEventData;
    public GraphicRaycaster m_Raycaster;
    public EventSystem m_EventSystem;


    void FixedUpdate() {
        if (PauseMenu.IsPaused) return;

        mouseX = Input.GetAxis("Mouse X") * _currentMouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * _currentMouseSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // Refactor at some point

        //if (Input.GetButton("Interact"))
        //{
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        m_PointerEventData.position = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            // Check result.distance less than range
            IInteractableUI hit;
            if ((hit = result.gameObject.GetComponent<IInteractableUI>()) != null)
            {
                if (Input.GetButtonUp("Interact"))
                    hit.Interact();
                else
                    hit.Hover();
            }
        }
        //}
    }

    public void SetMouseOver(bool val)
    {
        if (val)
            _currentMouseSensitivity = mouseSensitivity * 0.5f;
        else
            _currentMouseSensitivity = mouseSensitivity;
    }

    public void Save(SaveData data)
    {
        data.CameraRotation = xRotation;
        data.PlayerBodyRotation = playerBody.localRotation;
    }

    public void Load(SaveData data)
    {
        xRotation = data.CameraRotation;
        playerBody.localRotation = data.PlayerBodyRotation;
    }
}
