using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
	public int KeyNum;
	KeyPad _keyPad;

	private void Start()
	{
		_keyPad = GetComponentInParent<KeyPad>();
		
	}
	public bool CanInteract(Player p)
	{
		return true;
	}

	public bool Interact(Player p)
	{
		_keyPad.KeyClicked(this, p);
		return true;
	}
}
