using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;

    public DialogueChoiceUI choiceUI; // 🔥 nuova aggiunta

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.text = npcName;
        if (portraitImage != null) portraitImage.sprite = portrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    // 🔥 NUOVO METODO
    public void ShowChoices(DialogueChoice[] choices)
    {
        dialogueText.text = "";                   // nasconde testo
        ShowDialogueUI(false);                    // nascondo pannello
        choiceUI.ShowChoices(choices);            // mostro bottoni
    }
}