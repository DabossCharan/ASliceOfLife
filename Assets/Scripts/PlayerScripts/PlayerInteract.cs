using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    
    // Manage Dialogue Interaction
    private DialogTrigger dialogTrigger;
    private DialogueSystem dialogSystem;
    private bool talk;

    // Start is called before the first frame update
    void Start()
    {
        dialogTrigger = GetComponent<DialogTrigger>();
        dialogSystem = GameObject.Find("DialogueSystem").GetComponent<DialogueSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        // Start Dialog
        //if (talk && !dialogTrigger.dialogueStarted)
        //{
        //    dialogTrigger.TriggerDialogue();
        //} 
        
        if (talk)
        {
            dialogSystem.DisplayDialogue();
        }
    }

    private void GetInput()
    {
        talk = Input.GetButtonDown("Talk");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ron" && !dialogTrigger.dialogueStarted)
        {
            dialogTrigger.TriggerDialogue();
        }
    }
}
