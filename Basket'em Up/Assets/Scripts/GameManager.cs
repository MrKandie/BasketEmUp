using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton du gameManager
    [HideInInspector] public static GameManager i;
    [HideInInspector] public Library library;
    [HideInInspector] public MomentumManager momentumManager;
    [HideInInspector] public LevelManager levelManager;
    [HideInInspector] public BallManager ballManager;
    [HideInInspector] public EnemySpawner enemySpawner;
    [HideInInspector] public SoundManager soundManager;

    [Header("Game settings")]
    public int NoSettingsYet;


    private void Awake()
    {
        i = this;
        library = FindObjectOfType<Library>();
        momentumManager = FindObjectOfType<MomentumManager>();
        levelManager = FindObjectOfType<LevelManager>();
        ballManager = FindObjectOfType<BallManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        soundManager = FindObjectOfType<SoundManager>();
        ControlAllPlayers();
    }

    public void ControlPlayer(int playerID)
    {
        foreach (PlayerController player in levelManager.players)
        {
            player.DisableInput();
        }
        levelManager.players[playerID].EnableInput();
    }

    public void ControlAllPlayers()
    {
        foreach (PlayerController player in levelManager.players)
        {
            player.EnableInput();
        }
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
}
