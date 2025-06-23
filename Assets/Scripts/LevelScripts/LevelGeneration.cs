//Scrolls level
//

using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public float _Speed;
    [SerializeField] GameObject _LevelParent;
    [SerializeField] float _SpawnDistance;
    [SerializeField] float _RemoveDistance;
    [SerializeField] GameObject[] _LevelSegments;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if (spawnPos.x < _SpawnDistance)
        {
            SpawnLevel(spawnPos);
        }

        Transform firstChild = _LevelParent.transform.GetChild(0);

        if (firstChild.position.x < _RemoveDistance)
        {
            Destroy(firstChild.gameObject);
        }
    }

    void SpawnLevel(Vector3 pos)
    {
        int random = Random.Range(0, _LevelSegments.Length);
        GameObject newSegment = Instantiate(_LevelSegments[random], new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        newSegment.transform.parent = _LevelParent.transform;
    }

    public void ScrollAdvance(float moveAmount)
    {
        for (int i = 0; i < _LevelParent.transform.childCount; i++)
        {
            _LevelParent.transform.GetChild(i).position -= new Vector3(moveAmount, 0, 0);
        }
    }
}
