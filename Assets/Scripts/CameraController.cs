using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivityX = 50f;
    [SerializeField] float mouseSensitivityY = 50f;
    [SerializeField] Transform player = null;

    float xRotation = 0;
    float mouseX;
    float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.unscaledDeltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.unscaledDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}
