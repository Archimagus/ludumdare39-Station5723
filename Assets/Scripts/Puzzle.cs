using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

[SelectionBase]
public class Puzzle : MonoBehaviour, IInteractable
{
	[SerializeField]
	private ItemData[] _requiredItems;
	[Header("OnComplete")]
	[SerializeField]
	private ItemData _reward;
	[SerializeField]
	private Animator []_playAnimationsOnComplete;
	[SerializeField]
	private string[] _customAnimationTriggers;
	[SerializeField]
	private bool _swapPrefabOnComplete;
	[SerializeField][Tooltip("Leave empty to delete")]
	private GameObject _replacementPrefab;
	[SerializeField]
	private AudioClip _playSoundOnComplete;
	[SerializeField]
	private string _hintMessage = "";
	[SerializeField]
	private float _hintTime = 3.0f;
	private GameplayUIManager _gameplayUIManager;

	public bool Complete { get; private set; }


	public ReadOnlyCollection<ItemData> RequiredItems => Array.AsReadOnly( _requiredItems);

	public bool CanInteract(Player p) => Complete == false && (_requiredItems?.All(i => p.Inventory.Contains(i)) ?? true);

	private void Awake()
	{
		_gameplayUIManager = FindObjectOfType<GameplayUIManager>();
	}

	public bool Interact(Player p)
	{
		if (!CanInteract(p))
		{
			print("Can't Interact with " + name);
			StartCoroutine(_gameplayUIManager.DisplaySubtitle(_hintMessage, _hintTime));
			return false;
		}

		print("Interacting with " + name);
		Complete = true;
		gameObject.PlaySoundEffect(_playSoundOnComplete, sourceObject:p.gameObject);
		if(_reward != null)
			p.AddItem(_reward);
		if(!_playAnimationsOnComplete.IsNullOrEmpty())
		{
			for (int i = 0; i < _playAnimationsOnComplete.Length; i++)
			{
				string trigger = "Play";
				if(_customAnimationTriggers?.Length>i)
				{
					trigger = _customAnimationTriggers[i];

				}
				_playAnimationsOnComplete[i].SetTrigger(trigger);
			}
		}
		if(_swapPrefabOnComplete)
		{
			if (_replacementPrefab != null)
			{
				if (_replacementPrefab.scene.name == null)
					Instantiate(_replacementPrefab, transform.position, transform.rotation, transform.parent);
				else
					_replacementPrefab.SetActive(true);
			}
			gameObject.SetActive(false);
		}
		return true;
	}

}