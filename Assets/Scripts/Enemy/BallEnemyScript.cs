using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEnemyScript : MonoBehaviour
{
    public int points = 10;
    public int lives = 1;
    public float speed = 1;

    [Header("Movement")]
    private Vector3 lastPosition;
    private List<Vector3Int> moveMap;
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        lives += FindObjectOfType<GameManager>().stage / 10;
        speed+= (speed + FindObjectOfType<GameManager>().stage) / 10;
        lastPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!moving)
        {
            if(moveMap.Co)MapingMap();
        }*/
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            lives--;
            if (lives <= 0)
            {
                FindObjectOfType<GameManager>().AddPoints(points);
                FindObjectOfType<GameManager>().RemoveEnemy(gameObject);
                FindObjectOfType<GameManager>().CheckWinState();
                Destroy(gameObject);
            }
        }
    }

    private void MapingMap()
    {
        Vector3 position = gameObject.transform.position;
        Vector3Int positionInt = new Vector3Int
        {
            x = (int)Mathf.Round(position.x)-1,
            y = (int)Mathf.Round(position.y),
            z = 0
        };
        if (FindObjectOfType<GameManager>().CheckPosition(positionInt)) moveMap.Add(positionInt);

        positionInt.x += 2;
        if (FindObjectOfType<GameManager>().CheckPosition(positionInt)) moveMap.Add(positionInt);

        positionInt.x -= 1;
        positionInt.y -= 1;
        if (FindObjectOfType<GameManager>().CheckPosition(positionInt)) moveMap.Add(positionInt);

        positionInt.y += 2;
        if (FindObjectOfType<GameManager>().CheckPosition(positionInt)) moveMap.Add(positionInt);
    }

    private void Move()
    {
        
    }
}
