using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private StopMovement stopPlayerScript;
    private PlayerInteract playerInteract;
    public bool isPaused;
    public Button resumeButton;
    public Button retryButton;
    public Button exitButton;
    private TMP_Text resumeText;
    private TMP_Text retryText;
    private TMP_Text exitText;
    public Button currentObject;
    private Color selectedColor = new Color(1, 0, 0, 1);
    private Color unselectedColor = new Color(0, 0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        stopPlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<StopMovement>();
        playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
        Time.timeScale = 1.0f;
        resumeText = resumeButton.GetComponentInChildren<TMP_Text>();
        retryText = retryButton.GetComponentInChildren<TMP_Text>();
        exitText = exitButton.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        if (isPaused)
        {
            // I'm crying at the fact that I'm repeatedly making a reference to an object in UPDATE
            // i cri everytiem. If you can make it better, pls do.
            HighlightText();
            currentObject = EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<Button>();
        }
    }

    public void Pause()
    {
        isPaused = true;
        AudioListener.pause = true;
        pauseMenuPanel.SetActive(true);
        stopPlayerScript.StopPlayerMovement();
        resumeButton.Select();
        playerInteract.enabled = false;
    }

    public void Resume()
    {
        isPaused = false;
        AudioListener.pause = false;
        pauseMenuPanel.SetActive(false);
        stopPlayerScript.ResumePlayerMovement();
        playerInteract.enabled = true;
    }

    private void GetInput()
    {        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                ReturnToMainMenu();
            }
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HighlightText()
    {
        if (currentObject == resumeButton)
        {
            resumeText.color = selectedColor;
            retryText.color = unselectedColor;
            exitText.color = unselectedColor;

        }
        else if (currentObject == retryButton)
        {
            resumeText.color = unselectedColor;
            retryText.color = selectedColor;
            exitText.color = unselectedColor;
        }
        else if (currentObject == exitButton)
        {
            resumeText.color = unselectedColor;
            retryText.color = unselectedColor;
            exitText.color = selectedColor;
        }
        else if (currentObject == null)
        {
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        }
    }
}
