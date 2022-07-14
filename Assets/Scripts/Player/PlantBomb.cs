using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantBomb : MonoBehaviour
{
    public GameObject gameLogic;

    [Header("Bomb")]
    public GameObject bombPrefeb;
    public GameObject bombPrefebSpecial;
    public bool special = false;
    private float bombTimer = 3f;
    private int bombAmount = 1;
    private int bombRemain;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public LayerMask explosionLayerMask;


    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    public KeyCode bombPlant = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        //GameLogic
        bombAmount = gameLogic.GetComponent<GameManager>().bombAmount;
        bombTimer = gameLogic.GetComponent<GameManager>().bombTimer;

        bombRemain = bombAmount;
        bombPrefebSpecial = bombPrefeb;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(bombPlant) && bombRemain>0)
        {
            StartCoroutine(PlantingBomb());
        }
    }

    private IEnumerator PlantingBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        if (special)
        {
            GameObject bomb = Instantiate(bombPrefebSpecial, position, Quaternion.identity);
            special = false;
        }
        else
        {
            GameObject bomb = Instantiate(bombPrefeb, position, Quaternion.identity);
        }
        bombRemain--;

        yield return new WaitForSeconds(bombTimer);

        bombRemain++;
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        if(obj.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            obj.isTrigger = false;
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombRemain++;
    }

    /*public void SetBombRadious(int val)
    {
        explosionRadius += val;
    }*/
}
