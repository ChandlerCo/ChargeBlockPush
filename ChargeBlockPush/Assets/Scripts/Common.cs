using System.Collections;
using System.Collections.Generic;

public enum PState
{
    Free = 0,
    Setting = 1,
    Attacking = 2,
    Stunned = 3,
    Dead = 4
}

public enum Move
{
    None = -1,
    Charge = 0,
    Block = 1,
    Push = 2
}

public struct UserInput
{
    public float valX;
    public float valY;
    public float valC;
    public float valB;
    public float valP;
}

public struct Effect
{
    public double vel;
    public double dec;
    public double min;
}