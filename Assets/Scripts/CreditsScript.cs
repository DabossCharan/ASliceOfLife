using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour
{
    public Button currentObject;
    public Button replayButton;
    public Button quitButton;
    private TMP_Text yesText;
    private TMP_Text quitText;
    public Color selectedColor;
    public Color unselectedColor;

    private void Start()
    {
        yesText = replayButton.GetComponentInChildren<TMP_Text>();
        quitText = quitButton.GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        SelectText();
        HighlightText();
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void SelectText()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            replayButton.Select();
            currentObject = replayButton;
        }
        else
        {
            currentObject = EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<Button>();
        }
    }

    private void HighlightText()
    {
        if (currentObject == replayButton)
        {
            yesText.color = selectedColor;
            quitText.color = unselectedColor;
        }
        else if (currentObject == quitButton)
        {
            yesText.color = unselectedColor;
            quitText.color = selectedColor;
        }
    }

}
