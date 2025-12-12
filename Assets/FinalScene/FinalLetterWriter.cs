using UnityEngine;
using TMPro;
using System.Collections;

public class FinalLetterWriter : MonoBehaviour
{
    public TMP_Text letterText;

    [TextArea(6, 20)]
    public string fullMessage;

    public float writingSpeed = 0.06f;

    public AudioSource typeSound;

    void Start()
    {
        letterText.text = "";
        StartCoroutine(TypeLetter());
    }

    IEnumerator TypeLetter()
    {
        int soundCounter = 0;

        foreach (char c in fullMessage)
        {
            letterText.text += c;
            
            soundCounter++;

            bool shouldPlaySound =
                soundCounter % Random.Range(2, 4) == 0 &&
                c != ' ' && c != '\n' &&
                !"aeiouAEIOU".Contains(c.ToString());

            if (shouldPlaySound)
            {
                typeSound.pitch = Random.Range(0.88f, 1.02f);
                typeSound.volume = Random.Range(0.35f, 0.5f);
                typeSound.Play();
            }
            
            float pause = writingSpeed * Random.Range(0.9f, 1.3f);
            
            if (c == ',' )
                pause *= Random.Range(2.5f, 3.5f);

            else if (c == '.' || c == '!' || c == '?')
                pause *= Random.Range(6f, 9f);
            
            if (Random.value < 0.06f)
                pause *= Random.Range(1.5f, 2.5f);

            yield return new WaitForSeconds(pause);
        }
    }
}



