using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AtomicBomb : MonoBehaviour
{
    public GameObject gameLogic;
    private float bombTimer = 10f;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public LayerMask explosionLayerMask;
    private float explosionDuration = 1f;
    private int explosionRadius = 3;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic");
        explosionDuration = gameLogic.GetComponent<GameManager>().explosionDuration;
        explosionLayerMask = gameLogic.GetComponent<GameManager>().explosionLayerMask;
        destructibleTiles = gameLogic.GetComponent<GameManager>().destructibleTiles;

        StartCoroutine(PlantingBomb());
    }

    private IEnumerator PlantingBomb()
    {
        yield return new WaitForSeconds(bombTimer);
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);
        Destroy(explosion.gameObject, explosionDuration);
        Destroy(gameObject);
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
            ClearDestructible(position, length);
        }
        else if (length > 0)
        {
            GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            Destroy(explosion.gameObject, explosionDuration);
            Explode(position, Vector2.up, length-1);
            Explode(position, Vector2.down, length - 1);
            Explode(position, Vector2.left, length - 1);
            Explode(position, Vector2.right, length - 1);
        }
    }

    private void ClearDestructible(Vector2 position, int length)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }

        Explode(position, Vector2.up, length - 1);
        Explode(position, Vector2.down, length - 1);
        Explode(position, Vector2.left, length - 1);
        Explode(position, Vector2.right, length - 1);
    }
}
