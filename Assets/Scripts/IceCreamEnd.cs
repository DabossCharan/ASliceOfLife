using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IceCreamEnd : MonoBehaviour
{
    private bool startEndingScene;
    public DialogueSystem dialogueSystem;
    private float time = 0f;
    private float alpha = 0f;
    public float transitionDuration = 1f;
    public SpriteRenderer blackScreen;
    public AudioSource audioSource;
    public IceCreamZoomOut iceCreamZoom;
    public ParticleFollowPlayer particles;
    public bool isGoingToZoomOut;
    private bool isZoomingOut = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startEndingScene && dialogueSystem.finishedDialog)
        {
            if (isGoingToZoomOut && !isZoomingOut)
            {
                iceCreamZoom.enabled = true;
                particles.enabled = false;
                isZoomingOut = true;
            }
            else if (!iceCreamZoom.enabled || iceCreamZoom.enabled && iceCreamZoom.startEndScene)
            {
                time += Time.deltaTime;
                alpha = Mathf.Lerp(0, 1, time / transitionDuration);
                audioSource.volume = Mathf.Lerp(1, 0, time / transitionDuration);
                blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);

                if (time >= transitionDuration)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            startEndingScene = true;
        }
    }
}
