using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public GameObject ChoicePanel;
    [Header("Choices Settings")]
    public Button sceltaPositiva;
    public Button sceltaNeutra;
    public Button sceltaNegativa;
    public TMP_Text testoSceltaPositiva;
    public TMP_Text testoSceltaNeutra;
    public TMP_Text testoSceltaNegativa;
    public PunteggiScelte punteggiScelte; 

    [Header("NPC Settings")]
    public string npcName; // Cashier, Influencer, Gentlemen, Friend, ...
    public bool hasScore = true; // false per NPC senza scelte
    
    private bool isDialogueActive = false;
    private int dialogueIndex = 0;
    private bool isTyping = false;
    private bool waitingForChoice = false;
    private string[] currentDialogueLines;
    private bool[] currentAutoProgressLines;
    private ChoiceOption[] currentChoices;


    // Permette al sistema di sapere se puoi parlare
    public bool CanInteract()
    {
        if (waitingForChoice)
            return false;
            
        return !PlayerPrefs.HasKey(GetDailyKey());
    }

    public void Interact()
    {
        // Se il dialogo è già in corso → passa alla prossima linea
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        int currentDay = TotalGameManager.Instance.CurrentDay;

        // Prendi le frasi del giorno corrente
        currentDialogueLines = dialogueData.dialogueLines(currentDay);
        currentAutoProgressLines = dialogueData.autoProgressLines(currentDay);
        currentChoices = dialogueData.GetChoicesForDay(currentDay);

        nameText.text = dialogueData.npcName;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            //dialogueText.text = dialogueData.dialogueLines[dialogueIndex];
            dialogueText.text = currentDialogueLines[dialogueIndex];
            isTyping = false;
        }
        else if (++dialogueIndex < currentDialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }
    
    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in currentDialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if ( currentAutoProgressLines.Length > dialogueIndex &&
            currentAutoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        
        // Segna come parlato oggi SOLO se non ha punteggio
        if (!hasScore)
        {
            PlayerPrefs.SetInt(GetDailyKey(), 1);
            PlayerPrefs.Save();
        }

        // Mostra pannello delle scelte se necessario
        if (ChoicePanel != null)
            SetUpChoices();
            ChoicePanel.SetActive(true);
            waitingForChoice = true;
    }

    string GetDailyKey()
    {
        int day = TotalGameManager.Instance.CurrentDay;

        if (hasScore)
            return $"Day{day}_{npcName}Score";
        else
            return $"Day{day}_{npcName}_Spoken";
    }
    void SetUpChoices()
{
    if (currentChoices == null || currentChoices.Length != 3)
    {
        Debug.LogError($"[NPC:{npcName}] Numero di scelte non valido: {currentChoices?.Length ?? 0}. Devono essere esattamente 3.");
        return;
    }

    // Imposta testi delle scelte
    testoSceltaPositiva.text = currentChoices[0].buttonText;
    testoSceltaNeutra.text = currentChoices[1].buttonText;
    testoSceltaNegativa.text = currentChoices[2].buttonText;

    // Rimuovi listener precedenti
    sceltaPositiva.onClick.RemoveAllListeners();
    sceltaNeutra.onClick.RemoveAllListeners();
    sceltaNegativa.onClick.RemoveAllListeners();

    // Aggiungi nuovi listener CON i punteggi corretti del giorno
    sceltaPositiva.onClick.AddListener(() => OnChoiceSelected(currentChoices[0].scoreValue));
    sceltaNeutra.onClick.AddListener(() => OnChoiceSelected(currentChoices[1].scoreValue));
    sceltaNegativa.onClick.AddListener(() => OnChoiceSelected(currentChoices[2].scoreValue));
}

    public void OnChoiceSelected(int scoreValue)  
    {
        if (punteggiScelte != null)
        {
            punteggiScelte.ChoiceButtonClick(scoreValue);
        }
        waitingForChoice = false;
    }
}


