using Fusion;
using UnityEngine;

public class InventoryPlayer : MonoBehaviour
{
    private NetworkRunner _runner;
    public GameObject _inventaryPool;
    private ICharacterData _characterData;

    public void AddItem(GameObject item)
    {
        //item = item.GetComponent<GiveItem>()._uiItem;
        //if (item == null) return;
        //IUiItem uiItem = item.GetComponent<IUiItem>();
        //if (uiItem == null) return;
        //GameObject iconItem = Instantiate(item, _inventaryPool.transform, false);
        //iconItem.GetComponent<IUiItem>().Initialization(_characterData);
    }

    public void Initialization(ICharacterData characterData, GameObject inventaryPool, NetworkRunner runner)
    {
        // Здесь нужно будет что то придумать со списками 
        _characterData = characterData;
        _inventaryPool = inventaryPool;
        _runner = runner;
    }
}

public interface IUiItem
{
    public void Initialization(ICharacterData characterData);
    public abstract void UseItem();
}
