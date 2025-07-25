using System.Collections.Generic;
using UnityEngine;

public class ManagerItems : MonoBehaviour
{
    [SerializeField] private List<UiItem> _listUiItem;

    private void Awake()
    {
        if (_listUiItem == null)
        {
            Debug.LogError($" List<UiItem> - Not Found {gameObject.name} - ManagerItems");
        }
    }

    public UiItem GetUiItem(int Items)
    {
        UiItem currentItem = null;

        if (_listUiItem == null) return currentItem;

        foreach (var item in _listUiItem)
        {
            if (Items == (int)item.Items)
            {
                currentItem = item;
            }
        }
        return currentItem;
    }
}
