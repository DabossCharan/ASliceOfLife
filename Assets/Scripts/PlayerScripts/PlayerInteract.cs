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
        if (talk && !dialogTrigger.dialogueStarted)
        {
            dialogTrigger.TriggerDialogue();
        } else if (talk)
        {
            dialogSystem.DisplayDialogue();
        }
    }

    private void GetInput()
    {
        talk = Input.GetButtonDown("Talk");
    }
}
