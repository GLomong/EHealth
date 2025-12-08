using UnityEngine;

public class Cone : MonoBehaviour
{
    public float speed = 3f; // velocità con cui il cono scende

    void Update()
    {
        // se il gioco non è iniziato o è finito, NON muovere il cono
        if (!StartScreen.gameStarted || GameOverUI.gameEnded)
        {
            // ma se è già uscito dallo schermo, lo distruggiamo lo stesso
            if (transform.position.y < -7f)
                Destroy(gameObject);

            return;
        }

        // movimento verso il basso
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // cono eliminato quando esce sotto lo schermo
        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }
}


