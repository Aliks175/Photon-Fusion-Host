using Fusion;
using UnityEngine;

public class GiveItem : NetworkBehaviour
{
    [SerializeField] private Items items;
    public Items Items { get { return items; } }

    public void UpItem(PlayerCharecter playerCharecter)
    {
        if (playerCharecter == null) return;
        Debug.Log($"������� {gameObject.name} ������ ");
        playerCharecter.CharacterData.Inventory.RPC_AddItem((int)items);
        Runner.Despawn(Object);
    }

}
//public GameObject _uiItem;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!HasStateAuthority) return;//��� ����������� ������ � ����� 

//        var playerData = other.gameObject.GetComponent<PlayerCharecter>();// �������� ������ ������ ������� �������� 
//        if (playerData != null)
//        {
//            if (_uiItem.TryGetComponent(out IUiItem uiItem))
//            {
//                playerData.UpItemTest(Object, playerData.Object.InputAuthority);// ���������� ����� 
//            }
//            Runner.Despawn(Object);// ����� ���������� ���������� 
//        }
//    }

//    private void OnValidate()
//    {
//        if (_uiItem.GetComponent<IUiItem>() == null)
//        {
//            Debug.LogError($"Not found UiItem {this.name} ");
//        }
//    }
//}

public enum Items
{
    Apple,
    Cheese,
    Bread,
    Bomb,
    HealthPoint
}
