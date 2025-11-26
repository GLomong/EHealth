using UnityEngine;
using System.Collections;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager Instance;

    [Header("Params")]
    public float waterDuration = 15f;           // durata del booster acqua
    public float waterPitch = 0.95f;            // leggera decelerazione
    public float waterPerfectBonus = 0.03f;     // aumenta perfectWindow
    public float waterInstantStamina = 80f;     // energia immediata (< di candy)

    public float candyInstantStamina = 90f;      // energia immediata
    public float candyNeutraDuration = 3f;        // periodo neutro iniziale 
    public float candyBoostPitch = 1.25f;       // aumento velocità
    public float candyBoostDuration = 10f;       // periodo successivo più veloce
    public float candyPerfectPenalty = -0.02f;  // riduce perfectWindow (più difficile)
    public float candyDecayIncrease = 5f;        // aumento decay durante candy

    // internals
    private bool runningWater = false;
    private bool runningCandy = false;

    void Awake() { Instance = this; }

    public void ApplyWater()
    {
        if (runningWater || runningCandy) return;
        StartCoroutine(WaterCoroutine());
    }

    public void ApplyCandy()
    {
        if (runningCandy || runningWater) return;
        StartCoroutine(CandyCoroutine());
    }

    IEnumerator WaterCoroutine()
    {
        runningWater = true;
        var judge = Judge.Instance;
        var spawner = FindFirstObjectByType<NoteSpawner>();

        // controlli di sicurezza
        if (spawner == null || judge == null)
        {
            Debug.LogWarning("[BoosterManager] Water aborted: missing Judge or NoteSpawner.");
            runningWater = false;
            yield break;
        }

        if (spawner.musicSource == null)
        {
            Debug.LogWarning("[BoosterManager] Water aborted: missing musicSource on NoteSpawner.");
            runningWater = false;
            yield break;
        }

        if (StaminaManager.Instance == null)
        {
            Debug.LogWarning("[BoosterManager] Water aborted: missing StaminaManager.");
            runningWater = false;
            yield break;
        }

        // salva valori originali localmente
        float savedOriginalPitch = spawner.musicSource.pitch;
        float savedOriginalPerfectWindow = judge.perfectWindow;

        try 
        {
            // Energia immediata da acqua
            StaminaManager.Instance.AddStamina(waterInstantStamina);

            // apply
            spawner.musicSource.pitch = waterPitch;
            judge.perfectWindow = Mathf.Max(0.01f, savedOriginalPerfectWindow + waterPerfectBonus);

            // durata
            float end = Time.time + waterDuration;
            while (Time.time < end)
            {
                if (spawner.musicSource == null) break;

                // Se stamina è 0 → interrompi prima del tempo
                if (StaminaManager.Instance.stamina <= 0f)
                    break;

                yield return null;
            }
        }
        finally
        {
            // restore garantito
            if (spawner != null && spawner.musicSource != null)
            {
                spawner.musicSource.pitch = savedOriginalPitch;
            }

            if (judge != null)
            {
                judge.perfectWindow = savedOriginalPerfectWindow;
            }

            runningWater = false;
        }
    }

    IEnumerator CandyCoroutine()
    {
        runningCandy = true;
        var judge = Judge.Instance;
        var spawner = FindFirstObjectByType<NoteSpawner>();
        var stamina = StaminaManager.Instance;

        // controlli di sicurezza
        if (spawner == null || judge == null)
        {
            Debug.LogWarning("[BoosterManager] Water aborted: missing Judge or NoteSpawner.");
            runningWater = false;
            yield break;
        }

        if (spawner.musicSource == null)
        {
            Debug.LogWarning("[BoosterManager] Water aborted: missing musicSource on NoteSpawner.");
            runningWater = false;
            yield break;
        }

        if (stamina == null)
        {
            Debug.LogWarning("[BoosterManager] Water aborted: missing StaminaManager.");
            runningWater = false;
            yield break;
        }

        // salva valori originali 
        float savedOriginalPitch = spawner.musicSource.pitch;
        float savedOriginalPerfectWindow = judge.perfectWindow;
        float savedOriginalDecay = stamina.decayPerSecond;

        try 
        {
            // FASE 1: Energia immediata da candy ma i parametri rimangono invariati per ora 
            StaminaManager.Instance.AddStamina(candyInstantStamina);
            spawner.musicSource.pitch = savedOriginalPitch;
            judge.perfectWindow = savedOriginalPerfectWindow;
            stamina.decayPerSecond = savedOriginalDecay;

            // FASE 2: neutra per 5 secondi 
            float neutralEnd = Time.time + candyNeutraDuration;
            while (Time.time < neutralEnd)
            {
                // Se stamina è 0 → interrompi prima del tempo
                if (stamina.stamina <= 0f)
                    break;

                yield return null;
            }

            // FASE 3: aumento difficoltà per 10 secondi
            spawner.musicSource.pitch = candyBoostPitch;
            judge.perfectWindow = Mathf.Max(0.01f, savedOriginalPerfectWindow + candyPerfectPenalty);
            stamina.decayPerSecond = savedOriginalDecay + candyDecayIncrease;

            float boostEnd = Time.time + candyBoostDuration;
            while (Time.time < boostEnd)
            {
                // Se stamina è 0 → interrompi prima del tempo
                if (stamina.stamina <= 0f)
                    break;

                yield return null;
            }
            
        }
        finally
        {
            // restore garantito
            if (spawner != null && spawner.musicSource != null)
            {
                spawner.musicSource.pitch = savedOriginalPitch;
            }

            if (judge != null)
            {
                judge.perfectWindow = savedOriginalPerfectWindow;
            }

            if (stamina != null)
            {
                stamina.decayPerSecond = savedOriginalDecay;
            }

            runningCandy = false;
        }
    }
}

