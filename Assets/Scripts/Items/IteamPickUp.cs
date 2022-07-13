using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteamPickUp : MonoBehaviour
{
    //sekcja odpowiedzialna za typ podnoszonego itemu
    public enum ItemType
    {
        MoreBombs,
        Speed,
        ExplosionExpand,
    }
    public ItemType type;

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Player")){
            PickUp(obj.gameObject);
        }
    }

    private void PickUp(GameObject player)
    {
        switch (type)
        {
            case ItemType.MoreBombs:
                player.GetComponent<PlantBomb>().AddBomb();
                break;
            case ItemType.Speed:
                player.GetComponent<PlayerControl>().speed++;
                break;
            case ItemType.ExplosionExpand:
                player.GetComponent<PlantBomb>().explosionRadius++;
                break;
        }

        Destroy(gameObject);
    }
}
