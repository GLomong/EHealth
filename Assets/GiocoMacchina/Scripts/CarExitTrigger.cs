using UnityEngine;

public class CarExitTrigger : MonoBehaviour
{
    public float speed = 3f;
    public string minigameScene = "MinigiocoMacchina";

    private bool isActivated = false;
    private SceneFade fader;
    private GameObject player;

    private void Start()
    {
        fader = FindObjectOfType<SceneFade>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Contollo che non esista già un punteggio del giorno corrente 
        int currentDay = TotalGameManager.Instance.CurrentDay;
        string scoreKey = $"Day{currentDay}_CarScore";
        if (PlayerPrefs.HasKey(scoreKey))
        {
            Debug.Log($"[CarExitTrigger] Punteggio per il giorno {currentDay} già esistente: {PlayerPrefs.GetInt(scoreKey)}");
            return;
        }
        if (!isActivated && collision.CompareTag("Player"))
        {
            isActivated = true;
            StartCoroutine(CarSequence());
        }
    }

    private System.Collections.IEnumerator CarSequence()
    {
        // 1. Disattiva sprite del giocatore (entra in macchina)
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;

        // Blocca il controllo del personaggio
        MonoBehaviour movement = player.GetComponent<MonoBehaviour>(); 
        if (movement != null)
            movement.enabled = false;

        // 2. Movimento verso l’alto della macchina
        float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + 1f;

        while (transform.position.y < screenTop)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }


        // 3. Fade + cambio scena
        if (fader != null)
        {
            fader.FadeToScene(minigameScene);
        }
    }
}