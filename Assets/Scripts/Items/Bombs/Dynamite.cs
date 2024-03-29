using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dynamite : MonoBehaviour
{
    public GameManager GameManager;

    private float bombTimer = 1f;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public LayerMask explosionLayerMask;
    private float explosionDuration = 1f;

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

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
        }
        else
        {
            GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            Destroy(explosion.gameObject, explosionDuration);
        }
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            obj.isTrigger = false;
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
