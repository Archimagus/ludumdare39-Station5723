using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
	[SerializeField]
	private Slider _musicSlider;
	[SerializeField]
	private Slider _interfaceSlider;
	[SerializeField]
	private Slider _effectSlider;

	private float _musicVolume;
	private float _interfaceVolume;
	private float _effectsVolume;

	AudioManager _audioManager;

	void Start ()
	{
		_audioManager = FindObjectOfType<AudioManager>();

		_musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		_interfaceVolume = PlayerPrefs.GetFloat("InterfaceVolume", 0.5f);
		_effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 0.5f);

		SetValues();
	}

	public void SetValues()
	{
		_musicSlider.value = _musicVolume;
		_interfaceSlider.value = _interfaceVolume;
		_effectSlider.value = _effectsVolume;

		_audioManager.mainMixer.SetFloat("MusicVolume", AudioExtension.LinearToDecibel(_musicVolume));
		_audioManager.mainMixer.SetFloat("InterfaceVolume", AudioExtension.LinearToDecibel(_interfaceVolume));
		_audioManager.mainMixer.SetFloat("SoundVolume", AudioExtension.LinearToDecibel(_effectsVolume));
	}

	public void SaveValues()
	{
		PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
		PlayerPrefs.SetFloat("InterfaceVolume", _interfaceVolume);
		PlayerPrefs.SetFloat("EffectsVolume", _effectsVolume);
	}

	public void MusicSliderChanged()
	{
		_musicVolume = _musicSlider.value;
		_audioManager.mainMixer.SetFloat("MusicVolume", AudioExtension.LinearToDecibel(_musicVolume));
	}

	public void InterfaceSliderChanged()
	{
		_interfaceVolume = _interfaceSlider.value;
		_audioManager.mainMixer.SetFloat("InterfaceVolume", AudioExtension.LinearToDecibel(_interfaceVolume));
	}

	public void EffectSliderChanged()
	{
		_effectsVolume = _effectSlider.value;
		_audioManager.mainMixer.SetFloat("SoundVolume", AudioExtension.LinearToDecibel(_effectsVolume));
	}
}
