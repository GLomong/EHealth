using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro; // serve per TextMeshPro UI

public class StaminaManager : MonoBehaviour
{
    public static StaminaManager Instance;

    [Header("UI")]
    public Image staminaFill; // Image type = Filled
    public GameObject boosterChoicePanel; // panel with buttons

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float stamina;
    public float decayPerSecond = 8f;     // diminuisce lentamente nel tempo
    public float missPenalty = 10f;        // quanto scende alla miss
    public TextMeshProUGUI staminaText; 
    public float textVisibleDuration = 0.5f; // quanto tempo rimane visibile il messaggio
    public float popScale = 1.5f;       // grandezza del pop
    public float popDuration = 0.15f;   // durata del pop
    public float fadeDuration = 0.25f; // durata del fade out

    [Header("Refs")]
    public AudioSource musicSource;       // Reference to main music source
    public NoteSpawner spawner;           // optional (for disabling spawn)

    public event Action OnStaminaDepleted; // hook if needed
    private Coroutine displayCoroutine;
    private Vector3 originalScale;
    private Color originalColor;

    void Awake() { Instance = this; }

    void Start()
    {
        stamina = maxStamina;
        originalScale = staminaText.transform.localScale;
        originalColor = staminaText.color;
        staminaText.gameObject.SetActive(false); // testo nascosto all’inizio
        UpdateUI();
        boosterChoicePanel.SetActive(false);
    }

    void Update()
    {
        // decadi stamina solo se musica in riproduzione
        if (musicSource != null && musicSource.isPlaying)
        {
            stamina -= decayPerSecond * Time.deltaTime;
            if (stamina <= 0f)
            {
                stamina = 0f;
                OnDepleted();
            }
            UpdateUI();
        }
    }

    public void ReduceStamina(float amount)
    {
        stamina = Mathf.Max(0f, stamina - amount);
        UpdateUI();
        if (stamina <= 0f) OnDepleted();
    }

    public void AddStamina(float amount)
    {
        stamina = Mathf.Min(maxStamina, stamina + amount);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (staminaFill) staminaFill.fillAmount = stamina / maxStamina;
    }

    void OnDepleted()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            // pausa musica e gioco
            musicSource.Pause();
            // se vuoi stoppare entirely: spawner.enabled = false; ma Pause is fine
            boosterChoicePanel.SetActive(true);
            OnStaminaDepleted?.Invoke();
        }
    }

    // chiamato dai bottoni UI
    public void ChooseWater()
    {
        boosterChoicePanel.SetActive(false);
        ShowStaminaText((int)BoosterManager.Instance.waterInstantStamina);
        BoosterManager.Instance.ApplyWater();
        musicSource.UnPause();
    }
    public void ChooseCandy()
    {
        boosterChoicePanel.SetActive(false);
        ShowStaminaText((int)BoosterManager.Instance.candyInstantStamina);
        BoosterManager.Instance.ApplyCandy();
        musicSource.UnPause();
    }
    public void CancelChoice()
    {
        boosterChoicePanel.SetActive(false);
        // rilascia comunque il gioco (o tieni fermo finché non sceglie)
        musicSource.UnPause();
    }
    private void ShowStaminaText(int amount)
    {
        // ferma eventuale coroutine in corso
        if (displayCoroutine != null)
            StopCoroutine(displayCoroutine);

        staminaText.text = $"+{amount} Stamina!";
        displayCoroutine = StartCoroutine(DisplayCoroutine());
    }
    private IEnumerator DisplayCoroutine()
    {
        staminaText.gameObject.SetActive(true);    // mostra testo

        // Reset per sicurezza
        staminaText.transform.localScale = originalScale;
        staminaText.color = originalColor;
        // FASE 1: Pop-in scale animation (ingrandisce → torna normale)
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / popDuration;
            float scale = Mathf.Lerp(1f, popScale, t);
            staminaText.transform.localScale = originalScale * scale;
            yield return null;
        }

        // torna alla scala normale
        staminaText.transform.localScale = originalScale;

        // FASE 2: Rimane visibile
        yield return new WaitForSeconds(textVisibleDuration);

        // FASE 3: Fade out
        t = 0;
        Color startColor = staminaText.color;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            startColor.a = Mathf.Lerp(1f, 0f, t);
            staminaText.color = startColor;
            yield return null;
        }

        // FASE 4: Nascondi completamente
        staminaText.gameObject.SetActive(false);
    }
}

