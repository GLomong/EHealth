using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Transform spawnPoint;   // assegnalo dalla City


    void Start()
    {
        Debug.Log("Sono partitooo");
        Debug.Log("Timescale: " + Time.timeScale);
        
        rb = GetComponent<Rigidbody2D>();
        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;   // <-- CORRETTO
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }
}