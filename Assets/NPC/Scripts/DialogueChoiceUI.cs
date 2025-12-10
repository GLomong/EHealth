using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceUI : MonoBehaviour
{
    public static DialogueChoiceUI Instance;

    public GameObject choiceContainer;     // il panel che contiene tutti i bottoni
    public Button choiceButtonPrefab;      // un singolo prefab del bottone

    private void Awake()
    {
        Instance = this;
        choiceContainer.SetActive(false);
    }

    public void ShowChoices(DialogueChoice[] choices)
    {
        choiceContainer.SetActive(true);

        // Pulisci eventuali bottoni precedenti
        foreach (Transform child in choiceContainer.transform)
            Destroy(child.gameObject);

        // Crea un bottone per ogni scelta
        foreach (DialogueChoice choice in choices)
        {
            Button btn = Instantiate(choiceButtonPrefab, choiceContainer.transform);
            btn.GetComponentInChildren<TMP_Text>().text = choice.choiceText;

            btn.onClick.AddListener(() =>
            {
                Debug.Log("Hai scelto: " + choice.choiceText);
                Debug.Log("Punteggio: " + choice.score);

                // Chiudi UI scelte
                choiceContainer.SetActive(false);

                // Chiudi dialogo
                DialogueController.Instance.ShowDialogueUI(false);

                // Qui in futuro puoi avviare altre azioni
            });
        }
    }
}


