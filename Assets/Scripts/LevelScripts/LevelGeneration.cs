//Scrolls level
//

using UnityEngine;
using System.Collections.Generic;

public class LevelGeneration : MonoBehaviour
{
    public float _Speed;
    [SerializeField] GameObject _LevelParent;
    [SerializeField] float _SegmentSpawnDistance;
    [SerializeField] float _RemoveDistance;
    [SerializeField] GameObject[] _EasySegments;
    [SerializeField] GameObject[] _MediumSegments;
    [SerializeField] GameObject[] _HardSegments;
    DifficultyManager difficultyManager;
    [SerializeField] float _IntroSpawnDistance;


    List<float> levelHeights = new List<float>();
    float lowestLow = 100;
    [SerializeField] GameObject deathBox;
    public bool generateFlat;
    float spawnDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        difficultyManager = GetComponent<DifficultyManager>();
        float startLevelHeight = _LevelParent.transform.GetChild(0).transform.position.y;
        levelHeights.Add(startLevelHeight);
        lowestLow = startLevelHeight;
        spawnDistance = _IntroSpawnDistance;
    }

    // Update is called once per frame
    void Update()
    {
        CheckLevelDistance();
    }
    void CheckLevelDistance()
    {
        Transform lastLvl = _LevelParent.transform.GetChild(_LevelParent.transform.childCount - 1);
        Vector3 spawnPos = lastLvl.GetComponent<LevelSetup>().NextSegmentSpawnPos.position;
        if (spawnPos.x < spawnDistance)
        {
            SpawnLevel(spawnPos);
        }

        Transform firstChild = _LevelParent.transform.GetChild(0);

        if (firstChild.position.x < _RemoveDistance)
        {
            RemoveSegment(firstChild.gameObject);
        }
    }

    void SpawnLevel(Vector3 pos)
    {
        bool hasFoundLvl = false;
        GameObject newSegmentPrfb = _EasySegments[0];
        
        if(!generateFlat)
            while (!hasFoundLvl)
            {
                switch (Random.Range(0, 3))
                {
                    case 0: //easy
                        if (_EasySegments.Length > 0)
                        {
                            hasFoundLvl = true;
                            newSegmentPrfb = _EasySegments[Random.Range(0, _EasySegments.Length)];
                        }
                        else
                        {
                            Debug.Log("Missing easy level segments!");
                        }
                        break;

                    case 1: //normal
                        if (Random.value < difficultyManager.mediumDifficulty)
                        {
                            if (_MediumSegments.Length > 0)
                            {
                                hasFoundLvl = true;
                                newSegmentPrfb = _MediumSegments[Random.Range(0, _MediumSegments.Length)];
                            }
                            else
                            {
                                Debug.Log("Missing medium level segments!");
                            }
                        }
                        break;

                    case 2: //hard
                        if (Random.value < difficultyManager.hardDifficulty)
                        {
                            if (_HardSegments.Length > 0)
                            {
                                hasFoundLvl = true;
                                newSegmentPrfb = _HardSegments[Random.Range(0, _HardSegments.Length)];
                            }
                            else
                            {
                                Debug.Log("Missing hard level segments!");
                            }
                        }
                        break;
                }
            }


        GameObject newSegment = Instantiate(newSegmentPrfb, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        newSegment.transform.parent = _LevelParent.transform;

        float levelHeight = newSegment .transform.position.y;
        levelHeights.Add(levelHeight);
        if (levelHeight < lowestLow)
        {
            lowestLow = levelHeight;
            AdjustDeathBox(lowestLow);
        }
    }

    void RemoveSegment(GameObject segment)
    {
        //needs to check if the lowest platform gets removed but this if statement doesnt work.
        //if(segment.transform.position.y == lowestLow)
        //{
            GetLowestSegment();
        //}

        Destroy(segment);
        levelHeights.RemoveAt(0);

        AdjustDeathBox(lowestLow);
    }
    public void ScrollAdvance(float moveAmount)
    {
        for (int i = 0; i < _LevelParent.transform.childCount; i++)
        {
            _LevelParent.transform.GetChild(i).position -= new Vector3(moveAmount, 0, 0);
        }
    }

    void GetLowestSegment()
    {
        float currentLowest = 1000000;
        for(int i = 0; i < levelHeights.Count; i++)
        {
            if (levelHeights[i] < currentLowest)
            {
                currentLowest = levelHeights[i];
            }
        }
        lowestLow = currentLowest;
    }

    void AdjustDeathBox(float levelHeight)
    {
        deathBox.transform.position = new Vector3(deathBox.transform.position.x, levelHeight - 10, deathBox.transform.position.z);
    }

    public void SwitchSpawnDistance()
    {
        spawnDistance = _SegmentSpawnDistance;
    }


}
