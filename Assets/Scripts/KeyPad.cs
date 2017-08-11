using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyPad : MonoBehaviour
{
	public int[] _code;
	public ItemData _reward;
	[SerializeField]
	private Animator[] _playAnimationsOnComplete;
	[SerializeField]
	private string[] _customAnimationTriggers;
	[Space]
	public float _pushDist = 0.01f;
	public float _pushSpeed = 0.1f;

	TextMeshProUGUI _text;
	List<int> _keyPresses = new List<int>();
	Player _player;
	bool _complete;
	bool _playing;
	private void Start()
	{
		_text = GetComponentInChildren<TextMeshProUGUI>();
		_text.text = "";
	}

	internal void KeyClicked(Key key, Player p)
	{
		if (_complete)
			return;
		if (_playing)
			return;
		_playing = true;
		_player = p;
		gameObject.PlaySoundEffect(gameObject.GetSound("sfx_ui_keypad_press"));
		StartCoroutine(animateKeyPress(key));

	}
	IEnumerator animateKeyPress(Key key)
	{
		var tx = key.transform;
		var startPos = tx.position;
		Vector3 targetPos = tx.position - tx.up * _pushDist;
		while (Vector3.Distance(tx.position, targetPos) > 1e-6)
		{
			tx.position = Vector3.MoveTowards(tx.position, targetPos, _pushSpeed * Time.deltaTime);
			yield return null;
		}
		targetPos = startPos;
		_keyPresses.Add(key.KeyNum);
		_text.text = _text.text + key.KeyNum;

		while (Vector3.Distance(tx.position, targetPos) > 1e-6)
		{
			tx.position = Vector3.MoveTowards(tx.position, targetPos, _pushSpeed * Time.deltaTime);
			yield return null;
		}

		CheckCode();
		_playing = false;
	}

	private void CheckCode()
	{
		if (_keyPresses.Count < _code.Length)
			return;
		for (int k = 0; k < _keyPresses.Count; k++)
		{
			if(_keyPresses[k] != _code[k])
			{
				if (_text.text.Length == 4)
				{
					_text.text = "";
					_keyPresses.Clear();
				}
				gameObject.PlaySoundEffect(gameObject.GetSound("sfx_ui_keypad_bad"), sourceObject: gameObject);
				return;
			}
		}
		if (_text.text.Length == 4)
		{
			_text.text = "";
			_keyPresses.Clear();
		}
		// Code Correct
		gameObject.PlaySoundEffect(gameObject.GetSound("sfx_ui_keypad_good"), sourceObject: gameObject);
		_complete = true;
		_player.AddItem(_reward);
		if (!_playAnimationsOnComplete.IsNullOrEmpty())
		{
			for (int i = 0; i < _playAnimationsOnComplete.Length; i++)
			{
				string trigger = "Play";
				if (_customAnimationTriggers?.Length > i)
				{
					trigger = _customAnimationTriggers[i];

				}
				_playAnimationsOnComplete[i].SetTrigger(trigger);
			}
		}
	}
}
