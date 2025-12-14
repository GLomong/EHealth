using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("NPC Dialogue Settings")]
    public string npcName;
    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 1.5f;
    [Header("Dialogue per Day")]
    [TextArea(3, 5)]
    public string[] day1Lines;
    [TextArea(3, 5)]
    public string[] day2Lines;
    [TextArea(3, 5)]
    public string[] day3Lines;
    //public string[] dialogueLines;
    [Header("Auto Progress Lines Settings")]
    public bool[] autoProgressDay1;
    public bool[] autoProgressDay2;
    public bool[] autoProgressDay3;
    //public bool[] autoProgressLines;
    [Header("Choice Buttons per Day")]
    public ChoiceOption[] day1Choices;  // scelte per giorno 1
    public ChoiceOption[] day2Choices;
    public ChoiceOption[] day3Choices;

    // Restituisce le linee di dialogo in base al giorno corrente
    public string[] dialogueLines(int day)
    {
        switch (day)
        {
            case 1:
                return day1Lines;
            case 2:
                return day2Lines;
            case 3:
                return day3Lines;
            default:
                return new string[0];
        }
    }

    // Restituisce se il dialogo deve progredire automaticamente in base al giorno corrente
    public bool[] autoProgressLines(int day)
    {
        switch (day)
        {
            case 1:
                return autoProgressDay1;
            case 2:
                return autoProgressDay2;
            case 3:
                return autoProgressDay3;
            default:
                return new bool[0];
        }
    }
    // Restituisce le opzioni di scelta in base al giorno corrente
    public ChoiceOption[] GetChoicesForDay(int day)
    {
        switch (day)
        {
            case 1:
                return day1Choices;
            case 2:
                return day2Choices;
            case 3:
                return day3Choices;
            default:
                return new ChoiceOption[0];
        }
    }
}

// Struttura per una singola scelta (testo + effetto)
[System.Serializable]
public class ChoiceOption
{
    [TextArea(2, 3)] public string buttonText;  
    public int scoreValue = 0;                      // Punteggio associato alla scelta (0,+1,+2)  
}