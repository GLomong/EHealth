using UnityEngine;
using System.Collections;

public class CarShake : MonoBehaviour
{
    private Coroutine shakingRoutine;

    public void Shake(float duration, float angle)
    {
        // Se il gioco è finito, niente tremolio
        if (GameOverUI.gameEnded)
            return;

        // se sta già tremando, fermalo prima
        if (shakingRoutine != null)
            StopCoroutine(shakingRoutine);

        shakingRoutine = StartCoroutine(ShakeRoutine(duration, angle));
    }

    IEnumerator ShakeRoutine(float duration, float angle)
    {
        Quaternion startRot = transform.rotation;

        float t = 0f;
        while (t < duration)
        {
            // Se il gioco finisce durante lo shake → stop immediato
            if (GameOverUI.gameEnded)
            {
                transform.rotation = startRot;
                yield break;
            }

            float rot = Mathf.Sin(t * 50f) * angle;
            transform.rotation = Quaternion.Euler(0, 0, rot);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = startRot;
        shakingRoutine = null;
    }
}




