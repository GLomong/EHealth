using UnityEngine;
using UnityEngine.UI;
using System;

public class StaminaManager : MonoBehaviour
{
    public static StaminaManager Instance;

    [Header("UI")]
    public Image staminaFill; // Image type = Filled
    public GameObject boosterChoicePanel; // panel with buttons

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float stamina;
    public float decayPerSecond = 2f;     // diminuisce lentamente nel tempo
    public float missPenalty = 20f;        // quanto scende alla miss

    [Header("Refs")]
    public AudioSource musicSource;       // Reference to main music source
    public NoteSpawner spawner;           // optional (for disabling spawn)

    public event Action OnStaminaDepleted; // hook if needed

    void Awake() { Instance = this; }

    void Start()
    {
        stamina = maxStamina;
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
        BoosterManager.Instance.ApplyWater();
        musicSource.UnPause();
    }

    public void ChooseCandy()
    {
        boosterChoicePanel.SetActive(false);
        BoosterManager.Instance.ApplyCandy();
        musicSource.UnPause();
    }

    public void CancelChoice()
    {
        boosterChoicePanel.SetActive(false);
        // rilascia comunque il gioco (o tieni fermo finché non sceglie)
        musicSource.UnPause();
    }
}

