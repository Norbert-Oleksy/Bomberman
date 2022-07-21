using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantBomb : MonoBehaviour
{
    public GameManager GameManager;

    public bool saftyFirst = false;

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
    private float explosionDuration = 1f;
    private int explosionRadius = 1;


    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    public KeyCode bombPlant = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        //GameLogic
        GameManager = FindObjectOfType<GameManager>();
        explosionRadius = GameManager.explosionRadius;
        bombTimer = GameManager.bombTimer;
        explosionDuration = GameManager.explosionDuration;
        explosionLayerMask = GameManager.explosionLayerMask;
        destructibleTiles = GameManager.destructibleTiles;
        bombAmount = GameManager.bombAmount;

        bombRemain = bombAmount;
        bombPrefebSpecial = bombPrefeb;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(bombPlant) && bombRemain>0 && !saftyFirst)
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
            bombRemain--;
            yield return new WaitForSeconds(bombTimer);
            bombRemain++;
        }
        else
        {
            bombRemain--;

            GameObject bomb = Instantiate(bombPrefeb, position, Quaternion.identity);

            yield return new WaitForSeconds(bombTimer);
            position = bomb.transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            Explode(position, Vector2.up, explosionRadius);
            Explode(position, Vector2.down, explosionRadius);
            Explode(position, Vector2.left, explosionRadius);
            Explode(position, Vector2.right, explosionRadius);

            Destroy(explosion.gameObject, explosionDuration);
            Destroy(bomb);
            bombRemain++;
        }

    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            obj.isTrigger = false;
        }
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        position += direction;

        if (length > 0 && Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
        }
        else if (length > 0)
        {
            GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            Destroy(explosion.gameObject, explosionDuration);
            Explode(position, direction, length - 1);
        }
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombRemain++;
    }

    public void SetBombRadious(int val)
    {
        explosionRadius += val;
    }
}
