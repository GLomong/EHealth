using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text textComponent;    // il testo dentro il pannello
    public float charDelay = 0.03f;   // velocità di scrittura

    Coroutine typingCoroutine;

    public void ShowText(string fullText)
    {
        // se stava già scrivendo, fermalo
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        gameObject.SetActive(true);          // mostra il pannello (se lo script è sul pannello)
        typingCoroutine = StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string fullText)
    {
        textComponent.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            textComponent.text += fullText[i];
            yield return new WaitForSeconds(charDelay);
        }
        typingCoroutine = null;
    }
}

