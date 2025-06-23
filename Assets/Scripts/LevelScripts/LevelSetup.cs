using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public Transform NextSegmentSpawnPos;

    [System.Serializable]
    struct EnemySpawn
    {
        public GameObject enemyPrefab;
        public Transform[] spawnPositions;
        [Range(0.0f, 1f)]
        public float[] spawnChanceRange;
    }

    [SerializeField] EnemySpawn[] enemySpawns;


    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        foreach(EnemySpawn spawn in enemySpawns)
        {
            for(int i = 0; i < spawn.spawnChanceRange.Length; i++)
            {
                if(Random.value < spawn.spawnChanceRange[i])
                {
                    GameObject enemy = Instantiate(spawn.enemyPrefab, spawn.spawnPositions[i].position, Quaternion.identity);
                    enemy.transform.parent = transform;
                }
            }
        }
    }

}
