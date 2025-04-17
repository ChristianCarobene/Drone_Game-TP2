using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Rigidbody rigidbody;
    [SerializeField] private float speed = 10;
    [SerializeField] private float maxSpeed = 10;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate() // Interacciones con las Fisicas
    {
        Movement();
    }
    private void Movement()
    {
        float x = Input.GetAxis("Horizontal"); // -1 a 1 - AD
        float y = 0;
        float z = Input.GetAxis("Vertical");   // -1 a 1 - SW
        if (Input.GetKey(KeyCode.Space))
        {
            y = 1;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            y = -1;
        }
        if (MathF.Abs(z) < 0.01f && MathF.Abs(x) < 0.01f && MathF.Abs(y) < 0.01f)

        {
            return;
        }
        Vector3 forward = transform.forward;
        Vector3 right = transform.right; // El OCR podría haber omitido el "=" aquí
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 direction = (forward * z + right * x + Vector3.up * y).normalized; // Añadidos "*" que parecían faltar
        Debug.Log("VectorForward: " + forward + "\nVectorRight: " + right + "\nDirection: " + direction);
        rigidbody.AddForce(direction * speed, ForceMode.Acceleration);
        Vector3 velocity = rigidbody.velocity;
        if (velocity.magnitude > maxSpeed) // Asume que maxSpeed está definida en otra parte
        {
            rigidbody.velocity = velocity.normalized * maxSpeed;
        }

    }
}
