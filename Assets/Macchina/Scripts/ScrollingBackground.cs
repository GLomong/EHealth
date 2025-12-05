using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 3f;     // velocità dello scorrimento verso il basso
    public float resetPositionY;       // Y alla quale lo sfondo viene resettato
    public float startPositionY;       // Y di ripartenza dello sfondo dopo il reset

    void Update()
    { 
        // se il gioco è finito, lo sfondo NON si muove più
        if (GameOverUI.gameEnded)
            return;

        // movimento verso il basso (strada che scorre)
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // quando arriva troppo in basso → lo rimettiamo in alto
        if (transform.position.y <= resetPositionY)
        {
            transform.position = new Vector3(
                transform.position.x,
                startPositionY,
                transform.position.z
            );
        }
    }
}
