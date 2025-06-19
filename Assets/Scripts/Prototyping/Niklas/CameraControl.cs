using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject CameraTarget;
    public float VerticalMoveSpeed = 1f;
    public Vector2 IdleFraming;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newFraming = CameraTarget.transform.position + new Vector3(IdleFraming.x, IdleFraming.y, -10);
        newFraming.y = transform.position.y;
        transform.position = newFraming;
        AdjustHeight();
    }

    void AdjustHeight()
    {
        if (Mathf.Abs((CameraTarget.transform.position.y + IdleFraming.y) - transform.position.y) > 1)
        {
            Debug.Log("Adjusting");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, 0) - new Vector3(IdleFraming.x, IdleFraming.y, 0), 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(CameraTarget.transform.position, 0.5f);
    }
}
