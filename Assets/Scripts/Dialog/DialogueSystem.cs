using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    // Stop Player Movement When Dialog Starts
    private StopMovement stopMovementScript;
    public TMP_Text dialogTextUI;
    public Queue<string> dialogs;
    public GameObject dialogPanel;
    private PauseMenu pauseMenu;
    public GameObject dialogSpriteImage;

    // For the sake of stopping dialogue when paused
    public bool startedDialog = false;
    public bool finishedDialog = false;
    public bool justPaused = false;
    private bool isDialoging = false;
    public string dialog;
    public int currentTextLen;

    // Show Who's Talking
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        dialogs = new Queue<string>();
        stopMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<StopMovement>();
        pauseMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<PauseMenu>();
        image = dialogSpriteImage.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenu.isPaused)
        {
            StopAllCoroutines();
            justPaused = true;
        } else if (justPaused && startedDialog)
        {
            StartCoroutine(PrintLetters(dialog));
            stopMovementScript.StopPlayerMovement();
        }
    }

    public void StartDialogue(Dialog dialogue, Sprite sprite)
    {
        dialogs.Clear();
        stopMovementScript.StopPlayerMovement();
        foreach (string dialog in dialogue.dialogs)
        {
            dialogs.Enqueue(dialog);
        }

        dialogPanel.SetActive(true);
        image.sprite = sprite;
        DisplayDialogue();
        
        startedDialog = true;
    }

    public void DisplayDialogue()
    {
        if (dialogs.Count == 0)
        {
            EndDialogue();
            finishedDialog = true;
            return;
        }

        if (isDialoging)
        {
            StopAllCoroutines();
            dialogTextUI.SetText(dialog);
            isDialoging = false;
        }
        else
        {
            dialog = dialogs.Dequeue();
            StopAllCoroutines();
            StartCoroutine(PrintLetters(dialog));
        }
    }

    IEnumerator PrintLetters(string dialog)
    {
        isDialoging = true;
        string currentText = ""; 
        
        // If Game Paused During Middle Of Dialogue
        if (justPaused && startedDialog)
        {
            currentText = dialogTextUI.text;
            currentTextLen = currentText.Length;
            dialog = dialog.Substring(currentTextLen, dialog.Length - currentText.Length);
            justPaused = false;
        }

        // Print Letters Slowly
        dialogTextUI.SetText(currentText);

        foreach(char c in dialog.ToCharArray())
        {
            currentText += c;
            dialogTextUI.SetText(currentText);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        isDialoging = false;
    }

    public void EndDialogue()
    {
        stopMovementScript.ResumePlayerMovement();
        dialogPanel.SetActive(false);
        startedDialog = false;
    }
}
