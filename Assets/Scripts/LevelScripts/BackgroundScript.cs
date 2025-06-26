using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField] GameObject BackgroundParent;
    [SerializeField] float SpawnDistance;
    [SerializeField] float RemoveDistance;
    [SerializeField] GameObject BackGroundPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckLevelDistance();
    }

    void CheckLevelDistance()
    {
        Transform lastBg = BackgroundParent.transform.GetChild(BackgroundParent.transform.childCount - 1);
        Vector3 spawnPos = lastBg.GetChild(0).GetChild(0).transform.position;
        if (spawnPos.x < SpawnDistance)
        {
            SpawnLevel(spawnPos);
        }

        Transform firstChild = BackgroundParent.transform.GetChild(0);

        if (firstChild.position.x < RemoveDistance)
        {
            Destroy(firstChild.gameObject);
        }
    }


    void SpawnLevel(Vector3 pos)
    {
        GameObject newSegment = Instantiate(BackGroundPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        newSegment.transform.parent = BackgroundParent.transform;
    }

    public void ScrollAdvance(float moveAmount)
    {
        for (int i = 0; i < BackgroundParent.transform.childCount; i++)
        {
            BackgroundParent.transform.GetChild(i).position -= new Vector3(moveAmount, 0, 0);
        }
    }
}
