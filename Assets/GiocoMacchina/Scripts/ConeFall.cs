using UnityEngine;
using System.Collections;

public class ConeFall : MonoBehaviour
{
    public float duration = 0.4f;      // quanto ci mette a uscire
    public float destroyDelay = 0.5f;  // dopo quanto lo distrugge

    private bool isFalling = false;
    private Coroutine fallRoutine;

    // direction: +1 = verso destra, -1 = verso sinistra
    public void Fall(float direction)
    {
        // Se il gioco è finito → NON far partire la caduta
        if (GameOverUI.gameEnded)
            return;

        if (isFalling) return;

        fallRoutine = StartCoroutine(FallRoutine(direction));
    }

    private IEnumerator FallRoutine(float direction)
    {
        isFalling = true;

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("Nessuna Camera con tag MainCamera trovata!");
            yield break;
        }

        Vector3 startPos = transform.position;

        // posizione del cono in coordinate viewport (0-1)
        Vector3 viewportPos = cam.WorldToViewportPoint(startPos);

        // lo spingiamo fuori dallo schermo
        float targetViewportX = (direction > 0) ? 1.2f : -0.2f;
        viewportPos.x = targetViewportX;

        // abbassiamo leggermente
        viewportPos.y = Mathf.Clamp01(viewportPos.y - 0.2f);

        Vector3 endPos = cam.ViewportToWorldPoint(viewportPos);

        // disattivo il collider così la macchina non ci sbatte più
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        float t = 0f;
        while (t < duration)
        {
            // Se il gioco finisce DURANTE la caduta → stop immediato
            if (GameOverUI.gameEnded)
            {
                Destroy(gameObject);
                yield break;
            }

            float lerpT = t / duration;
            transform.position = Vector3.Lerp(startPos, endPos, lerpT);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        // Se il gioco finisce qui, elimina subito il cono
        if (GameOverUI.gameEnded)
        {
            Destroy(gameObject);
            yield break;
        }

        // aspetta ancora un po' e poi distruggi
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
