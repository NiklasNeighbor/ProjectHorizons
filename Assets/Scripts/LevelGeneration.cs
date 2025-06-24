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
        ScrollLevel();
        CheckLevelDistance();
    }

    void ScrollLevel()
    {
        for(int i = 0; i < _LevelParent.transform.childCount; i++)
        {
            _LevelParent.transform.GetChild(i).position -= new Vector3(_Speed * 0.01f, 0, 0);
        }
    }

    void CheckLevelDistance()
    {
        Transform lastChild = _LevelParent.transform.GetChild(_LevelParent.transform.childCount - 1);
        Vector3 spawnPos = lastChild.GetChild(0).transform.position;
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
}
