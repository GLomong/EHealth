using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Transform entryTarget;   // Punto dove fermarsi(prima di dialogo iniziale)
    public Transform exitTarget;   // Punto dove fermarsi per uscire(prima di dialogo finale)
    public Transform[] jumpPoints;  // Punti dove saltare
    public float entrySpeed = 2f;
    public float jumpSpeed = 5f;
    public int positions = 0;      // Punto di partenza del personaggio (indice 0)
    public int pos_max = 0;
    public bool canClick = true;
    public Transform out_target;
    public bool outComplete = false;
    public int ponte_score;
    public bool flag_visualizzazione=false;
    public int num_mosse=0;


    [Header("Probabilità caduta")]
    [Range(0,100)] public int fallChance1 = 5;
    [Range(0,100)] public int fallChance2 = 20;
    [Range(0,100)] public int fallChance3 = 40;


    private bool entryComplete = false;
    private bool isJumping = false;
    private Transform currentTarget;
    private bool isdialogue = false;
    private bool isouting = false;
    private bool exitDialogueFinish = false;
    private bool exitComplete = false;
    private bool dialogueinit = false;
    

    void Start()
    {
        // Imposta l'ultimo indice valido
        pos_max = jumpPoints.Length - 1;
    }

    void Update()
    {
        if (!entryComplete)
        {
            MoveToEntry();
        }
        else if (!dialogueinit) EnterDialogue();
        else if (!isJumping)
        {
            if(positions >= pos_max) 
            {
                MoveToExit();
            }
            if(exitComplete == true|| isdialogue) 
            {
                ExitDialogue();
                exitComplete = false;
            }
            if(exitDialogueFinish == true|| isouting) 
            {
                ExitMovement();
                exitDialogueFinish = false;
            }
        }
        else if (isJumping)
        {
            ContinueJump();
        }
    }

    void MoveToEntry()
    {
        transform.position = Vector3.MoveTowards(transform.position, entryTarget.position, entrySpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, entryTarget.position) < 0.2f)
            {
                Debug.Log("arrivato ad entry target");
                //qua si aggiungono prima le cose del dialogo se necesario
                entryComplete = true;
                canClick = true;
            }
    }

    public void StartJump(int index)
    {
        num_mosse+=1;
        int chance = 0;
        if (index == 1) chance = fallChance1;
        else if (index == 2) chance = fallChance2;
        else if (index == 3) chance = fallChance3;
        //float multiplier = Mathf.Pow(2, day - 1); 
        //int chance = Mathf.RoundToInt(Chance * multiplier);
        int roll = Random.Range(0, 100);
        if (roll < chance)
        {
            FallAndReset(); //cade
            return;
        }

        positions += index;

        if (positions > pos_max)
        {
            positions = pos_max;
        }

        currentTarget = jumpPoints[positions];
        isJumping = true;
        canClick = false;

        Debug.Log("StartJump chiamato, target = " + currentTarget.name);

    }

void FallAndReset()
    {
        Debug.Log("SEI CADUTO!");
        StartCoroutine(FallAnimation());
    }

    IEnumerator FallAnimation()
    {
        canClick = false;
        isJumping = false;

        // 1. Animazione shrink
        Vector3 originalScale = transform.localScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;  // velocità dell’animazione
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        // 2. Teletrasporto al punto iniziale
        positions = 0;
        transform.position = jumpPoints[0].position;

        // 3. Animazione grow
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
            yield return null;
        }

        // Riattiva il gioco
        canClick = true;
    }

    void ContinueJump()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, jumpSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
            {
                isJumping = false;
                if(positions < pos_max)
                    canClick=true;
                else 
                {
                    ponte_score=num_mosse;
                    num_mosse=0;
                    flag_visualizzazione=true;
                }
            }
    }
    
    void MoveToExit()
    {
        transform.position = Vector3.MoveTowards(transform.position, exitTarget.position, entrySpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, exitTarget.position) < 0.01f)
            exitComplete = true;
    }

    void EnterDialogue()
    {
        //inserisci qua il dialogo
        //dialogueinit=true;  //da decommentare dentro l'if che caratterizza la fine del dialogo
    }
    void ExitDialogue()
    {
        isdialogue=true;
        //quello che bisognerà programmare
        if(isdialogue==true)  //non ho la distanza, da capire come determinare la fine del dialogo
        {
            exitDialogueFinish = true;
            isdialogue=false;
        }
    }

    void ExitMovement()
    {
        isouting=true;
        transform.position = Vector3.MoveTowards(transform.position, out_target.position, entrySpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, out_target.position) < 0.01f)
            {
                outComplete = true;  //da dire cosa si farà quando finisce di uscire dallo schermo
                isouting=false;
            }
    }
}
