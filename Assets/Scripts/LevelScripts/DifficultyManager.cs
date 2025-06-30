using System;
using UnityEngine;
using UnityEngine.Rendering;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] int difficulty;

    [Range(0, 1)]
    public float mediumDifficulty = 0;
    [Range(0, 1)]
    public float hardDifficulty = 0;

    [SerializeField] Vector2Int mediumRange;
    [SerializeField] Vector2Int hardRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseDifficulty(int addedDifficulty)
    {
        difficulty += addedDifficulty;

        if (difficulty > mediumRange.x && difficulty < mediumRange.y)
        {
            mediumDifficulty = (float)(difficulty - mediumRange.x) / (float)(mediumRange.y - mediumRange.x);
        }

        if (difficulty > hardRange.x && difficulty < hardRange.y)
        {
            hardDifficulty = (float)(difficulty - hardRange.x) / (float)(hardRange.y - hardRange.x);
        }
    }
    
    public float GetMediumDifficulty()
    {
        return mediumDifficulty;
    }

    public float GetHardDifficulty()
    {
        return hardDifficulty;
    }


}
