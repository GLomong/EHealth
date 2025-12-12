using UnityEngine;

public class ConeSpawner : MonoBehaviour
{
    public GameObject conePrefab;
    public GiocoMan giocoMan; 
    public float spawnInterval = 5f; 
    public float minX = -2.5f;
    public float maxX = 2.5f;

    void Start() 
    {
        StartCoroutine(SpawnRoutine());
    }

    System.Collections.IEnumerator SpawnRoutine()
    {
        // Finché il gioco NON è finito, spawna coni
        while (!GameOverUI.gameEnded)
        {
             if (!StartScreen.gameStarted) 
            yield return null;

            yield return new WaitForSeconds(spawnInterval);
            
            // può succedere che il gioco finisca DURANTE la wait
            if (GameOverUI.gameEnded)
                yield break;

            SpawnCone();
        }
    }

    void SpawnCone()
    {
        // incrementa il totale coni spawnati
        if (giocoMan != null)
            giocoMan.totalSpawnedCones++;

        // Se il gioco è finito per sicurezza NON spawnare
        if (GameOverUI.gameEnded)
            return;

        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, transform.position.y, 0);
        Instantiate(conePrefab, pos, Quaternion.identity);
    }
}

