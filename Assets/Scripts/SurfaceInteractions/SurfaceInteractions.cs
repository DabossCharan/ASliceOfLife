using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SurfaceInteractions : MonoBehaviour
{
    [Header("Orange Platform Settings")]
    public bool stick;
    public float stickyJumpBonus;
    public float stickyHorizontalBonus;
    public float stickyBonusTime;

    public bool hasBonus = false;
    private float regularJumpForce;
    private float regularHorizontalAccel;
    private float regularHorizontalSpeed;

    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        regularJumpForce = playerMovement.jumpForce;
        regularHorizontalAccel = playerMovement.horizontalAccel;
        regularHorizontalSpeed = playerMovement.horizontalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        HandleOrangePlatform();
    }

    #region HandleOrangePlatforms
    void HandleOrangePlatform()
    {
        // Upon Hitting Orange Platform, Get Stuck
        if (stick && !hasBonus)
        {
            StartCoroutine(StickyOrange());
            AddBonus(stickyJumpBonus, stickyHorizontalBonus);
        }
        else if (stick)
        {
            StartCoroutine(StickyOrange());
        }
        else if (hasBonus)
        {
            if (playerMovement.grounded)
            {
                StopCoroutine(StickyOrange());
                CancelBonus(stickyJumpBonus, stickyHorizontalBonus);
            }
            else
            {
                StartCoroutine(StickyBonusTime());
                StopCoroutine(StickyOrange());
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    IEnumerator StickyOrange()
    {
        playerMovement.playerVelocity = Vector2.zero;
        yield return null;
    }

    IEnumerator StickyBonusTime()
    {
        yield return new WaitForSeconds(stickyBonusTime);
        CancelBonus(stickyJumpBonus, stickyHorizontalBonus);
    }

    #endregion


    #region Bonuses
    void AddBonus(float jumpBonus, float horizontalBonus)
    {
        hasBonus = true;
        playerMovement.jumpForce += jumpBonus;
        playerMovement.horizontalAccel += horizontalBonus;
        playerMovement.horizontalSpeed += horizontalBonus;
    }

    void CancelBonus(float jumpBonus, float horizontalBonus)
    {
        hasBonus = false;
        playerMovement.jumpForce = regularJumpForce;

        if (playerMovement.playerVelocity.y > jumpBonus)
        {
            playerMovement.playerVelocity.y -= jumpBonus;
        }

        playerMovement.horizontalAccel = regularHorizontalAccel;
        playerMovement.horizontalSpeed = regularHorizontalSpeed;
    }
    #endregion
}
