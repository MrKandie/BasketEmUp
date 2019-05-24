using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject rookiePrefab;
    public GameObject trainerPrefab;

    private List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        spawnPoints.Clear();
        foreach (Transform t in transform)
        {
            spawnPoints.Add(t);
        }
    }

    public void SpawnEnemy(GameObject enemyPrefab, int quantity)
    {
        if (spawnPoints.Count <= 0) { return; } 
        for (int i = 0; i < quantity; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemyPrefab.transform.position = spawnPoints[Mathf.RoundToInt(Random.Range(0, spawnPoints.Count))].position;
        }
    }
}
