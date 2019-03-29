using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton du gameManager
    [HideInInspector] public static GameManager i;
    [HideInInspector] public Library library;
    [HideInInspector] public MomentumManager momentumManager;
    [HideInInspector] public LevelManager levelManager;
    [HideInInspector] public BallMovementManager ballMovementManager;

    [Header("Game settings")]
    public int NoSettingsYet;


    private void Awake()
    {
        i = this;
        library = FindObjectOfType<Library>();
        momentumManager = FindObjectOfType<MomentumManager>();
        levelManager = FindObjectOfType<LevelManager>();
        ballMovementManager = FindObjectOfType<BallMovementManager>();
        ControlPlayer(0);
    }

    private void Update()
    {
        CheckInputs();   
    }

    public void ControlPlayer(int playerID)
    {
        foreach (PlayerController player in levelManager.players)
        {
            player.DisableInput();
        }
        levelManager.players[playerID].EnableInput();
    }

    public Vector3 GetGroundPosition(Vector3 position)
    {
        RaycastHit hit;
        Vector3 groundPosition = position;
        Vector3 groundPositionNoY = groundPosition + new Vector3(0, 1000, 0);
        if (Physics.Raycast(groundPositionNoY,
            Vector3.down,
            out hit))
        {
            return hit.point;
        } else
        {
            return position;
        }
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
