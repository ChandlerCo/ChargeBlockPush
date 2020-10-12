using static System.Math;
using System.Collections;
using System.Collections.Generic;

public class Push : PlayerMove
{
    private PlayerModel p;
    private double count;

    public Push(PlayerModel player)
    {
        p = player;
        count = 0.3;
    }

    public void Set(UserInput input, float timeDelta)
    {
        count-=timeDelta;
        if (input.valP == 0)
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
        if (count <= 0)
        {
            p.changeState(PState.Free);
        }
    }

    public Move getMove()
    {
        return Move.Push;
    }

    public Effect getEffect()
    {
        Effect e;

        e.vel = 10;
        e.dec = -25;
        e.min = 1.5;

        return e;
    }
}