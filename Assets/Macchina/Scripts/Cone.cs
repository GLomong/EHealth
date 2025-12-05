using UnityEngine;

public class Cone : MonoBehaviour
{
    public float speed = 3f; // velocità di discesa

    void Update()
    {
        // Se il gioco è finito → NON muovere più il cono
        if (!GameOverUI.gameEnded)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        // MA continuiamo comunque a cancellare il cono se esce dallo schermo
        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }
}

