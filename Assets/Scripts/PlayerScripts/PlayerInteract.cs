using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    
    // Manage Dialogue Interaction
    private DialogueSystem dialogSystem;
    private bool talk;

    // Start is called before the first frame update
    void Start()
    {
        dialogSystem = GameObject.Find("DialogueSystem").GetComponent<DialogueSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        
        if (talk)
        {
            dialogSystem.DisplayDialogue();
        }
    }

    private void GetInput()
    {
        talk = Input.GetButtonDown("Talk");
    }
}
