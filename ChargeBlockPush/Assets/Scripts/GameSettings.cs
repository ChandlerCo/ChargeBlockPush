using System.Collections;
using System.Collections.Generic;

public static class GameSettings
{
    private static string map;
    
    public static string Map
    {
        get
        {
            return map;
        }
        set
        {
            map = value;
        }
    }
}
