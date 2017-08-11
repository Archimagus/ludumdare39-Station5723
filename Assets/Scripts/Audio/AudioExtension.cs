using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// Extension class for implementing audio
/// </summary>
public static class AudioExtension
{
	private static AudioManager _manager;

	/// <summary>
	/// Sets the _manager singleton. Should only be called by AudioManager.
	/// </summary>
	/// <param name="audioManager"></param>
	public static AudioManager Manager
	{
		set
		{
			_manager = value;
		}
	}

	public static int NumberOfSounds
	{
		get { return _manager.soundClips.Length; }
	}

	/// <summary>
	/// Get AudioClip by name
	/// </summary>
	/// <param name="name"></param>
	/// <returns>AudioClip with matching name, null if doesn't exist</returns>
	public static AudioClip GetSound(this GameObject self, string name)
	{
		try
		{
			return _manager.soundClips.First(clip => clip.name == name);
		}
		catch (System.NullReferenceException)
		{
			Debug.LogWarning("Clip doesn't exist: " + name);
			return null;
		}
	}

	/// <summary>
	/// Get Audioclip by index
	/// </summary>
	/// <param name="index"></param>
	/// <returns>AudioClip at index, null if doesn't exist</returns>
	public static AudioClip GetSound(this GameObject self, int index)
	{
		try
		{
			return _manager.soundClips[index];
		}
		catch (System.NullReferenceException)
		{
			Debug.LogWarning("Clip doesn't exist: clips[" + index + "]");
			return null;
		}
	}

	/// <summary>
	/// Play AudioClip on sourceObject. If not souceObject is given, plays on AudioManager.
	/// </summary>
	/// <param name="self">Reference to  calling GameObject</param>
	/// <param name="clip">AudioClip to be played</param>
	/// <param name="volume">Volume to play clip (0.0 to 1.0)</param>
	/// <param name="sourceObject">GameObject to hold AudioSource to play the AudioClip</param>
	public static void PlaySoundEffect(this GameObject self, AudioClip clip, float volume=1.0f,  GameObject sourceObject=null)
	{
		if (clip == null) return;
		if (_manager == null)
		{
			_manager = GameObject.FindObjectOfType<AudioManager>();
		}
		var go = sourceObject != null ? sourceObject : _manager.gameObject;
		AudioSource source = go.AddComponent<AudioSource>();
		source.outputAudioMixerGroup = _manager.soundMixerGroup;
		source.clip = clip;
		source.volume = volume;
		source.Play();
		DestroySource(source);
	}

	/// <summary>
	/// Play a clip in a loop
	/// </summary>
	/// <param name="self"></param>
	/// <param name="clip"></param>
	/// <param name="volume"></param>
	/// <returns>AudioSource of loop clip, null if clip is null</returns>
	public static AudioSource PlaySoundEffectLoop(this GameObject self, AudioClip clip, float volume=1.0f, GameObject sourceObject=null)
	{
		if (clip == null) return null;
		if (_manager == null)
		{
			_manager = GameObject.FindObjectOfType<AudioManager>();
		}
		var go = sourceObject != null ? sourceObject : _manager.gameObject;
		AudioSource source = go.AddComponent<AudioSource>();
		source.outputAudioMixerGroup = _manager.soundMixerGroup;
		source.clip = clip;
		source.volume = volume;
		source.loop = true;
		source.Play();
		return source;
	}

	/// <summary>
	/// Stop playing a looped clip and clean up its AudioSource
	/// </summary>
	/// <param name="self"></param>
	/// <param name="source"></param>
	public static void StopSoundEffectLoop(this GameObject self, AudioSource source)
	{
		source.Stop();
		DestroySource(source);
	}

	private static IEnumerator DestroySource(AudioSource source)
	{
		while (source.isPlaying)
		{
			yield return null;
		}
		Object.Destroy(source);
	}

	/// <summary>
	/// Play AudioClip on AudioManager.
	/// </summary>
	/// <param name="self">Reference to  calling GameObject</param>
	/// <param name="clip">AudioClip to be played</param>
	/// <param name="volume">Volume to play clip (0.0 to 1.0)</param>
	public static void PlayInterfaceSound(this GameObject self, AudioClip clip, float volume=1.0f)
	{
		if (clip == null) return;
		if (_manager == null)
		{
			_manager = GameObject.FindObjectOfType<AudioManager>();
		}
		_manager.InterfaceSource.PlayOneShot(clip, volume);
	}

	/// <summary>
	/// Change currently playing music
	/// </summary>
	/// <param name="self">Reference to calling GameObject</param>
	/// <param name="music">Enum of music to be played</param>
	public static void ChangeMusic(this GameObject self, Music music)
	{
		if (_manager == null)
		{
			_manager = GameObject.FindObjectOfType<AudioManager>();
		}
		_manager.PlayMusic(music);
	}

	/// <summary>
	/// Take a volume in linear (0 to 1) space and convert it to dB (-80 to 0)
	/// </summary>
	/// <param name="lin">The volume in linear space</param>
	/// <returns>the volume in dB space</returns>
	public static float LinearToDecibel(this float lin)
	{
		if (lin <= float.Epsilon)
			return -80;
		return Mathf.Log(lin, 3) * 20;
	}

	/// <summary>
	/// Take a volume in dB (-80 to 0) space and convert it to linear (0 to 1)
	/// </summary>
	/// <param name="lin">The volume in dB space</param>
	/// <returns>the volume in linear space</returns>
	public static float DecibelToLinear(this float db)
	{
		return Mathf.Pow(3, db / 20);
	}
}
