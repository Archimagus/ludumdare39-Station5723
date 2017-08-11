using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Item : MonoBehaviour, IInteractable
{
	[SerializeField]
	private ItemData _data;
	public ItemData Data => _data;



	public bool CanInteract(Player p) => true;

	bool IInteractable.Interact(Player p)
	{
		print("Interacting with " + _data.name);
		p.AddItem(_data);
		Destroy(gameObject);
		return true;
	}
}
