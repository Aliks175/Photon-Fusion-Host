using Fusion;
using UnityEngine;

public class InventoryPlayer : NetworkBehaviour
{
    //private NetworkRunner _runner;
    public ManagerItems _managerItems;
    public GameObject _inventaryPool;
    private ICharacterData _characterData;

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_AddItem(int Items)
    {
        UiItem TempItem = _managerItems.GetUiItem(Items);
        if (TempItem == null) { return; }
        var IUiItem = TempItem.GetComponent<IUiItem>();
        if (IUiItem == null) { return; }

        GameObject iconItem = Instantiate(TempItem.gameObject, _inventaryPool.transform, false);
        iconItem.GetComponent<IUiItem>().Initialization(_characterData);
    }

    public void Initialization(ICharacterData characterData)
    {
        // Здесь нужно будет что то придумать со списками 
        _characterData = characterData;
        _managerItems = GetComponent<ManagerItems>();
        //_runner = runner;

        _inventaryPool = GameObject.Find("InventaryPool");

        if (_inventaryPool == null)
        {
            Debug.LogError($"Not Found - inventaryPool - InventoryPlayer");
        }
    }
}

public interface IUiItem
{
    public void Initialization(ICharacterData characterData);
    public abstract void UseItem();
}
