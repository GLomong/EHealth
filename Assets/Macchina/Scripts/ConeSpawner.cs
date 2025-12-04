using UnityEngine;

public class ConeSpawner : MonoBehaviour
{
    public GameObject conePrefab;
    public float spawnInterval = 5f; //ogni quanti secondi deve apparire un cono
    public float minX = -2.5f; //limite sinistro e destro dove il cono può nascere
    public float maxX = 2.5f;

    void Start() 
    {
        StartCoroutine(SpawnRoutine()); //ciclo che continua nel tempo
    }

    System.Collections.IEnumerator SpawnRoutine() //ciclo che genera i coni
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnCone();
        }
    }

    void SpawnCone() //ogni 5 secondi nasce un cono in una posizione casuale orizzontalmente, sempre alla stessa altezza
    {
        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, transform.position.y, 0);
        Instantiate(conePrefab, pos, Quaternion.identity);
    }
}
