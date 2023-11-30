using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    // This Script is Meant to Be Used By Other Characters
    // When Made But Testing For Now
    public Dialog dialog;
    public bool dialogueStarted = false;
    public DialogueSystem dialogueSystem;

    public void Start()
    {
        dialogueSystem = GameObject.Find("DialogueSystem").GetComponent<DialogueSystem>();
    }

    public void TriggerDialogue()
    {
        dialogueSystem.StartDialogue(dialog);
        dialogueStarted = true;
    }
}
