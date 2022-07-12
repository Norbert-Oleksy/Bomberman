using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public float speed = 5f;

    public KeyCode playerMoveUp = KeyCode.W;
    public KeyCode playerMoveDown = KeyCode.S;
    public KeyCode playerMoveLeft = KeyCode.A;
    public KeyCode playerMoveRight = KeyCode.D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

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
    
}

/* Pomys³y
    
    // Przenosi postaæ co kratkê
     private float nextActionTime = 0.0f;
     public float period = 1f;
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            canMove = true;
        }

        Vector2 position = rigidbody2d.position;

        if (Input.GetKey(playerMoveUp) && canMove)
        {
            rigidbody2d.MovePosition(position + Vector2.up);
            canMove = false;
        }
        else if (Input.GetKey(playerMoveDown) && canMove)
        {
            rigidbody2d.MovePosition(position + Vector2.down);
            canMove = false;
        }
        else if (Input.GetKey(playerMoveLeft) && canMove)
        {
            rigidbody2d.MovePosition(position + Vector2.left);
            canMove = false;
        }
        else if (Input.GetKey(playerMoveRight) && canMove)
        {
            rigidbody2d.MovePosition(position + Vector2.right);
            canMove = false;
        }

    }

*/