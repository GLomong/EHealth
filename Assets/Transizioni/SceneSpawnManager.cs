using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{
    public Transform spawnFromBridge;
    public GameObject player;

    void Start()
    {
        // controlla se arriva dal ponte
        if (PlayerPrefs.GetInt("ReturnFromBridge", 0) == 1)
        {
            PlayerPrefs.SetInt("ReturnFromBridge", 0); // reset

            // sposta il personaggio nel punto desiderato
            player.transform.position = spawnFromBridge.position;
        }
    }
}