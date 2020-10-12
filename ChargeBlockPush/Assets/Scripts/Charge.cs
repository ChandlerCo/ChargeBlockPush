using static System.Math;
using System.Collections;
using System.Collections.Generic;

public class Charge : PlayerMove
{
    private PlayerModel p;
    private double count;

    private const double spd1 = 20;
    private const double spd2 = 21.5;
    private const double acc1 = 12.5;
    private const double acc2 = -165;

    private const double delta = 0.01;

    public Charge(PlayerModel player)
    {
        p = player;
        count = 0.3;
    }

    public void Set(UserInput input, float timeDelta)
    {
        count-=timeDelta;
        if (input.valC == 0)
        {
            p.changeState(PState.Free);
        }
        else if (count < 0)
        {
            p.changeState(PState.Attacking);
            
            p.accX = Cos(p.angle) * acc1;
            p.accY = Sin(p.angle) * acc1;
            
            p.velX = Cos(p.angle) * spd1;
            p.velY = Sin(p.angle) * spd1;
        }
    }
    
    public void Attack(UserInput input, float timeDelta)
    {
        double acc = Sqrt(p.accX * p.accX + p.accY * p.accY);
        double speed = Sqrt(p.velX * p.velX + p.velY * p.velY);
        
        if (Abs(Abs(acc) - Abs(acc1)) < delta && speed < spd2)
        {
            p.accX = Cos(p.angle) * acc1;
            p.accY = Sin(p.angle) * acc1;
        }
        else
        {
            p.accX = Cos(p.angle) * acc2;
            p.accY = Sin(p.angle) * acc2;

            if (speed < 1.6)     // stop
            {
                p.changeState(PState.Free);
            }
        }
    }

    public Move getMove()
    {
        return Move.Charge;
    }

    public Effect getEffect()
    {
        Effect e;
        
        e.vel = 30;
        e.dec = -60;
        e.min = 5;

        return e;
    }
}

