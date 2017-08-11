using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private ControllerManager _controllerManager;

    [SerializeField]
    private Image _blackScreen;
    [SerializeField]
    private GameObject _mainMenuParent;
    [SerializeField]
    private GameObject _mainPanel;
    [SerializeField]
    private GameObject _creditsPanel;

    // Used for xbox controller functionality
    private Button[] _mainButtons;
    private Button _creditButton;

    void Start ()
    {
        _controllerManager = GameManager.controllerManager;
        _blackScreen = GameManager.blackScreen.GetComponent<Image>();

        _mainButtons = _mainPanel.GetComponentsInChildren<Button>();
        _creditButton = _creditsPanel.GetComponentInChildren<Button>();

        // Only have main menu for the main scene
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            StartCoroutine(InitialFadeOut());
        }
        else
        {
            SkipMainMenu();
        }
    }

    public void StartGame()
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_game_start_01"), 0.5f);
        gameObject.ChangeMusic(Music.Level);

        _mainPanel.SetActive(false);
        _mainMenuParent.SetActive(false);
        GameManager.playerControlManager.EnablelayerControl();
        GameManager.power.SetPowerIsDepleting(true);
        GameManager.gameplayUIManager.ShowGameplayUI();
    }

    public void Credits()
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
        _mainPanel.SetActive(false);
        _creditsPanel.SetActive(true);

        if(_controllerManager.xboxControllerIsConnected)
        {
            _creditButton.Select();
        }
    }

    public void BackToMain()
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
        _creditsPanel.SetActive(false);
        _mainPanel.SetActive(true);

        if(_controllerManager.xboxControllerIsConnected)
        {
            _mainButtons[0].Select();
        }        
    }

    public void Exit()
    {
        StartCoroutine(ExitGame());
    }

    IEnumerator InitialFadeOut()
    {
        GameManager.playerControlManager.DisablePlayerControl();
        _controllerManager.currentPanel = _mainPanel;
        yield return new WaitForSeconds(1);
        if (_controllerManager.xboxControllerIsConnected)
        {
            GameManager.cursorManager.HideCursor();
            _mainButtons[0].Select();
        }
        _blackScreen.CrossFadeAlpha(0, 1, false);
        yield return new WaitForSeconds(1);
        GameManager.blackScreen.SetActive(false);
    }

    private void SkipMainMenu()
    {
        _blackScreen.CrossFadeAlpha(0, 0, false);
        GameManager.blackScreen.SetActive(false);
        _mainPanel.SetActive(false);
        _mainMenuParent.SetActive(false);
        GameManager.power.SetPowerIsDepleting(true);
    }

    IEnumerator ExitGame()
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_game_exit_01"));
        _mainPanel.SetActive(false);
        GameManager.blackScreen.SetActive(true);
        // Make sure blackscreen alpha is set to zero before fade in
        _blackScreen.CrossFadeAlpha(0, 0, false);
        _blackScreen.CrossFadeAlpha(1, 1.25f, false);

        yield return new WaitForSeconds(3);
        Application.Quit();
    }
}
