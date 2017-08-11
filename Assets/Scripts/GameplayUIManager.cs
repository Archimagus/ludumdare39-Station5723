using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameplayUIManager : MonoBehaviour
{
    public GameObject crosshair;
    public GameObject inventoryPanel;
    public GameObject subtitlesPanel;
    public GameObject lostPanel;
    public GameObject wonPanel;
    public GameObject lostContentsPanel;
    public GameObject wonContentsPanel;
    public GameObject powerIndicatorPanel;

    private GameObject _gameOverPanel;

	private TextMeshProUGUI _subtitleGUI;

	private void Awake()
    {
        if(SceneManager.GetActiveScene().name.Equals("Main"))
        {
            HideGameplayUI();
        }
		_subtitleGUI = subtitlesPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowGameplayUI()
    {
        crosshair.SetActive(true);
        inventoryPanel.SetActive(true);
        subtitlesPanel.SetActive(true);
        powerIndicatorPanel.SetActive(true);
    }

    public void HideGameplayUI()
    {
        crosshair.SetActive(false);
        inventoryPanel.SetActive(false);
        subtitlesPanel.SetActive(false);
        powerIndicatorPanel.SetActive(false);
    }

	public IEnumerator DisplaySubtitle(string message, float messageTime)
	{
		if (!string.IsNullOrEmpty(_subtitleGUI.text)) { yield break; }
		_subtitleGUI.text = message;
		float startTime = Time.time;
		while (Time.time - startTime < messageTime)
		{
			yield return null;
		}
		_subtitleGUI.text = "";
	}

    public void ShowLostPanel()
    {
        gameObject.ChangeMusic(Music.GameLost);
        GameManager.playerControlManager.DisablePlayerControl();
        lostPanel.SetActive(true);
        _gameOverPanel = lostContentsPanel;

        if (GameManager.controllerManager.xboxControllerIsConnected)
        {
            lostPanel.GetComponentsInChildren<Selectable>()[0].Select();
        }
        else
        {
            GameManager.cursorManager.ShowCursor();
        }
    }

    public void ShowWonPanel()
    {
        gameObject.ChangeMusic(Music.GameWon);
        GameManager.playerControlManager.DisablePlayerControl();
        wonPanel.SetActive(true);
        _gameOverPanel = wonContentsPanel;

        if (GameManager.controllerManager.xboxControllerIsConnected)
        {
            wonPanel.GetComponentsInChildren<Selectable>()[0].Select();
        }
        else
        {
            GameManager.cursorManager.ShowCursor();
        }
    }

    public void MainMenu()
    {
        _gameOverPanel.SetActive(false);
        StartCoroutine(ChangeScene(false));
    }

    public void ExitGame()
    {
        _gameOverPanel.SetActive(false);
        StartCoroutine(ChangeScene(true));
    }

    IEnumerator ChangeScene(bool isExit)
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_game_exit_01"));

        yield return new WaitForSeconds(3);

        if(isExit)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }
}
