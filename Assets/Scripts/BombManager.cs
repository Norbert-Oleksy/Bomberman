using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BombManager : MonoBehaviour
{

    public GameObject gameLogic;

    [Header("Bomb")]
    public GameObject bombPrefeb;
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

    // Start is called before the first frame update
    void Start()
    {
        //GameLogic
        bombAmount = gameLogic.GetComponent<GameManager>().bombAmount;
        explosionRadius = gameLogic.GetComponent<GameManager>().explosionRadius;
        bombTimer = gameLogic.GetComponent<GameManager>().bombTimer;
        explosionDuration = gameLogic.GetComponent<GameManager>().explosionDuration;

        bombRemain = bombAmount;
    }

    public void PlantBomb(Vector2 position)
    {
        StartCoroutine(PlantingBomb(position));
    }

    private IEnumerator PlantingBomb(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefeb, position, Quaternion.identity);
        bombRemain--;

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
}
