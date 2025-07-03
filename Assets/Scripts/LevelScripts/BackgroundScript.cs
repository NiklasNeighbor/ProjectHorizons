using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField] GameObject BackgroundParent;
    [SerializeField] float SpawnDistance;
    [SerializeField] float RemoveDistance;

    [System.Serializable]
    struct Layer
    {
        public GameObject layerParent;
        public GameObject layerPrefab;
        public float layerSpeed;
    }

    [SerializeField] Layer[] backGroundLayers;


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

        foreach (Layer l in backGroundLayers)
        {
            if(l.layerParent == null || l.layerPrefab == null)
            {
                Debug.Log("There are missing background elements!");
            }else
            {
                Transform lastLayerSegTf = l.layerParent.transform.GetChild(l.layerParent.transform.childCount - 1);
                if (lastLayerSegTf.position.x < SpawnDistance)
                {
                    Vector3 spawnPos = lastLayerSegTf.GetChild(0).GetChild(0).transform.position;
                    SpawnLevel(spawnPos, l);
                }


                Transform firstLayerSegTf = l.layerParent.transform.GetChild(0);
                if (firstLayerSegTf.position.x < RemoveDistance)
                {
                    Destroy(firstLayerSegTf.gameObject);
                }
            }
        }
    }

    void SpawnLevel(Vector3 pos, Layer layer)
    {
        GameObject newSegment = Instantiate(layer.layerPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        newSegment.transform.parent = layer.layerParent.transform;
    }

    public void ScrollAdvance(float moveAmount)
    {
        foreach(Layer l in backGroundLayers)
        {
            for(int i = 0; i < l.layerParent.transform.childCount; i++)
            {
                l.layerParent.transform.GetChild(i).Translate(new Vector3(-moveAmount * l.layerSpeed, 0, 0));
            }
        }
    }
}
