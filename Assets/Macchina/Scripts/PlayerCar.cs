using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCar : MonoBehaviour
{
    public float speed = 5f; //quanto veloce si muove orizzontalmente
    public float limitX = 2.5f; //limiti laterali strada
    public bool canMove = true; //variabile per bloccare la macchina quando appare la notifica

    void Update()
    {
        if (!canMove) return; //se falsa, macchina non si muove

        //Movimento orizzontale: -1 sx, 0 fermo, +1 dx
        float inputX = 0f;

        var kb = Keyboard.current;
        if (kb != null) //controlla che esista una tastiera
        {
            if (kb.leftArrowKey.isPressed || kb.aKey.isPressed) //per andare a sx
                inputX = -1f;
            else if (kb.rightArrowKey.isPressed || kb.dKey.isPressed) //per andare a dx
                inputX = 1f;
        }

        //La macchina si muove solo lungo x
        Vector3 move = new Vector3(inputX * speed * Time.deltaTime, 0f, 0f);
        transform.position += move;

        float clampedX = Mathf.Clamp(transform.position.x, -limitX, limitX); //forzo la x dentro limiti
        transform.position = new Vector3(clampedX, transform.position.y, 0f); //cambia x, no y e no z
    }

    private void OnTriggerEnter2D(Collider2D collision) //quando l'auto colpisce ostacolo
    {
        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Hai preso un ostacolo!");
            
        }
    }
   
}   
