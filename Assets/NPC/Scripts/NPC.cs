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
    
    private bool isDialogueActive = false;
    private int dialogueIndex = 0;
    private bool isTyping = false;
    private bool hasSpoken = false;


    // Permette al sistema di sapere se puoi parlare
    public bool CanInteract()
    {
        // return true;
        return !hasSpoken;
    }

    public void Interact()
    {
        if (hasSpoken)
            return;

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
        // SE STA ANCORA SCRIVENDO → completa subito la frase
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueData.dialogueLines[dialogueIndex];
            isTyping = false;
        }
        // Se ci sono altre frasi → vai avanti
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        // 🔥 ARRIVATI ALLA FINE DEL DIALOGO
        else
        {
            // Se ci sono scelte → mostriamo la UI delle scelte
            if (dialogueData.hasChoices)
            {
                DialogueChoiceUI.Instance.ShowChoices(dialogueData.choices);
                
                // far comparire il pannello delle scelte, indipendenti (3 bottoni indipendenti: 2, 1, 0)

                // Importante: il dialogo non deve andare avanti
                isDialogueActive = false;
            }
            else
            {
                EndDialogue();
            }
        }
    }
    
    // comparire scelte
    // disattivare NPC

    
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

        // Se questa linea deve avanzare automaticamente
        if (dialogueData.autoProgressLines.Length > dialogueIndex &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        hasSpoken = true;

        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        
        if (ChoicePanel != null)
        ChoicePanel.SetActive(true);
        
        this.enabled = false;
        
        // disattivare che posso riparlare con NPC
        
    }
}


