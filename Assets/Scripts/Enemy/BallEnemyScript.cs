using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEnemyScript : MonoBehaviour
{
    public int points = 10;
    public int lives = 1;
    public float speed = 0.2f;

    private GameManager GameManager;

    [Header("Movement")]
    private Vector3Int lastPosition;
    private Vector3Int destination;
    public List<Vector3Int> moveMap = new List<Vector3Int>();
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        int stage = GameManager.stage;
        lives += stage / 10;
        speed+= (speed /10)*speed;
        points += (stage - 1) * 10;
        lastPosition.x = (int)gameObject.transform.position.x;
        lastPosition.y = (int)gameObject.transform.position.y;
        lastPosition.z=0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            moving = true;
            MapingMap();
        }else{
            if (transform.position == destination)
            {
                lastPosition = destination;
                moving = false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            lives--;
            if (lives <= 0)
            {
                GameManager.AddPoints(points);
                GameManager.RemoveEnemy(gameObject);
                GameManager.CheckWinState();
                Destroy(gameObject);
            }
        }
    }

    private void MapingMap()
    {
        Vector3 position = gameObject.transform.position;
        Vector3Int positionInt = new()
        {
            x = (int)Mathf.Round(position.x)-1,
            y = (int)Mathf.Round(position.y)-1,
            z = 0
        };
        if (GameManager.CheckPosition(positionInt))
        {
            moveMap.Add(positionInt);
        }
        positionInt.x += 2;
        if (GameManager.CheckPosition(positionInt))
        {
            moveMap.Add(positionInt);
        }
        positionInt.x -= 1;
        positionInt.y -= 1;
        if (GameManager.CheckPosition(positionInt))
        {
            moveMap.Add(positionInt);
        }
        positionInt.y += 2;
        if (GameManager.CheckPosition(positionInt))
        {
            moveMap.Add(positionInt);
        }
        if (moveMap.Count > 0) { 
            Move();
        }
        else
        {
            moving = false;
        }

    }

    private void Move()
    {
        int ind = Random.Range(0, moveMap.Count);
        Vector3Int pos = moveMap[ind];
        if (pos == lastPosition)
        {
            if (Random.value < 0.5f)
            {
                ind = Random.Range(0, moveMap.Count);
                pos = moveMap[ind];
            }
        }
        destination = pos;
        destination.y += 1;
        moveMap.Clear();
    }

}
