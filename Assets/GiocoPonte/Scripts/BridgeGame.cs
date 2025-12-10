using UnityEngine;
using UnityEngine.UI;


public class BridgeGame : MonoBehaviour
{
    public Transform player;
    public float stepDistance = 1f;
    public int totalPlanks = 20;

    private int currentPosition = 0;

    [Header("Probabilità caduta")]
    [Range(0f, 1f)] public float fallChance2 = 0.3f;
    [Range(0f, 1f)] public float fallChance3 = 0.6f;

    public Button b1, b2, b3;

    public void StartGame()
    {
        Debug.Log("Il gioco del ponte è iniziato!");

        currentPosition = 0;

        // abilita i bottoni
        b1.interactable = true;
        b2.interactable = true;
        b3.interactable = true;
    }



    public void Jump1()
    {
        MovePlayer(1);
    }

    public void Jump2()
    {
        if (Random.value < fallChance2)
        {
            PlayerFalls();
        }
        else MovePlayer(2);
    }

    public void Jump3()
    {
        if (Random.value < fallChance3)
        {
            PlayerFalls();
        }
        else MovePlayer(3);
    }

    void MovePlayer(int steps)
    {
        currentPosition += steps;

        // fine ponte = vittoria
        if (currentPosition >= totalPlanks)
        {
            Debug.Log("Hai attraversato il ponte!");
            return;
        }

        // muove il personaggio
        Vector3 pos = player.position;
        pos.x += steps * stepDistance;
        player.position = pos;
    }

    void PlayerFalls()
    {
        Debug.Log("SEI CADUTO! Torni all'inizio...");
        currentPosition = 0;

        Vector3 pos = player.position;
        pos.x = 0;
        player.position = pos;
    }
}