using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;
    public GameObject GameUi;

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

    void Start()
    {
        StartCoroutine(StartSequence());
    }

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
            GameUi.GetComponent<GameUI>().InfoText.SetActive(true);
            GameUi.GetComponent<GameUI>().InfoText.GetComponent<TextMeshProUGUI>().text = "Game Over";
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator StartSequence()
    {
        GameUi.GetComponent<GameUI>().InfoText.SetActive(true);
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerControl>().enabled = false;
            player.GetComponent<PlantBomb>().enabled = false;
        }
        GameUi.GetComponent<GameUI>().InfoText.GetComponent<TextMeshProUGUI>().text = "READY?";
        yield return new WaitForSeconds(2.0f);



        GameUi.GetComponent<GameUI>().InfoText.GetComponent<TextMeshProUGUI>().text = "GO!";
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerControl>().enabled = true;
            player.GetComponent<PlantBomb>().enabled = true;
        }
        yield return new WaitForSeconds(0.5f);
        GameUi.GetComponent<GameUI>().InfoText.SetActive(false);
    }
}
