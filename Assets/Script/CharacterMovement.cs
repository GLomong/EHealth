using UnityEngine;
using UnityEngine.InputSystem; // necessario per il nuovo input system

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    // Questo metodo viene chiamato dal PlayerInput quando premi un tasto
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
