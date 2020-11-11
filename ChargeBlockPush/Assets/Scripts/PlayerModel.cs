using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static System.Math;

public class PlayerModel
{
    //private Rigidbody2D rb;
    private CapsuleCollider2D collider;

    private double posX;
    private double posY;
    public double velX;
    public double velY;
    public double accX;
    public double accY;

    public PState state;

    public double angle; // from pi to -pi (rad)

    public float moveSpd;

    private PlayerMove pMove;

    private double minStunSpd;

    public PlayerModel(double x, double y, CapsuleCollider2D cldr)//, Rigidbody2D rbody)
    {
        collider = cldr;
        collider.direction = CapsuleDirection2D.Horizontal;

        changeState(PState.Free);

        posX = x;
        posY = y;

        angle = -PI/2;

        moveSpd = 1.5f;
    }

    // accessors

    public double getPosX() { return posX; }

    public double getPosY() { return posY; }

    public double getAngleRad() { return angle; }

    public double getAngleDeg() { return angle * 180/PI; }

    public Move getMove()
    {
        if (state == PState.Attacking || state == PState.Setting)
            return pMove.getMove();
        return Move.None;
    }

    public Effect getEffect()
    {
        if (state == PState.Attacking)
            return pMove.getEffect();
        Effect e;
        e.vel = 0;
        e.dec = 0;
        e.min = 0;
        return e;
    }

    // modifiers

    private void kinematics(double s)
    {
        velX += accX * s;
        velY += accY * s;
    }

    public void changeState(PState newState)
    {
        velX = 0;
        velY = 0;
        accX = 0;
        accY = 0;

        state = newState;

        if (state == PState.Attacking)
        {
            switch(getMove())
            {
                case Move.Charge:
                    collider.offset = new Vector2(0f, -0.06f);
                    collider.size = new Vector2(0.9f, 0.9f);
                    break;
                case Move.Block:
                    collider.offset = new Vector2(0f, -0.06f);
                    collider.size = new Vector2(1.1f, 0.8f);
                    break;
                case Move.Push:
                    collider.offset = new Vector2(0f, -0.2f);
                    collider.size = new Vector2(2.5f, 0.95f);
                    break;
            }
        }
        else
        {
            collider.offset = new Vector2(0f, -0.06f);
            collider.size = new Vector2(0.6f, 0.6f);
        }
    }

    private void playerFree(UserInput input, double timeDelta)
    {
        if (input.valY != 0 || input.valX != 0)
        {
            angle = Atan2(input.valY, input.valX);
            velX = Cos(angle) * moveSpd;
            velY = Sin(angle) * moveSpd;
        }
        else
        {
            velX = 0;
            velY = 0;
        }

        if (input.valC > 0)
        {
            changeState(PState.Setting);
            pMove = new Charge(this);
        }
        else if (input.valB > 0)
        {
            changeState(PState.Setting);
            pMove = new Block(this);
        }
        else if (input.valP > 0)
        {
            changeState(PState.Setting);
            pMove = new Push(this);
        }
    }

    private void playerStunned()
    {
        double speed = Sqrt(velX * velX + velY * velY);
        if (speed < minStunSpd)
        {
            changeState(PState.Free);
        }
    }

    private void playerSet(UserInput input, float timeDelta)
    {
        pMove.Set(input, timeDelta);
    }

    private void playerAttack(UserInput input, float timeDelta)
    {
        pMove.Attack(input, timeDelta);
    }

    private void playerDead()
    {
        if (Sqrt(velX * velX + velY * velY) < 0.25)
        {
            velX = 0;
            velY = 0;
            accX = 0;
            accY = 0;
        }
    }

    public void Update(UserInput input, float timeDelta, double x, double y)
    {
        posX = x;
        posY = y;
        switch(state)
        {
            case PState.Free:
                playerFree(input, timeDelta);
                break;
            case PState.Stunned:
                playerStunned();
                break;
            case PState.Setting:
                playerSet(input, timeDelta);
                break;
            case PState.Attacking:
                playerAttack(input, timeDelta);
                break;
            case PState.Dead:
                playerDead();
                break;
        }

        kinematics(timeDelta);
    }

    private void collision(PlayerModel p2)
    {
        changeState(PState.Stunned);

        Effect e = p2.getEffect();

        double speed = Sqrt(velX * velX + velY * velY) + e.vel;

        double diffX = posX - p2.posX;
        double diffY = posY - p2.posY;
        double dist = Sqrt(diffX * diffX + diffY * diffY);

        angle = Atan2(diffY, diffX);

        velX = diffX / dist * speed;
        velY = diffY / dist * speed;
        
        accX = diffX / dist * e.dec;
        accY = diffY / dist * e.dec;

        minStunSpd = e.min;
    }

    public void Collide(PlayerModel p2)
    {
        if ((state == PState.Free || state == PState.Setting) && p2.state == PState.Attacking)
        {
            collision(p2);
        }
        else if (state == PState.Attacking && p2.state == PState.Attacking)
        {
            Move m1 = getMove();
            Move m2 = p2.getMove();

            if (m1 == Move.Charge && m2 == Move.Block)
            {
                collision(p2);
            }
            else if (m1 == Move.Block && m2 == Move.Push)
            {
                collision(p2);
            }
            else if (m1 == Move.Push && m2 == Move.Charge)
            {
                collision(p2);
            }
        }
    }

    public void playerKilled()
    {
        collider.enabled = false;
        
        double vX = velX;
        double vY = velY;

        Debug.Log(vX + " | " + vY);
        
        changeState(PState.Dead);
        
        velX = vX;
        velY = vY;

        double spd = Sqrt(velX * velX + velY * velY);
        
        accX = -velX / 0.5;
        accY = -velY / 0.5;
    }
}
