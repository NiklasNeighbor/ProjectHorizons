using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] int difficulty;

    [Range(0, 1)]
    public float mediumDifficulty;
    [Range(0, 1)]
    public float hardDifficulty;

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
            mediumDifficulty = (difficulty - mediumRange.x) / (mediumRange.y - mediumRange.x);
        }

        if (difficulty > hardRange.x && difficulty < hardRange.y)
        {
            hardDifficulty = (difficulty - hardRange.x) / (hardRange.y - hardRange.x);
        }
    }
}
