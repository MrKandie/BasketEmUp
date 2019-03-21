using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton du gameManager
    [HideInInspector] public static GameManager i;

    [Header("Game settings")]
    public int NoSettingsYet;


    [Header("Game informations")]
    public List<PlayerController> playerList = new List<PlayerController>();

    private void Awake()
    {
        i = this;
        ControlPlayer(0);
    }

    private void Update()
    {
        CheckInputs();   
    }

    public void ControlPlayer(int playerID)
    {
        foreach (PlayerController player in playerList)
        {
            player.DisableInput();
        }
        playerList[playerID].EnableInput();
    }

    public void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ControlPlayer(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ControlPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ControlPlayer(2);
        }
    }
}
