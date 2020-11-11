using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private SpriteRenderer sprite;

    private PlayerModel playerData;

    private Rigidbody2D rb;

    public int playerType;
    private Func<UserInput>[] playerInput;

    private UserInput P1Input()
    {
        UserInput input;

        input.valY = Input.GetAxis("P1Vertical");
        input.valX = Input.GetAxis("P1Horizontal");

        input.valC = Input.GetAxis("P1Fire1");
        input.valB = Input.GetAxis("P1Fire2");
        input.valP = Input.GetAxis("P1Fire3");

        return input;
    }

    private UserInput P2Input()
    {
        UserInput input;

        input.valY = Input.GetAxis("P2Vertical");
        input.valX = Input.GetAxis("P2Horizontal");

        input.valC = Input.GetAxis("P2Fire1");
        input.valB = Input.GetAxis("P2Fire2");
        input.valP = Input.GetAxis("P2Fire3");

        return input;
    }

    // Start is called before the first frame update
    void Start()
    {
        int x = 0;
        int y = 0;
        playerData = new PlayerModel(x, y, GetComponent<CapsuleCollider2D>());

        
        rb = GetComponent<Rigidbody2D>();
        rb.transform.position = new Vector2(x, y);

        sprite = GetComponent<SpriteRenderer>();

        playerInput = new Func<UserInput>[2];
        playerInput[0] = P1Input;
        playerInput[1] = P2Input;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UserInput input = playerInput[playerType]();

        playerData.Update(input, Time.deltaTime, transform.position.x, transform.position.y);

        rb.velocity = new Vector2((float)playerData.velX, (float)playerData.velY);

        // set animation parameters
        animator.SetInteger("State", (int)playerData.state);

        int angle = (int)Mathf.Round((float)playerData.getAngleDeg());
        animator.SetInteger("Direction", angle);
        if (Mathf.Abs(angle) > 90)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }

        
        if (playerData.state == PState.Free)
        {
            float speed = Mathf.Sqrt((float)(playerData.velX * playerData.velX + playerData.velY * playerData.velY));
            if (speed > 0.01)
            {
                animator.SetInteger("SubState", 1);
            }
            else
            {
                animator.SetInteger("SubState", 0);
            }
        }
        
        if (playerData.state == PState.Setting || playerData.state == PState.Attacking)
        {
            animator.SetInteger("SubState", (int)playerData.getMove());
        }
    }

    public PlayerModel getPlayerData()
    {
        return playerData;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerModel p2 = collision.gameObject.GetComponent<PlayerController>().getPlayerData();
            playerData.Collide(p2);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Boundary")
        {
            playerData.playerKilled();
        }
    }
}
