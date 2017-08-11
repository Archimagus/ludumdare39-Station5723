using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class VideoSettingsManager : MonoBehaviour
{
    private ControllerManager _controllerManager;
    private Brightness _brightness;
    private PostProcessingManager _postProcessingManager;

    public VideoSettings videoSettings;
    public List<ResolutionOptions> resolutionOptions;
    public OverallQualitySettings[] overallQualitySettings;
    public ShadowQualitySettings[] shadowQualitySettings;
    public PostProcessingOption[] postProcessingOptions;
    public Slider brightnessSlider;

    private UnityEngine.Resolution[] _supportedResolutions;

    private int _currentResolutionIndex = 0;
    private int _currentQualitySettingIndex = 0;
    private int _currentShadowQualitySettingIndex = 0;
    private int _currentPostProcessingIndex = 0;

    void Start ()
    {
        _controllerManager = GameManager.controllerManager;
        _brightness = GameManager.brightness;
        _postProcessingManager = GameManager.postProcessingManager;

        CreateSupportedResolutionOptions();
        SetIndexValues();
    }
	
	void Update ()
    {
		
	}

    private void CreateSupportedResolutionOptions()
    {
        _supportedResolutions = Screen.resolutions;
        for (int i = 0; i < _supportedResolutions.Length; i++)
        {
            if (_supportedResolutions[i].refreshRate >= 29)
            {
                ResolutionOptions resolutionOption = new ResolutionOptions();
                resolutionOption.width = _supportedResolutions[i].width;
                resolutionOption.height = _supportedResolutions[i].height;
                resolutionOption.refreshRate = _supportedResolutions[i].refreshRate;

                resolutionOptions.Add(resolutionOption);
            }
        }
    }

    private void SetIndexValues()
    {
        _currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", _supportedResolutions.Length - 1);
        _currentQualitySettingIndex = PlayerPrefs.GetInt("QualitySettingIndex", QualitySettings.GetQualityLevel());
        _currentShadowQualitySettingIndex = PlayerPrefs.GetInt("ShadowQualitySettingIndex", QualitySettings.GetQualityLevel());
        _currentPostProcessingIndex = PlayerPrefs.GetInt("PostProcessingOptionIndex", postProcessingOptions.Length - 1);
    }

    public void PopulateVideoSettingValues()
    {
        if (_currentResolutionIndex >= resolutionOptions.Count)
        {
            _currentResolutionIndex = resolutionOptions.Count - 1;
        }
        videoSettings.currentResolutionText.text = resolutionOptions[_currentResolutionIndex].width + " x " + resolutionOptions[_currentResolutionIndex].height + " " + resolutionOptions[_currentResolutionIndex].refreshRate + "Hz";

        videoSettings.currentQualityText.text = overallQualitySettings[_currentQualitySettingIndex].settingText;
        QualitySettings.SetQualityLevel(_currentQualitySettingIndex);

        videoSettings.currentShadowQualityText.text = shadowQualitySettings[_currentShadowQualitySettingIndex].settingText;
        QualitySettings.shadowResolution = shadowQualitySettings[_currentShadowQualitySettingIndex].shadowResolution;

        videoSettings.currentPostProcessingOptionText.text = postProcessingOptions[_currentPostProcessingIndex].settingText;
        _postProcessingManager.SetPostProcessing(postProcessingOptions[_currentPostProcessingIndex].isEnabled);

        brightnessSlider.value = _brightness.brightness;

        if(!_controllerManager.xboxControllerIsConnected)
        {
            SettingsButtonHelper();
        }
        
    }

    public void SettingsButtonHelper()
    {
        if (_currentResolutionIndex < 1)
        {
            videoSettings.resSettingButtons[0].interactable = false;
        }
        else if (_currentResolutionIndex.Equals(resolutionOptions.Count - 1))
        {
            videoSettings.resSettingButtons[1].interactable = false;
        }
        else
        {
            videoSettings.resSettingButtons[0].interactable = true;
            videoSettings.resSettingButtons[1].interactable = true;
        }

        if (_currentQualitySettingIndex < 1)
        {
            videoSettings.qualitySettingButtons[0].interactable = false;
        }
        else if (_currentQualitySettingIndex.Equals(overallQualitySettings.Length - 1))
        {
            videoSettings.qualitySettingButtons[1].interactable = false;
        }
        else
        {
            videoSettings.qualitySettingButtons[0].interactable = true;
            videoSettings.qualitySettingButtons[1].interactable = true;
        }

        if (_currentShadowQualitySettingIndex < 1)
        {
            videoSettings.shadowQualitySettingButtons[0].interactable = false;
        }
        else if (_currentShadowQualitySettingIndex.Equals(shadowQualitySettings.Length - 1))
        {
            videoSettings.shadowQualitySettingButtons[1].interactable = false;
        }
        else
        {
            videoSettings.shadowQualitySettingButtons[0].interactable = true;
            videoSettings.shadowQualitySettingButtons[1].interactable = true;
        }

        if (_currentPostProcessingIndex < 1)
        {
            videoSettings.postProcessOptionButtons[0].interactable = false;
            videoSettings.postProcessOptionButtons[1].interactable = true;
        }
        else
        {
            videoSettings.postProcessOptionButtons[0].interactable = true;
            videoSettings.postProcessOptionButtons[1].interactable = false;
        }
    }

    public void EnableAllSettingArrows()
    {
        videoSettings.resSettingButtons[0].interactable = true;
        videoSettings.resSettingButtons[1].interactable = true;

        videoSettings.qualitySettingButtons[0].interactable = true;
        videoSettings.qualitySettingButtons[1].interactable = true;

        videoSettings.shadowQualitySettingButtons[0].interactable = true;
        videoSettings.shadowQualitySettingButtons[1].interactable = true;

        videoSettings.postProcessOptionButtons[0].interactable = true;
        videoSettings.postProcessOptionButtons[1].interactable = true;
    }

    public void ChangeResolution(int direction)
    {
        if (direction < 0)
        {
            if(_currentResolutionIndex > 0)
            {
                _currentResolutionIndex--;
            }
            else
            {
                _currentResolutionIndex = resolutionOptions.Count - 1;
            }            
        }
        else
        {
            if(_currentResolutionIndex < resolutionOptions.Count - 1)
            {
                _currentResolutionIndex++;
            }
            else
            {
                _currentResolutionIndex = 0;
            }      
        }

        videoSettings.currentResolutionText.text = resolutionOptions[_currentResolutionIndex].width + " x " + resolutionOptions[_currentResolutionIndex].height + " " + resolutionOptions[_currentResolutionIndex].refreshRate + "Hz";

        Screen.SetResolution(resolutionOptions[_currentResolutionIndex].width, resolutionOptions[_currentResolutionIndex].height, true, 60);
        PlayerPrefs.SetInt("ResolutionIndex", _currentResolutionIndex);

        if(!_controllerManager.xboxControllerIsConnected)
        {
            SettingsButtonHelper();
        }

        if (!_controllerManager.xboxControllerIsConnected)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ChangeOverallQuality(int direction)
    {
        if (direction < 0)
        {
            if(_currentQualitySettingIndex > 0)
            {
                _currentQualitySettingIndex--;
            }
            else
            {
                _currentQualitySettingIndex = overallQualitySettings.Length - 1;
            }
        }
        else
        {
            if(_currentQualitySettingIndex < overallQualitySettings.Length - 1)
            {
                _currentQualitySettingIndex++;
            }
            else
            {
                _currentQualitySettingIndex = 0;
            }
        }

        videoSettings.currentQualityText.text = overallQualitySettings[_currentQualitySettingIndex].settingText;

        QualitySettings.SetQualityLevel(_currentQualitySettingIndex);
        QualitySettings.shadowResolution = overallQualitySettings[_currentQualitySettingIndex].shadowResolution;

        // Set shadow quality index to overall quality index since they are 1:1
        _currentShadowQualitySettingIndex = _currentQualitySettingIndex;
        videoSettings.currentShadowQualityText.text = shadowQualitySettings[_currentShadowQualitySettingIndex].settingText;

        PlayerPrefs.SetInt("QualitySettingIndex", _currentQualitySettingIndex);

        if (!_controllerManager.xboxControllerIsConnected)
        {
            SettingsButtonHelper();
        }

        if (!_controllerManager.xboxControllerIsConnected)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ChangeShadowQuality(int direction)
    {
        if (direction < 0)
        {
            if(_currentShadowQualitySettingIndex > 0)
            {
                _currentShadowQualitySettingIndex--;
            }
            else
            {
                _currentShadowQualitySettingIndex = shadowQualitySettings.Length - 1;
            }
            
        }
        else
        {
            if(_currentShadowQualitySettingIndex < shadowQualitySettings.Length - 1)
            {
                _currentShadowQualitySettingIndex++;
            }
            else
            {
                _currentShadowQualitySettingIndex = 0;
            }
            
        }

        videoSettings.currentShadowQualityText.text = shadowQualitySettings[_currentShadowQualitySettingIndex].settingText;

        QualitySettings.shadowResolution = shadowQualitySettings[_currentShadowQualitySettingIndex].shadowResolution;
        PlayerPrefs.SetInt("ShadowQualitySettingIndex", _currentShadowQualitySettingIndex);

        if (!_controllerManager.xboxControllerIsConnected)
        {
            SettingsButtonHelper();
        }

        if (!_controllerManager.xboxControllerIsConnected)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void TogglePostProcessingOption(int direction)
    {
        if (direction < 0)
        {
            if(_currentPostProcessingIndex > 0)
            {
                _currentPostProcessingIndex--;
            }
            else
            {
                _currentPostProcessingIndex = 1;
            }
            
        }
        else
        {
            if(_currentPostProcessingIndex < 1)
            {
                _currentPostProcessingIndex++;
            }
            else
            {
                _currentPostProcessingIndex = 0;
            }
        }

        videoSettings.currentPostProcessingOptionText.text = postProcessingOptions[_currentPostProcessingIndex].settingText;
        _postProcessingManager.SetPostProcessing(postProcessingOptions[_currentPostProcessingIndex].isEnabled);

        PlayerPrefs.SetInt("PostProcessingOptionIndex", _currentPostProcessingIndex);

        if (!_controllerManager.xboxControllerIsConnected)
        {
            SettingsButtonHelper();
        }

        if (!_controllerManager.xboxControllerIsConnected)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void BrightnessValueChanged()
    {
        _brightness.brightness = brightnessSlider.value;
    }

    [System.Serializable]
    public class VideoSettings
    {
        public Button[] resSettingButtons;
        public TextMeshProUGUI currentResolutionText;

        public Button[] qualitySettingButtons;
        public TextMeshProUGUI currentQualityText;

        public Button[] shadowQualitySettingButtons;
        public TextMeshProUGUI currentShadowQualityText;

        public Button[] postProcessOptionButtons;
        public TextMeshProUGUI currentPostProcessingOptionText;
    }

    [System.Serializable]
    public class ResolutionOptions
    {
        public int width;
        public int height;
        public int refreshRate;
    }

    [System.Serializable]
    public class OverallQualitySettings
    {
        public string settingText;
        public ShadowResolution shadowResolution;
    }

    [System.Serializable]
    public class ShadowQualitySettings
    {
        public string settingText;
        public ShadowResolution shadowResolution;
    }

    [System.Serializable]
    public class PostProcessingOption
    {
        public string settingText;
        public bool isEnabled;
    }
}
