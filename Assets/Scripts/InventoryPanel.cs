using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
	[SerializeField]
	Image _inventoryItemPrefab;
	Player _player;

	// Update is called once per frame
	void Update()
	{
		if(_player==null)
		{
			_player = FindObjectOfType<Player>();
			Debug.Assert(_player != null);
			_player.InventoryChanged += _player_InventoryChanged;
		}
	}

	private void _player_InventoryChanged(object sender, InventoryChangeEventArgs e)
	{
		if (e.Element.Hidden)
			return;
		while(transform.childCount > 0)
		{
			var child = transform.GetChild(0);
			child.SetParent(null);
			Destroy(child.gameObject);
		}
		foreach (var item in _player.Inventory)
		{
			if (!item.Hidden)
			{
				var child = Instantiate(_inventoryItemPrefab);
				child.sprite = item.Icon;
				child.transform.SetParent(transform, false);
			}
		}
	}
}
