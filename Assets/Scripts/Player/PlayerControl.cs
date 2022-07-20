using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject gameLogic;
    Rigidbody2D rigidbody2d;
    private float speed;
    public GameObject deadbody;

    public KeyCode playerMoveUp = KeyCode.W;
    public KeyCode playerMoveDown = KeyCode.S;
    public KeyCode playerMoveLeft = KeyCode.A;
    public KeyCode playerMoveRight = KeyCode.D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        //GameLogic
        speed = gameLogic.GetComponent<GameManager>().speedPlayers;
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        Vector2 translation = new Vector2();

        if (Input.GetKey(playerMoveUp))
        {
            translation = Vector2.up * speed * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(playerMoveDown))
        {
            translation = Vector2.down * speed * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(playerMoveLeft))
        {
            translation = Vector2.left * speed * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(playerMoveRight))
        {
            translation = Vector2.right * speed * Time.fixedDeltaTime;
        }

        rigidbody2d.MovePosition(position + translation);
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.layer == LayerMask.NameToLayer("Explosion") || obj.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        Vector2 position = transform.position;
        enabled = false;
        gameObject.SetActive(false);
        GetComponent<PlantBomb>().enabled = false;
        GameObject deadplayer = Instantiate(deadbody, position, Quaternion.identity);
        Invoke(nameof(OnDeathSequenceEnded), 0.2f);
    }

    private void OnDeathSequenceEnded()
    {
        FindObjectOfType<GameManager>().CheckWinState();
    }

    public void SetPlayerSpeed(int val)
    {
        speed=speed+(speed/10)*val;
    }

}