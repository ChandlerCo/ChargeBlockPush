using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{
    void Start()
    {
        GameSettings.Map = "Map1";
    }

    public void StartGame()
    {
        Debug.Log("Start");
        SceneManager.LoadScene("Game");   // switch scene
    }

    public void ChangeMap(string newMap)
    {
        GameSettings.Map = newMap;
    }
}
