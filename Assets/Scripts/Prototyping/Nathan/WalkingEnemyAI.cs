using UnityEngine;

public class WalkingEnemyAI : MonoBehaviour
{
    public bool canShoot;
    [SerializeField] Rigidbody2D rb;
    public float moveSpeed;
    public Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectedPlayer())
        {
            rb.linearVelocityX = moveSpeed;
            Debug.Log("MOVE");
            //moveSpeed = moveSpeed * 2;
        }
        else
        {
            rb.linearVelocityX = 0;
        }
       // rb.linearVelocityX = moveSpeed;
    }
    bool DetectedPlayer()
    {
        if(Vector2.Distance(rb.position, player.position) <= 10)
            return true;
        else
            return false;
    }
   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            moveSpeed = moveSpeed * 2f;
        }
        if(collision.tag == "DeathZone")
        {
            Destroy(this.gameObject);
        }
    }*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("boop");
        }
        if(collision.gameObject.layer == 7)
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
