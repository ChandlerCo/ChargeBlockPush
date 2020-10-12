using System.Collections;
using System.Collections.Generic;

public interface PlayerMove
{
    void Set(UserInput input, float timeDelta);
    void Attack(UserInput input, float timeDelta);
    Move getMove();
    Effect getEffect();
}
