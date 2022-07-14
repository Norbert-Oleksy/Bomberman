using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteamPickUp : MonoBehaviour
{
    public GameObject bomb;
    //sekcja odpowiedzialna za typ podnoszonego itemu
    public enum ItemType
    {
        MoreBombs,
        Speed,
        ExplosionExpand,
        PackDynamites,
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
                player.GetComponent<PlayerControl>().SetPlayerSpeed(1);
                break;
            case ItemType.ExplosionExpand:
               // player.GetComponent<PlantBomb>().SetBombRadious(1);
                break;
            case ItemType.PackDynamites:
                player.GetComponent<PlantBomb>().bombPrefebSpecial = bomb;
                player.GetComponent<PlantBomb>().special = true;
                break;
        }

        Destroy(gameObject);
    }
}
