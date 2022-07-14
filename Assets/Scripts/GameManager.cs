using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;

    [Header("Game-Variables-Player")]
    public float speedPlayers = 5f;

    [Header("Game-Variables-Bomb")]
    public int bombAmount = 1;
    public int explosionRadius = 1;
    public float bombTimer = 3f;
    public float explosionDuration = 1f;
    public LayerMask explosionLayerMask;
    public Tilemap destructibleTiles;

    [Header("Game-Variables-Destructible")]
    public float destructionTime = 1f;
    [Range(0f, 1f)]
    public float spawnChance = 0.2f;
    public GameObject[] spawnItems;

    public void CheckWinState()
    {
        int aliveCount = 0;

        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                aliveCount++;
            }
        }

        if (aliveCount <= 1)
        {
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
