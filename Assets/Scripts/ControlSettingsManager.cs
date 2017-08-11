using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettingsManager : MonoBehaviour
{
    [SerializeField]
    private Slider _xSensitivitySlider;
    [SerializeField]
    private Slider _ySensitivitySlider;

    public float xValue;
    public float yValue;

    void Start ()
    {
        xValue = PlayerPrefs.GetFloat("HorizontalSensitivitySetting", 2.0f);
        yValue = PlayerPrefs.GetFloat("VerticalSensitivitySetting", 2.0f);

        SetSliderValues();
    }
	
    private void SetSliderValues()
    {
        _xSensitivitySlider.value = xValue;
        _ySensitivitySlider.value = yValue;
    }

    public void HorizontalSensitivityChanged()
    {
        xValue = _xSensitivitySlider.value;
    }

    public void VerticalSensitivityChanged()
    {
        yValue = _ySensitivitySlider.value;
    }

    public void SaveValues()
    {
        PlayerPrefs.SetFloat("HorizontalSensitivitySetting", xValue);
        PlayerPrefs.SetFloat("VerticalSensitivitySetting", yValue);
    }
}
