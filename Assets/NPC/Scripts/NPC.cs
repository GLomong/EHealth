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

    [Header("NPC Settings")]
    public string npcName; // Cashier, Influencer, Gentlemen, Friend, ...
    public bool hasScore = true; // false per NPC senza scelte
    
    private bool isDialogueActive = false;
    private int dialogueIndex = 0;
    private bool isTyping = false;
    private bool waitingForChoice = false;


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

        nameText.text = dialogueData.npcName;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueData.dialogueLines[dialogueIndex];
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
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

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex &&
            dialogueData.autoProgressLines[dialogueIndex])
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
            ChoicePanel.SetActive(true);
    }

    string GetDailyKey()
    {
        int day = TotalGameManager.Instance.CurrentDay;

        if (hasScore)
            return $"Day{day}_{npcName}Score";
        else
            return $"Day{day}_{npcName}_Spoken";
    }
}


