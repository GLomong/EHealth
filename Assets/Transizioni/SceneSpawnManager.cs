using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{
    public Transform spawnFromBridge;

    void Start()
    {
        string spawn = PlayerPrefs.GetString("SpawnPoint", "Default");

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (spawn == "FromBridge")
        {
            player.transform.position = spawnFromBridge.position;
        }
        else
        {
            // fallback: lascia il player nella posizione della scena
        }
    }
}