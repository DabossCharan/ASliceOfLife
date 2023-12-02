using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    // Audio
    private AudioSource audioSource;

    // Time Management
    public float currentTime = 0;
    private float relativeTime = 0;
    public float displayFirstSpriteTime;
    public float displaySecondSpriteTime;
    public float displayThirdSpriteTime;
    public float whiteOut1;
    public float whiteOut2;
    public float fadeOut;
    public float duration;

    // Sprite Management
    public SpriteRenderer mouse;
    public SpriteRenderer bgMoon;
    public SpriteRenderer avocado;
    public SpriteRenderer blackScreen;
    public SpriteRenderer whiteScreen;
    
    // Transition Management
    private float alpha = 0;
    private bool startNextTransition;
    private float whiteOutAlpha = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        mouse.enabled = false;
        bgMoon.enabled = false;
        avocado.enabled = false;
        whiteScreen.enabled = false;
        audioSource.playOnAwake = true;
        blackScreen.color = new Color(0, 0, 0, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        relativeTime += Time.deltaTime;

        if (currentTime >= displayThirdSpriteTime && startNextTransition)
        {
            duration = fadeOut - displayThirdSpriteTime;
            avocado.enabled = true;
            bgMoon.enabled = false;
            mouse.enabled = false;
            whiteScreen.enabled = false;
            startNextTransition = false;

        }
        else if (currentTime >= displaySecondSpriteTime && startNextTransition)
        {
            duration = displayThirdSpriteTime - displaySecondSpriteTime;
            bgMoon.enabled = true;
            avocado.enabled = false;
            mouse.enabled = false;
            whiteScreen.enabled = false;
            startNextTransition = false;


        } else if (currentTime >= whiteOut1 && startNextTransition)
        {

        }
        else if (currentTime >= displayFirstSpriteTime && startNextTransition)
        {
            duration = displaySecondSpriteTime - displayFirstSpriteTime;
            mouse.enabled = true;
            avocado.enabled = false;
            bgMoon.enabled = false;
            whiteScreen.enabled = false;
            startNextTransition = false;
        }

        if (alpha == 1)
        {
            startNextTransition = true;
            alpha = 0;
            relativeTime = 0;
        }

        alpha = Mathf.Lerp(0, 1, relativeTime / duration);
        blackScreen.color = new Color(0, 0, 0, alpha);
    }
}
