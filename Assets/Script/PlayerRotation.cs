using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private new Rigidbody rigidbody;
    [Header("Rotation Settings")]
    [SerializeField] private float yawSensitivity = 150f;
    [SerializeField] private float pitchSensitivity = 100f;
    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;
    private float currentYaw = 0f;
    private float currentPitch = 0f;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        currentYaw = transform.eulerAngles.y;
        currentPitch = transform.eulerAngles.x;

        if (currentPitch > 180) currentPitch -= 360;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

    }
    private void FixedUpdate() // Interacciones con las Fisicas
    {
        Rotation();
    }
    private void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * yawSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * pitchSensitivity * Time.deltaTime;

        currentYaw += mouseX;
        currentPitch -= mouseY;

        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f); // Roll es 0 si no lo implementas

    }

}
