using rharel.Debug;
using rharel.M3PD.Common.Delegates;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This behavior handles the UI of a dynamic item selection dialog, where
/// the user is able to select one item out of a collection.
/// </summary>
public class ItemSelectionDialog: MonoBehaviour
{
    /// <summary>
    /// The prefab to use as a representation of an item.
    /// </summary>
    public GameObject itemPrefab;

    /// <summary>
    /// Occurs after the last text in the queue has been typed.
    /// </summary>
    public event EventHandler<
            ItemSelectionDialog, 
            KeyValuePair<string, object>>
        ItemSelected = delegate { };

    /// <summary>
    /// Indicates whether the menu is currently active.
    /// </summary>
    public bool IsActive { get { return gameObject.activeSelf; } }
    /// <summary>
    /// Activates the menu.
    /// </summary>
    public void Show() { gameObject.SetActive(true); }
    /// <summary>
    /// Deactivates the menu.
    /// </summary>
    public void Hide() { gameObject.SetActive(false); }

    /// <summary>
    /// Clears the item collection.
    /// </summary>
    public void Clear()
    {
        // Destroy all children except the first, since the first is the 
        // dialog's title text object.
        for (int i = transform.childCount - 1; i > 0; --i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        _user_data_by_item.Clear();
    }
    /// <summary>
    /// Sets the item collection the user is able to choose from.
    /// </summary>
    /// <param name="items">An enumeration of the items.</param>
    /// <remarks>
    /// <paramref name="items"/> must not be null.
    /// </remarks>
    public void SetItems(IEnumerable<KeyValuePair<string, object>> items)
    {
        Require.IsNotNull(items);

        Clear();

        foreach (var item in items)
        {
            var go = Instantiate(itemPrefab);
            var button = go.GetComponent<Button>();

            button.GetComponentInChildren<Text>()
                  .text = item.Key;
            button.onClick.AddListener(() => OnItemSelected(item.Key));

            go.transform.SetParent(transform, false);

            _user_data_by_item.Add(item.Key, item.Value);
        }
    }

    void OnValidate()
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning("Missing an item prefab.");
        }
        else if (itemPrefab.GetComponent<Button>() == null)
        {
            Debug.LogWarning("The item prefab is missing a Button component.");
        }
    }

    private void OnItemSelected(string key)
    {
        ItemSelected(
            this, 
            new KeyValuePair<string, object>(
                key, 
                _user_data_by_item[key]
            )
        );
    }

    private readonly Dictionary<string, object> _user_data_by_item = (
        new Dictionary<string, object>()
    );
}
