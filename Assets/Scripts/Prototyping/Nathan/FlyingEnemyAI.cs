using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FlyingEnemyAI : MonoBehaviour
{
    public bool hasProjectile;
    [SerializeField] Rigidbody2D rb;
    public float moveSpeed;
    public Transform player;
    public bool chasingPlayer;
    public GameObject objectToSpawn; // the prefab to spawn

    Vector2 targetPosition;

    private void Start()
    {
        chasingPlayer = false;
        player = GameObject.FindWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (chasingPlayer && Vector2.Distance(rb.position, player.position) > 2.5f)
            {
                Vector2 newTargetPosition = Vector2.MoveTowards(rb.position, new Vector2(player.position.x + 2, targetPosition.y), moveSpeed * 3f * Time.deltaTime);
                rb.MovePosition(newTargetPosition);
            }
            else
            {
                if (chasingPlayer)
                    chasingPlayer = false;
                rb.linearVelocity = Vector2.left * moveSpeed;
            }
            if (PlayerDetected())
            {
                targetPosition = new Vector2(player.position.x + 2, player.position.y);
                chasingPlayer = true;
            }
        }
    }
    bool PlayerDetected()
    {
        if (Vector2.Distance(rb.position, player.position) <= 10 && rb.position.x > player.position.x)
            return true;
        else
            return false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("boop");
        }
        if (collision.gameObject.layer == 7)
        {
            GameObject spawned = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            spawned.SetActive(true);//spawns in death particle effect prefab

            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
