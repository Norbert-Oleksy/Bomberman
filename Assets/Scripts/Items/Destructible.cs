using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject gameLogic;

    private float destructionTime = 1f;
    public float spawnChance;
    private GameObject[] spawnItems;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");

        //GameLogic
        spawnChance = gameLogic.GetComponent<GameManager>().spawnChance;
        destructionTime = gameLogic.GetComponent<GameManager>().destructionTime;
        spawnItems = gameLogic.GetComponent<GameManager>().spawnItems;
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        if(spawnChance > 0 && Random.value < spawnChance)
        {
            int index = Random.Range(0, spawnItems.Length);
            Instantiate(spawnItems[index], transform.position, Quaternion.identity);
        }
    }
}
