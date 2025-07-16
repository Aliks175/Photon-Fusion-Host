using Fusion;
using UnityEngine;

public class GiveItem : NetworkBehaviour
{
    public GameObject _uiItem;

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;//Код срабатывает только у хоста 

        var playerData = other.gameObject.GetComponent<PlayerCharecter>();// получаем данные игрока который коснулся 
        if (playerData != null)
        {
            if (_uiItem.TryGetComponent(out IUiItem uiItem))
            {
                playerData.UpItemTest(Object, playerData.Object.InputAuthority);// Отправляем вызов 
            }
            Runner.Despawn(Object);// после завершения уничтожаем 
        }
    }

    private void OnValidate()
    {
        if (_uiItem.GetComponent<IUiItem>() == null)
        {
            Debug.LogError($"Not found UiItem {this.name} ");
        }
    }
}
