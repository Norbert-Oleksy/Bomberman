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
    public GameUI GameUI;

    [Header("Game-Variables-Player")]
    public float speedPlayers = 5f;

    [Header("Game-Variables-Map")]
    public Tilemap destructibleTiles;
    public Tilemap arenaTiles;
    public TileBase boxTile;
    public TileBase otherTile;
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
    public int stage = 1;
    public List<GameObject> mobs;
    public GameObject enemyPrefeb;

    private GameObject infoText;
    private GameObject timeText;
    private GameObject pointsText;
    private GameObject scoreText;
    private GameObject stageText;

    void Start()
    {
        GameUI = FindObjectOfType<GameUI>();
        infoText = GameUI.InfoText;
        if (mode == ItemType.Arena) StartCoroutine(StartSequence());
        if (mode == ItemType.SinglePlayer)
        {
            stageText = GameUI.stageText;
            pointsText = GameUI.pointsText;
            scoreText = GameUI.scoreText;
            timeText = GameUI.TimeText;
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

    //Sprawdza czy gra siê koñczy
    public void CheckWinState(string msg="")
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
                if (msg != null && msg != "")
                {
                    infoText.GetComponent<TextMeshProUGUI>().text = msg;
                }
                else
                {
                    infoText.GetComponent<TextMeshProUGUI>().text = "Game Over";
                }
                Invoke(nameof(NewRound), 3f);
            }
        }

        if(mode == ItemType.SinglePlayer)
        {
            int aliveCount = 0;
            foreach (GameObject player in players)
            {
                if (player.activeSelf)
                {
                    aliveCount++;
                }
            }
            if (aliveCount <= 0)
            {
                timerOn = false;
                ScoreSummary();
                infoText.SetActive(true);
                infoText.GetComponent<TextMeshProUGUI>().text = "Game Over";

                if (PlayerPrefs.HasKey("SinglePlayerHighestScore"))
                {
                    if (PlayerPrefs.GetInt("SinglePlayerHighestScore",0) < score)
                    {
                        PlayerPrefs.SetInt("SinglePlayerHighestScore", score);
                    }

                }
                else
                {
                    PlayerPrefs.SetInt("SinglePlayerHighestScore", score);
                }
                
                if (PlayerPrefs.HasKey("SinglePlayerHighestStage"))
                {
                    if (PlayerPrefs.GetInt("SinglePlayerHighestStage",0) < stage)
                    {
                        PlayerPrefs.SetInt("SinglePlayerHighestStage", stage);
                    }

                }
                else
                {
                    PlayerPrefs.SetInt("SinglePlayerHighestStage", stage);
                }



                PlayerPrefs.Save();
                Invoke(nameof(NewRound), 3f);
            }else if (mobs.Count <= 0)
            {
                StartCoroutine(NextStage());
            }
        }
    }

    //£aduje jeszcze raz t¹ sam¹ scene
    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Odpowiada za pocz¹tek gry SinglePalyer
    private IEnumerator StageStart()
    {

        infoText.SetActive(true);
        infoText.GetComponent<TextMeshProUGUI>().text = "STAGE " + stage;
        yield return new WaitForSeconds(1.0f);
        infoText.SetActive(false);
        timerOn = true;
    }

    //Odpowiada za pocz¹tek gry Arena
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

    //Generuje boxy na mapie
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
        EnemysGenerate();
    }

    //Timer 
    public void Timer()
    {
        if (timer <= 0)
        {
            timerOn = false;
            ScoreSummary();
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

    //Odpowiada za przejœcie do nastêpnego etapu
    public IEnumerator NextStage()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PlantBomb>().saftyFirst = true;
        }
        timerOn = false;
        ScoreSummary();
        timer = startTimer;
        DestroyAllBombs();
        yield return new WaitForSeconds(2.0f);
        Destroy(GameObject.FindWithTag("PickUp"));
        destructibleTiles.ClearAllTiles();        
        foreach (GameObject player in players)
        {
            player.transform.position = new Vector3(0, 0, 0);
            player.GetComponent<PlantBomb>().saftyFirst = false;
        }
        if (spawnBoxChance < 8) spawnBoxChance += 0.02f;
        MapGenerate();
        stage++;
        pointsText.GetComponent<TextMeshProUGUI>().text = "0";
        stageText.GetComponent<TextMeshProUGUI>().text = stage.ToString();
        StartCoroutine(StageStart());
    }

    //Dodaje punkty
    public void AddPoints(int i)
    {
        points += i;
        pointsText.GetComponent<TextMeshProUGUI>().text = points.ToString();
    }

    //Sumuje punkty pod koniec etapu
    public void ScoreSummary()
    {
        score = score + points * (int)timer * stage;
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
        points = 0;
    }

    private void EnemysGenerate()
    {
        Vector3Int cell = new Vector3Int();
        cell.z = 0;
        bool plc;
        for (int count=1;count <= (1+ stage/2);count++)
        {
            if (count >= 6) break;
            plc = false;
            while (!plc)
            {
                cell.x = (int)Mathf.Round( (0 - mapWidth) + Random.Range(0, mapWidth+mapWidth));
                cell.y = (int)Mathf.Round((0 - mapHight) + Random.Range(0, mapHight + mapHight));

                if (!(cell.y >= -2 && cell.y <= 2 && cell.x >= -2 && cell.x <= 2))
                {
                    TileBase tile = arenaTiles.GetTile(cell);
                    if (tile == null)
                    {
                        destructibleTiles.SetTile(cell, null);
                        cell.y += 1;
                        GameObject enemy = Instantiate(enemyPrefeb, cell, Quaternion.identity);
                        mobs.Add(enemy);
                        plc = true;
                    }
                }
            }
        }
    }

    public void RemoveEnemy(GameObject en)
    {
        mobs.Remove(en);
    }

    public bool CheckPosition(Vector3Int pos)
    {
        TileBase tile = arenaTiles.GetTile(pos);
        TileBase tile2 = destructibleTiles.GetTile(pos);
        if(tile == null && tile2 == null)
        {
            return true;
        }
        return false;
    }

    private void DestroyAllBombs()
    {
        GameObject[] bomby = GameObject.FindGameObjectsWithTag("Bomb");

        for (var i = 0; i < bomby.Length; i++)
        {
            bomby[i].transform.position = new Vector3Int(20, 20, 0);
        }
    }
}
