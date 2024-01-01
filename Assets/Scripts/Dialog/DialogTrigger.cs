using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    public bool dialogueStarted = false;
    public DialogueSystem dialogueSystem;
    private bool alreadyTriggered = false;
    public Sprite sprite;

    public void Start()
    {
        dialogueSystem = GameObject.Find("DialogueSystem").GetComponent<DialogueSystem>();
        sprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void TriggerDialogue()
    {
        dialogueSystem.StartDialogue(dialog, sprite);
        dialogueStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !alreadyTriggered)
        {
            TriggerDialogue();
            alreadyTriggered = true;
        }
    }
}
