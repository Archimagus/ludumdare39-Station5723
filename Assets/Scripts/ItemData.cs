using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
	[SerializeField]
	[Tooltip("Is this item visible in the inventory")]
	private bool _hidden;
	[SerializeField]
	private Sprite _icon;

	public bool Hidden { get { return _hidden; } }
	public Sprite Icon { get { return _icon; } }
}