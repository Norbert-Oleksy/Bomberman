using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Detonator : MonoBehaviour
{
    public GameManager GameManager;
    private float bombTimer = 2f;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public LayerMask explosionLayerMask;
    private float explosionDuration = 1f;
    private int explosionRadius = 30;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        explosionDuration = GameManager.explosionDuration;
        explosionLayerMask = GameManager.explosionLayerMask;
        destructibleTiles = GameManager.destructibleTiles;

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
