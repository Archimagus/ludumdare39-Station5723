using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SharedPanelManager : MonoBehaviour
{
    private Brightness _brightness;
    private ControllerManager _controllerManager;
    private AudioSettingsManager _audioSettingsManager;

    private GameObject _currentSubPanel;

    [SerializeField]
    private GameObject _optionsPanel;
    [SerializeField]
    private GameObject _audioPanel;
    [SerializeField]
    private GameObject _videoPanel;
    [SerializeField]
    private GameObject _controlsPanel;
    [SerializeField]
    private GameObject _instructionsPanel;

    private GameObject _parentPanel;

    // These are used for xbox controller functionality
    private Button[] _optionButtons;
    private Slider[] _audioSliders;
    private Button[] _videoButtons;
    private Button _controlButton;
    private Button _instructionButton;


    private void Start()
    {
        _brightness = GameManager.brightness;
        _controllerManager = GameManager.controllerManager;
        _audioSettingsManager = GameManager.audioSettingsManager;

        _optionButtons = _optionsPanel.GetComponentsInChildren<Button>();
        _audioSliders = _audioPanel.GetComponentsInChildren<Slider>();
        _videoButtons = _videoPanel.GetComponentsInChildren<Button>();
        _controlButton = _controlsPanel.GetComponentInChildren<Button>();
        _instructionButton = _instructionsPanel.GetComponentInChildren<Button>();
    }

    public void GoToOptions(GameObject parent)
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
        if (parent != null)
        {
            _parentPanel = parent;
            parent.SetActive(false);
        }

        ActivateNewSubPanel(_optionsPanel, _optionButtons[0]);
    }

    public void ReturnToOptions()
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
        if (_currentSubPanel.Equals(_videoPanel))
        {
            PlayerPrefs.SetFloat("GameBrightness", _brightness.brightness);
        }
        else if(_currentSubPanel.Equals(_audioPanel))
        {
            _audioSettingsManager.SaveValues();
        }
        else if(_currentSubPanel.Equals(_controlsPanel))
        {
            GameManager.controlSettingsManager.SaveValues();
        }

        DeactivateSubPanel();

        ActivateNewSubPanel(_optionsPanel, _optionButtons[0]);
    }

    public void GoToAudio()
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
        DeactivateSubPanel();
        ActivateNewSubPanel(_audioPanel, _audioSliders[0]);
    }

    public void GoToVideo()
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
        GameManager.videoSettingsManager.PopulateVideoSettingValues();

        DeactivateSubPanel();
        ActivateNewSubPanel(_videoPanel, _videoButtons[0]);
    }

    public void GoToControls()
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
        DeactivateSubPanel();
        ActivateNewSubPanel(_controlsPanel, _controlButton);
    }

    public void GoToInstructions(GameObject parent)
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
        if (parent != null)
        {
            _parentPanel = parent;
            parent.SetActive(false);
        }

        ActivateNewSubPanel(_instructionsPanel, _instructionButton);
    }

    public void ReturnToParent()
    {
        gameObject.PlayInterfaceSound(gameObject.GetSound("sfx_ui_select_01"));
        DeactivateSubPanel();

        _parentPanel.SetActive(true);
        _controllerManager.currentPanel = _parentPanel;

        if (_controllerManager.xboxControllerIsConnected)
        {
            _parentPanel.GetComponentsInChildren<Button>()[0].Select();
        }
    }

    private void ActivateNewSubPanel(GameObject panel, Selectable selectable)
    {
        _currentSubPanel = panel;
        _currentSubPanel.SetActive(true);
        _controllerManager.currentPanel = _currentSubPanel;

        if (_controllerManager.xboxControllerIsConnected)
        {
            selectable.Select();
        }
    }

    private void DeactivateSubPanel()
    {
        _currentSubPanel.SetActive(false);
        _currentSubPanel = null;
    }

    public void SelectAvailableButton(int direction)
    {

    }
}
