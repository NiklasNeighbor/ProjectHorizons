using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public Transform NextSegmentSpawnPos;
    DifficultyManager difficultyManager;
    enum Difficulty { Easy, Medium, Hard };

    [System.Serializable]
    struct EnemySpawn
    {
        public GameObject enemyPrefab;
        public Transform spawnPosition;
        public Difficulty difficulty;
    }

    [SerializeField] EnemySpawn[] enemySpawns;

    void Start()
    {
        difficultyManager = GameObject.FindWithTag("GameController").GetComponent<DifficultyManager>();
        SpawnEnemies();
    }

    void SpawnEnemies()
    {

        foreach (EnemySpawn spawn in enemySpawns)
        {
            float random = Random.value;
            bool doSpawn = false;

            switch (spawn.difficulty)
            {
                case Difficulty.Easy:
                    doSpawn = true;
                    break;

                case Difficulty.Medium:
                    if (random <= difficultyManager.GetMediumDifficulty()) //?? getting the difficultyManager in start gives errors
                    {
                        doSpawn = true;
                    }
                    break;

                case Difficulty.Hard:
                    if (random <= difficultyManager.GetHardDifficulty())
                    {
                        doSpawn = true;
                    }
                    break;
            }

            if(doSpawn)
            {
                GameObject enemy = Instantiate(spawn.enemyPrefab, spawn.spawnPosition.position, Quaternion.identity);
                enemy.transform.parent = transform;
            }
        }
    }

}


