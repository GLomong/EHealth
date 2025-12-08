using UnityEngine;

public class PlayerDiscoToMappa2 : MonoBehaviour
{
    public Transform spawnFromDisco;
    public GameObject player;

    void Start()
    {
        // controlla se arriva dal Market
        if (PlayerPrefs.GetInt("ReturnFromDisco", 0) == 1)
        {
            PlayerPrefs.SetInt("ReturnFromDisco", 0); // reset

            // sposta il personaggio esattamente nel punto desiderato
            player.transform.position = spawnFromDisco.position;
        }
    }
}
