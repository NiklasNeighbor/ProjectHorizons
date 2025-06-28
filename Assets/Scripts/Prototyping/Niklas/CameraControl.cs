using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject CameraTarget;
    public float VerticalMoveSpeed = 1f;
    public Vector2 IdleFraming;
    public float LowerLimit = -4f;
    public LayerMask Ground;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CameraTarget != null)
        {
            Vector3 newFraming = CameraTarget.transform.position + new Vector3(IdleFraming.x, IdleFraming.y, -10);
            newFraming.y = transform.position.y;
            transform.position = newFraming;
            AdjustHeight();
        }
    }

    void AdjustHeight()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, CameraTarget.transform.position.y + IdleFraming.y, transform.position.z);

        while (CameraTarget.transform.position.y < transform.position.y + LowerLimit)
        {
            transform.Translate(Vector3.down * 0.1f);
            //Debug.Log("Failsafe Scroll Down");
        }

        if (Vector3.Distance(currentPos, targetPos) > 0.5f)
        {
            //Debug.Log("Must Scroll!");
            if (Physics2D.Raycast(CameraTarget.transform.position, Vector2.down, 0.75f, Ground))
            {
                transform.position = Vector3.MoveTowards(currentPos, targetPos, VerticalMoveSpeed * Time.deltaTime);
                //Debug.Log("Grounded! Scrolling.");
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, 0) - new Vector3(IdleFraming.x, IdleFraming.y, 0), 0.5f);
        Gizmos.color = Color.yellow;
        if (CameraTarget != null)
            Gizmos.DrawSphere(CameraTarget.transform.position, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x - 10, transform.position.y + LowerLimit, 0), new Vector3(transform.position.x + 10, transform.position.y + LowerLimit, 0));
    }
}
