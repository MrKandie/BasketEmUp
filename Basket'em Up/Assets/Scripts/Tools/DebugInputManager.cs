using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugInputManager : MonoBehaviour
{
    private void Update()
    {
        CheckInputs();
    }

    public void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(1);
        }
        GameManager g = FindObjectOfType<GameManager>();
        if (g!= null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                g.enemySpawner.SpawnEnemy(g.enemySpawner.rookiePrefab, 1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                g.enemySpawner.SpawnEnemy(g.enemySpawner.trainerPrefab, 1);
            }
        }
    }
}
