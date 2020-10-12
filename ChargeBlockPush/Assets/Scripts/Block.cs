using static System.Math;
using System.Collections;
using System.Collections.Generic;

public class Block : PlayerMove
{
    private PlayerModel p;
    private double count;

    public Block(PlayerModel player)
    {
        p = player;
        count = 0.3;
    }

    public void Set(UserInput input, float timeDelta)
    {
        count-= timeDelta;
        if (input.valB == 0)
        {
            p.changeState(PState.Free);
        }
        else if (count < 0)
        {
            p.changeState(PState.Attacking);
            count = 0.5;
        }
    }
    
    public void Attack(UserInput input, float timeDelta)
    {
        count-=timeDelta;
        if (count <= 0 && input.valB == 0)
        {
            p.changeState(PState.Free);
        }
    }

    public Move getMove()
    {
        return Move.Block;
    }

    public Effect getEffect()
    {
        Effect e;

        e.vel = 15;
        e.dec = -40;
        e.min = 1;

        return e;
    }
}