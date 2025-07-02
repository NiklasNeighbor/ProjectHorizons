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
    [Range(0, 1)]
    public float addedSpeed;

    [SerializeField] Vector2Int mediumRange;
    [SerializeField] Vector2Int hardRange;
    [SerializeField] Vector2Int speedUpRange;

    // Update is called once per frame
    void Update()
    {
        IncreaseSpeed();
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

    void IncreaseSpeed()
    {
        if (difficulty > speedUpRange.x && difficulty < speedUpRange.y)
        {
            addedSpeed = (float)(difficulty - speedUpRange.x) / (float)(speedUpRange.y - speedUpRange.x);
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
