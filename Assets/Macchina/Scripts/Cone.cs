using UnityEngine;

public class Cone : MonoBehaviour
{
    public float speed = 3f; //velocità con cui il cono scende

    void Update() //per far scorrere i coni verso la macchina
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime); //vector muove verso il basso e deltatime per movim fluido

        if (transform.position.y < -7f) //cono eliminato quando esce sotto lo schermo
        {
            Destroy(gameObject);
        }
    }
}
