using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaxiTrigger : MonoBehaviour
{
    public GameObject player;              // Riferimento al ragazzo
    public GameObject fineGiornoPanel;     // Pannello UI "Fine giorno 1"
    public TMPro.TextMeshProUGUI endDayText; // Testo da mostrare nel pannello
    public float speed = 5f;               // Velocità taxi
    public string nextSceneName = "Città"; // Nome della scena successiva

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && !activated)
        {
            activated = true;

            // 1. Nasconde il ragazzo
            player.SetActive(false);

            // 2. Attiva la camera per seguire il taxi
            CameraFollowClamp camFollow = Camera.main.GetComponent<CameraFollowClamp>();
            if (camFollow != null)
            {
                camFollow.activeFollow = true;
            }

            // 3. Avvia la coroutine che muove il taxi
            StartCoroutine(MoveTaxiOut());
        }
    }

    private IEnumerator MoveTaxiOut()
    {
        // 1. Prepara pannello e testo
        CanvasGroup cg = fineGiornoPanel.GetComponent<CanvasGroup>();
        cg.alpha = 0f; // Assicura che il pannello sia trasparente all'inizio
        fineGiornoPanel.SetActive(true);

        float endAlpha = 0.6f;
        float fadeInDuration = 1f;
        Color textColor = endDayText.color;
        textColor.a = 0f;
        endDayText.color = textColor;
        endDayText.text = "End of Day 1";

        // 2. Il taxi si muove verso destra finché non esce e si avvia il fade in del pannello
        float timer = 0f;

        while (transform.position.x < 23f)
        {
            // Movimento del taxi
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

            // Fade in del pannello e del testo
            if (timer < fadeInDuration)
            {
                cg.alpha = Mathf.Lerp(0f, endAlpha, timer / fadeInDuration);
                textColor.a = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
                endDayText.color = textColor;
                timer += Time.deltaTime;
            }
            yield return null;
        }

        // Assicura che il pannello e il testo siano completamente visibili
        cg.alpha = endAlpha;
        textColor.a = 1f;
        endDayText.color = textColor;

        // 3. Aggiorna il testo con animazione di scrittura
        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            // Fai scomparire il testo
            textColor.a = Mathf.Lerp(1f, 0f, t / fadeInDuration);
            endDayText.color = textColor;
            yield return null;
        }
        // endDayText.text = "Score: " + PlayerPrefs.GetInt("Day1Score", 0);

        endDayText.text = "Goodnight!";
        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            // Fai apparire il testo
            textColor.a = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            endDayText.color = textColor;
            yield return null;
        }
        // Assicura che il testo sia completamente visibile
        textColor.a = 1f;
        endDayText.color = textColor;

        // 4. Aspetta per un certo tempo
        yield return new WaitForSeconds(0.2f);

        // 5. Fade to black
        float initialAlpha = cg.alpha;
        float textInitialAlpha = textColor.a;

        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            // Fai diventare il pannello nero
            cg.alpha = Mathf.Lerp(initialAlpha, 1f, t / fadeInDuration);

            // Fai scomparire il testo
            textColor.a = Mathf.Lerp(textInitialAlpha, 0f, t / fadeInDuration);
            endDayText.color = textColor;

            yield return null;
        }

        // Assicura che sia completamente nero e testo invisibile
        cg.alpha = 1f; // sicuro che sia completamente nero
        textColor.a = 0f;
        endDayText.color = textColor;

        // 5. Carica la scena successiva
        SceneManager.LoadScene(nextSceneName);
    }
}