using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game-Logic-Setings")]
    public ItemType mode;
    public enum ItemType
    {
        Arena,
        SinglePlayer,
    }

    public GameObject[] players;
    public GameObject GameUi;

    [Header("Game-Variables-Player")]
    public float speedPlayers = 5f;

    [Header("Game-Variables-Map")]
    public Tilemap destructibleTiles;
    public Tilemap arenaTiles;
    public TileBase boxTile;
    public int mapHight=4;
    public int mapWidth = 8;
    [Range(0f, 1f)]
    public float spawnBoxChance = 0.5f;

    [Header("Game-Variables-Bomb")]
    public int bombAmount = 1;
    public int explosionRadius = 1;
    public float bombTimer = 3f;
    public float explosionDuration = 1f;
    public LayerMask explosionLayerMask;

    [Header("Game-Variables-Destructible")]
    public float destructionTime = 1f;
    [Range(0f, 1f)]
    public float spawnChance = 0.2f;
    public GameObject[] spawnItems;

    [Header("Game-Variables")]
    private int score;
    public int points = 0;
    public float startTimer=180f;
    private float timer;
    private bool timerOn = false;
    private int stage = 1;

    private GameObject infoText;
    private GameObject timeText;

    void Start()
    {
        infoText = GameUi.GetComponent<GameUI>().InfoText;
        timeText = GameUi.GetComponent<GameUI>().TimeText;
        if (mode == ItemType.Arena) StartCoroutine(StartSequence());
        if (mode == ItemType.SinglePlayer)
        {
            MapGenerate();
            timer = startTimer;
            score = points;
            StartCoroutine(StageStart());
        }
    }
    void Update()
    {
        if(timerOn)Timer();
    }

    public void CheckWinState()
    {
        if (mode == ItemType.Arena)
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
                infoText.SetActive(true);
                infoText.GetComponent<TextMeshProUGUI>().text = "Game Over";
                Invoke(nameof(NewRound), 3f);
            }
        }
        if(mode == ItemType.SinglePlayer)
        {
            infoText.SetActive(true);
            infoText.GetComponent<TextMeshProUGUI>().text = "Game Over";
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator StageStart()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerControl>().enabled = false;
            player.GetComponent<PlantBomb>().enabled = false;
        }
        infoText.SetActive(true);
        infoText.GetComponent<TextMeshProUGUI>().text = "STAGE " + stage;
        yield return new WaitForSeconds(2.0f);

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerControl>().enabled = true;
            player.GetComponent<PlantBomb>().enabled = true;
        }
        infoText.SetActive(false);
        timerOn = true;
    }

    private IEnumerator StartSequence()
    {
        infoText.SetActive(true);
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerControl>().enabled = false;
            player.GetComponent<PlantBomb>().enabled = false;
        }
        infoText.GetComponent<TextMeshProUGUI>().text = "READY?";
        yield return new WaitForSeconds(2.0f);
        infoText.GetComponent<TextMeshProUGUI>().text = "GO!";
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerControl>().enabled = true;
            player.GetComponent<PlantBomb>().enabled = true;
        }
        yield return new WaitForSeconds(0.5f);
        infoText.SetActive(false);
    }

    public void MapGenerate()
    {
        Vector3Int cell = new Vector3Int();
        for (int y=0-mapHight;y<= mapHight; y++)
        {
            for (int x = 0 - mapWidth; x <= mapWidth; x++)
            {
                if (y >=-1 && y <= 1 && x >= -1 && x <= 1) continue;
                cell.Set(x, y-1, 0);
                TileBase tile = arenaTiles.GetTile(cell);
                if (tile != null) continue;
                if(spawnBoxChance > 0 && Random.value < spawnBoxChance) destructibleTiles.SetTile(cell, boxTile);
            }
        }
    }

    public void Timer()
    {
        if (timer <= 0)
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerControl>().enabled = false;
                player.GetComponent<PlantBomb>().enabled = false;
            }
            infoText.SetActive(true);
            infoText.GetComponent<TextMeshProUGUI>().text = "Game Over";
            Invoke(nameof(NewRound), 3f);
        }
        else
        {
            float min = Mathf.FloorToInt(timer / 60);
            float sec = Mathf.FloorToInt(timer % 60);
            timeText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00} : {1:00}", min, sec);
            timer -= Time.deltaTime;
        }
    }

    public void NextStage()
    {
        Destroy(GameObject.FindWithTag("Bomb"));
        Destroy(GameObject.FindWithTag("PickUp"));
        timerOn = false;
        score = points*(int)timer;
        timer = startTimer;
        foreach (GameObject player in players)
        {
            player.transform.position = new Vector3(0, 0, 0);
        }
        destructibleTiles.ClearAllTiles();
        MapGenerate();
        stage++;
        StartCoroutine(StageStart());
    }
}
