using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private LayerMask _interactableLayers;
	private List<ItemData> _inventory = new List<ItemData>();
	private PauseManager _pauseManager;
	public float _reach = 2;

	public ReadOnlyCollection<ItemData> Inventory => _inventory?.AsReadOnly();

	public event EventHandler<InventoryChangeEventArgs> InventoryChanged;

	internal void AddItem(ItemData item)
	{
		print("Adding Inventory Item: " + item.name);
		_inventory.Add(item);
		InventoryChanged?.Invoke(this, new InventoryChangeEventArgs(CollectionChangeAction.Add, item));
	}
	internal void RemoveItem(ItemData item)
	{
		print("Removing Inventory Item: " + item.name);
		_inventory.Remove(item);
		InventoryChanged?.Invoke(this, new InventoryChangeEventArgs(CollectionChangeAction.Remove, item));
	}

	private void Awake()
	{
		if (_pauseManager == null)
		{
			_pauseManager = FindObjectOfType<PauseManager>();
		}
	}

	private void Update()
	{
		if(Input.GetButtonDown("Interact") && !_pauseManager.gameIsPaused)
		{
			var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, _reach, _interactableLayers))
			{
				var interactable = hit.transform.GetComponentInParent<IInteractable>();

				if(interactable?.Interact(this)??false)
				{
					CheckInventory();
				}
			}
		}
	}

	private void CheckInventory()
	{
		var puzzles = GameManager.AllPuzzles;

		foreach (var item in Inventory.ToArray())
		{
			if (puzzles.Any(p => p.RequiredItems.Any(i => i == item) && p.Complete == false))
				continue;
			RemoveItem(item);
		}
	}
}

public class InventoryChangeEventArgs : EventArgs
{
	public InventoryChangeEventArgs(CollectionChangeAction action, ItemData element)
	{
		Action = action;
		Element = element;
	}

	public virtual CollectionChangeAction Action { get; }
	public virtual ItemData Element { get; }
}
