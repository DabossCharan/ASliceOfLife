using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SpriteRenderer blackScreen;
    private float time = 0;
    private bool playGame = false;
    private float transitionDuration = 2;
    private float alpha;
    public Button playButton;
    public Button howToPlayButton;
    private TMP_Text playText;
    private TMP_Text howToPlayText;
    public TMP_Text title;
    public Button currentObject;
    public Color selectedColor;
    public Color unselectedColor;
    public GameObject howToPlayPanel;
    private bool howToPlaying = false;
    public AudioSource audioSource;

    private void Start()
    {
        Time.timeScale = 1.0f;
        playText = playButton.GetComponentInChildren<TMP_Text>();
        howToPlayText = howToPlayButton.GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        if (playGame)
        {
            time += Time.deltaTime;
            alpha = Mathf.Lerp(0, 1, time / transitionDuration);
            audioSource.volume = Mathf.Lerp(1, 0, time / transitionDuration);
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
            playText.color = new Color(playText.color.r, playText.color.g, playText.color.b, 1 - alpha);
            howToPlayText.color = new Color(howToPlayText.color.r, howToPlayText.color.g, howToPlayText.color.b, 1 - alpha);
            title.color = new Color(title.color.r, title.color.g, title.color.b, 1 - alpha);

            if (time >= transitionDuration)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

        }
        else if (!howToPlaying)
        {
            SelectText();
            HighlightText();
        }

        if (howToPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            howToPlaying = false;
            howToPlayPanel.SetActive(false);
        }

        
    }
    public void PlayGame()
    {
        playGame = true;
    }

    private void HighlightText()
    {
        if (currentObject == playButton)
        {
            playText.color = selectedColor;
            howToPlayText.color = unselectedColor;
        }
        else if (currentObject == howToPlayButton)
        {
            playText.color = unselectedColor;
            howToPlayText.color = selectedColor;
        }
    }

    private void SelectText()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            playButton.Select();
            currentObject = playButton;
        } else
        {
            currentObject = EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<Button>();
        }
    }

    public void HowToPlay()
    {
        howToPlayPanel.SetActive(true);
        howToPlaying = true;
    }
}
