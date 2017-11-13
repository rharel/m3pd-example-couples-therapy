using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemSelectionDialog))]
public class ItemSelectionDialogTester: MonoBehaviour
{
	void Awake()
	{
		_dialog = GetComponent<ItemSelectionDialog>();
	}
	void Start()
	{
		var items = new KeyValuePair<string, object>[]
		{
			new KeyValuePair<string, object>("hello", "world"),
			new KeyValuePair<string, object>("what", "a"),
			new KeyValuePair<string, object>("beautiful", "day"),
			new KeyValuePair<string, object>("this is a really really really long item", "indeed")
		};
		_dialog.SetItems(items);
		_dialog.ItemSelected += OnItemSelected;
	}

	private void OnItemSelected(
		ItemSelectionDialog _, 
		KeyValuePair<string, object> item)
	{
		Debug.Log(
			"Selected key '" + item.Key + 
			"' with value: '" + item.Value + "'."
		);
	}

	private ItemSelectionDialog _dialog;
}
