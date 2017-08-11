using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    private ControllerManager _controllerManager;

    [SerializeField]
    private Image _blackScreen;
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _pauseParent;
    [SerializeField]
    private GameObject _pausePanel;

    private GameObject _currentSubPanel;

    public bool gameIsPaused = false;

    // This is used for xbox controller functionality
    private Button[] _pauseButtons;

    void Start()
    {
        _controllerManager = GameManager.controllerManager;
        _blackScreen = GameManager.blackScreen.GetComponent<Image>();

        _pauseButtons = _pausePanel.GetComponentsInChildren<Button>();
    }

    private void Update()
    {
        if(!GameManager.power.gameOver)
        {
            // Only toggle pause menu when main menu is not longer active in hierarchy
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Xbox_Start")) && !_mainMenu.activeInHierarchy)
            {
                if (!gameIsPaused)
                {
                    PauseGame();
                }
            }
        }

    }

    private void PauseGame()
    {
        Time.timeScale = 0;

        gameIsPaused = true;
        GameManager.gameplayUIManager.HideGameplayUI();
        GameManager.playerControlManager.DisablePlayerControl();

        _pauseParent.SetActive(true);
        _controllerManager.currentPanel = _pausePanel;

        if (_controllerManager.xboxControllerIsConnected)
        {
            _pauseButtons[0].Select();
        }
        else
        {
            GameManager.cursorManager.ShowCursor();
        }
    }

    public void Resume()
    {
        _pauseParent.SetActive(false);
        GameManager.playerControlManager.EnablelayerControl();
        GameManager.power.SetPowerIsDepleting(true);
        GameManager.gameplayUIManager.ShowGameplayUI();

        if (Cursor.visible)
        {
            GameManager.cursorManager.HideCursor();
        }

        gameIsPaused = false;

        Time.timeScale = 1;

        EventSystem.current.SetSelectedGameObject(null);
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
    }

    public void Quit()
    {
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_game_exit_01"));
        GameManager.blackScreen.SetActive(true);
        // Make sure blackscreen alpha is set to zero before fade in
        _blackScreen.CrossFadeAlpha(0, 0, false);
        _blackScreen.CrossFadeAlpha(1, 1, false);

        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene("Main");
    }
}
