using UnityEngine;

public class PlayerMarketToCity: MonoBehaviour
{
    public Transform spawnFromMarket;
    public GameObject player;

    void Start()
    {
        // controlla se arriva dal Market
        if (PlayerPrefs.GetInt("ReturnFromMarket", 0) == 1)
        {
            PlayerPrefs.SetInt("ReturnFromMarket", 0); // reset

            // sposta il personaggio esattamente nel punto desiderato
            player.transform.position = spawnFromMarket.position;
        }
    }
}

