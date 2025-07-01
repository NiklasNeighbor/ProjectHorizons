using UnityEngine;

public class WalkingEnemyAI : MonoBehaviour
{
    public bool canShoot;
    [SerializeField] Rigidbody2D rb;
    public float moveSpeed;
    public Transform player;

    public GameObject objectToSpawn; // the prefab effect
    [SerializeField] int pointsOnDeath;
    [SerializeField] Animator anim;
    [SerializeField] float spotDistance;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (DetectedPlayer())
            {
                if (anim.GetBool("PlayerDetected") == false)
                    anim.SetBool("PlayerDetected", true);
                rb.linearVelocityX = moveSpeed;
                Debug.Log("MOVE");
                //moveSpeed = moveSpeed * 2;
            }
            else
            {
                rb.linearVelocityX = 0;
            }
        }
        // rb.linearVelocityX = moveSpeed;
    }
    bool DetectedPlayer()
    {
        if (Vector2.Distance(rb.position, player.position) <= spotDistance)
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("boop");
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            Destroy(collision.gameObject);
            Death();
        }
    }

    void Death()
    {
        GameObject spawned = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        spawned.SetActive(true);//spawns in death particle effect prefab
        GameObject.FindWithTag("GameController").GetComponent<ScoreManager>().IncreaseScore(pointsOnDeath);
        Destroy(this.gameObject);
    }
}
