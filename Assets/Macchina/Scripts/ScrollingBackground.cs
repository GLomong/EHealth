using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 3f; //velocità dello scorrimento verso il basso
    public float resetPositionY; //posizione Y alla quale lo sfondo è considerato troppo in basso
    public float startPositionY; //posizione Y in cui lo sfondo deve ripartire dopo il reset

    void Update()
    {
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime); //peer far "scorrere" strada sotto macchina

        if (transform.position.y <= resetPositionY) //sfondo sceso troppo in basso
        {
            transform.position = new Vector3(transform.position.x, startPositionY, transform.position.z); //sfondo spostato in alto
        }
    }
}