using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float destructionTime = 1f;

    [Range(0f, 1f)]
    public float spawnChance = 0.2f;
    public GameObject[] spawnItems;

    // Start is called before the first frame update
    void Start()
    {
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
