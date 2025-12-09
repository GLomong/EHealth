using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public string[] dialogueLines;

    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;

    public float typingSpeed = 0.05f;
    
    public bool hasChoices;   // Se vero → alla fine del dialogo mostra le scelte
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;  // testo della scelta che appare sul bottone
    public int score;          // punteggio associato a questa scelta
}