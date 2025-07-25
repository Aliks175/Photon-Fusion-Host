using Fusion;
using UnityEngine;

public class PickUpPlayer : NetworkBehaviour
{
    private PlayerCharecter playerCharecter;

    private void Awake()
    {
        playerCharecter = GetComponent<PlayerCharecter>();
        if (playerCharecter == null)
        {
            Debug.LogError($"Miss PlayerCharecter - {gameObject.name} - PickUpPlayer");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority == false) return;

        if (other.gameObject.TryGetComponent(out GiveItem giveItem))
        {
            Debug.Log($"{gameObject.name} Поднимает {giveItem.gameObject.name}");
            giveItem.UpItem(playerCharecter);
        }
    }
}
