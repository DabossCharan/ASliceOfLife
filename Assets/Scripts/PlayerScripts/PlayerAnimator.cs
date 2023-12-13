using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (playerMovement.jumpKeyDown)
        {
            playerAnimator.SetBool("startJump", true);
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("isLanding", false);
            playerAnimator.SetBool("isFalling", false);
        }
        else if (playerMovement.playerVelocity.y > 0)
        {
            playerAnimator.SetBool("isJumping", true);
            playerAnimator.SetBool("startJump", false);
            playerAnimator.SetBool("isLanding", false);
            playerAnimator.SetBool("isFalling", false);
        }
        else if (playerMovement.justLanded)
        {
            playerAnimator.SetBool("isLanding", true);
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("startJump", false);
            playerAnimator.SetBool("isFalling", false);
        }
        else if (playerMovement.playerVelocity.y < 0)
        {
            playerAnimator.SetBool("isFalling", true);
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("isLanding", false);
            playerAnimator.SetBool("startJump", false);
        }
        else
        {
            playerAnimator.SetBool("startJump", false);
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("isLanding", false);
            playerAnimator.SetBool("isFalling", false);
        }

        if (playerMovement.grounded)
        {
            playerAnimator.SetBool("isMoving", playerMovement.isMoving);
        }
    }
}
