using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour, ISaveable {

    public float mouseSensitivity = 200f;
    public Transform playerBody;

    private float mouseX, mouseY, xRotation = 0f;
    private float _currentMouseSensitivity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        mouseX = Input.GetAxis("Mouse X") * _currentMouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * _currentMouseSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void SetMouseOver(bool val)
    {
        if (val)
            _currentMouseSensitivity = mouseSensitivity * 0.8f;
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
