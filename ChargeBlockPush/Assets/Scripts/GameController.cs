using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject pfMap;
    public GameObject pfMap2;
    // Start is called before the first frame update
    void Start()
    {
        switch(GameSettings.Map)
        {
            case "Map2":
                Instantiate(pfMap2);
                break;
            default:
                Instantiate(pfMap);
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
