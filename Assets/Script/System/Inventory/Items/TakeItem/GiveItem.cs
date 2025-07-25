using Fusion;
using UnityEngine;

public class GiveItem : NetworkBehaviour
{
    [SerializeField] private Items items;
    public Items Items { get { return items; } }

    public void UpItem(PlayerCharecter playerCharecter)
    {
        if (playerCharecter == null) return;
        Debug.Log($"Предмет {gameObject.name} Поднят ");
        playerCharecter.CharacterData.Inventory.RPC_AddItem((int)items);
        Runner.Despawn(Object);
    }

}
//public GameObject _uiItem;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!HasStateAuthority) return;//Код срабатывает только у хоста 

//        var playerData = other.gameObject.GetComponent<PlayerCharecter>();// получаем данные игрока который коснулся 
//        if (playerData != null)
//        {
//            if (_uiItem.TryGetComponent(out IUiItem uiItem))
//            {
//                playerData.UpItemTest(Object, playerData.Object.InputAuthority);// Отправляем вызов 
//            }
//            Runner.Despawn(Object);// после завершения уничтожаем 
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
