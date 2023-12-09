using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    // Stop Player Movement When Dialog Starts
    private StopMovement stopMovementScript;
    public TMP_Text dialogTextUI;
    private Queue<string> dialogs;
    public GameObject dialogPanel;
    // Start is called before the first frame update
    void Start()
    {
        dialogs = new Queue<string>();
        stopMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<StopMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(Dialog dialogue)
    {
        dialogs.Clear();
        stopMovementScript.StopPlayerMovement();
        foreach (string dialog in dialogue.dialogs)
        {
            dialogs.Enqueue(dialog);
        }

        dialogPanel.SetActive(true);

        DisplayDialogue();
    }

    public void DisplayDialogue()
    {
        if (dialogs.Count == 0)
        {
            EndDialogue();
            return;
        }

        string dialog = dialogs.Dequeue();
        StopAllCoroutines();
        StartCoroutine(PrintLetters(dialog));
    }

    IEnumerator PrintLetters(string dialog)
    {
        string currentText = "";
        dialogTextUI.SetText(currentText);

        foreach(char c in dialog.ToCharArray())
        {
            currentText += c;
            dialogTextUI.SetText(currentText);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void EndDialogue()
    {
        stopMovementScript.ResumePlayerMovement();
        dialogPanel.SetActive(false);
    }
}
