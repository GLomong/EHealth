using UnityEngine;

public class CarArrivalCutscene : MonoBehaviour
{
    public Transform finalPosition;
    public float speed = 3f;

    public GameObject player;
    public Transform playerSpawnPosition;

    void Start()
    {
        bool returnFromMinigame = PlayerPrefs.GetInt("ReturnFromCarMinigame", 0) == 1;

        if (returnFromMinigame)
        {
            PlayerPrefs.SetInt("ReturnFromCarMinigame", 0);

            // nascondi il giocatore
            player.SetActive(false);

            // reset flip e scala
            var sr = GetComponent<SpriteRenderer>();
            sr.flipX = false;
            sr.flipY = false;

            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                Mathf.Abs(transform.localScale.y),
                1
            );

            // porta la macchina fuori dallo schermo
            transform.position = finalPosition.position + new Vector3(0, 6f, 0);

            // 🔥 ruota la macchina verso il market (180°)
            transform.eulerAngles = Vector3.zero;


            StartCoroutine(CarSequence());
        }
        else
        {
            player.SetActive(true);
        }
    }

    System.Collections.IEnumerator CarSequence()
    {
        while (Vector3.Distance(transform.position, finalPosition.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                finalPosition.position,
                speed * Time.deltaTime
            );

            yield return null;
        }

        // appare il giocatore nella posizione giusta
        player.transform.position = playerSpawnPosition.position;
        player.SetActive(true);
    }
}


