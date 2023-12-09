using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovement : MonoBehaviour
{
    private PlayerMovement movementScript;
    
    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void StopPlayerMovement()
    {
        Time.timeScale = 0;
        movementScript.enabled = false;
    }

    public void ResumePlayerMovement()
    {
        Time.timeScale = 1;
        movementScript.enabled = true;
    }
}
