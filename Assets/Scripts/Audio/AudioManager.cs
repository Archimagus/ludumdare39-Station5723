using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	private int _activeSource = 0;
	private AudioSource[] _musicSources;
	private AudioSource _interfaceSource;

	[SerializeField]
	private float _crossFadeTime=1f;

	public AudioSource InterfaceSource
	{
		get { return _interfaceSource; }
	}

	[HideInInspector]
	public AudioMixer mainMixer;
	[HideInInspector]
	public AudioMixerGroup musicMixerGroup;
	[HideInInspector]
	public AudioMixerGroup soundMixerGroup;
	[HideInInspector]
	public AudioMixerGroup interfaceMixerGroup;

	public AudioClip[] soundClips;
	public AudioClip[] musicClips;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	private void Awake()
	{
		if (mainMixer == null)
		{
			mainMixer = Resources.Load("MainMixer") as AudioMixer;
			musicMixerGroup = mainMixer.FindMatchingGroups("Music")[0];
			soundMixerGroup = mainMixer.FindMatchingGroups("Sound")[0];
			interfaceMixerGroup = mainMixer.FindMatchingGroups("Interface")[0];
		}
		if (_musicSources == null)
		{
			_musicSources = new AudioSource[2];
			for (int i = 0; i < _musicSources.Length; i++)
			{
				_musicSources[i] = gameObject.AddComponent<AudioSource>();
				_musicSources[i].loop = true;
				_musicSources[i].outputAudioMixerGroup = musicMixerGroup;
			}
		}
		if (_interfaceSource == null)
		{
			_interfaceSource = gameObject.AddComponent<AudioSource>();
			_interfaceSource.outputAudioMixerGroup = interfaceMixerGroup;
		}
		AudioExtension.Manager = this;
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	private void Start()
	{
		if (!_musicSources[_activeSource].isPlaying)
		{
			_activeSource = Mathf.Abs(_activeSource - 1);
			PlayMusic(Music.Menu);
		}
	}

	private IEnumerator CrossFadeMusic(AudioClip nextClip)
	{
		var oldSource = _activeSource;
		_activeSource = Mathf.Abs(_activeSource - 1);
		_musicSources[_activeSource].clip = nextClip;
		_musicSources[_activeSource].Play();

		float time = 0.0f;
		while (time <= _crossFadeTime)
		{
			time += Time.deltaTime;
			float progress = time / _crossFadeTime;
			_musicSources[oldSource].volume = Mathf.Lerp(1.0f, 0.0f, progress);
			_musicSources[_activeSource].volume = Mathf.Lerp(0.0f, 1.0f, progress);
			yield return null;
		}
		_musicSources[oldSource].Stop();
	}

	/// <summary>
	/// Crossfade current music into new music
	/// </summary>
	/// <param name="music">Music to be played</param>
	public void PlayMusic(Music music)
	{
		AudioClip nextClip = null;
		try
		{
			switch (music)
			{
				case Music.Menu:
					nextClip = musicClips.First(clip => clip.name == "mx_menu");
					break;

				case Music.Level:
					nextClip = musicClips.First(clip => clip.name == "mx_level");
					break;

				case Music.GameLost:
					nextClip = musicClips.First(clip => clip.name == "mx_gamelost");
					break;

				case Music.GameWon:
					nextClip = musicClips.First(clip => clip.name == "mx_gamewon");
					break;

				default:
					Debug.LogWarning("Music enum not found: " + music.ToString());
					return;
			}
		}
		catch (System.NullReferenceException)
		{
			Debug.LogWarning("Tried to play a null clip: " + music.ToString());
		}

		if (nextClip != null)
		{
			StartCoroutine(CrossFadeMusic(nextClip));
		}
	}
}

public enum Music
{
	Menu,
	Level,
	GameLost,
	GameWon
}
