using System.Collections;
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
    public float diveMultiplier;
    Vector2 targetPosition;
    [SerializeField] Animator anim;
    private void Start()
    {
        chasingPlayer = false;
        player = GameObject.FindWithTag("Player").transform;
    }
    // Update is called once per frame
    private void Update()
    {

        if (player != null)
        {
            if (PlayerDetected())
            {
                targetPosition = new Vector2(player.position.x + 2, player.position.y);
                anim.SetBool("PlayerDetected", PlayerDetected());
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector2 newPosition = new Vector2(rb.position.x, player.position.y + 4);
                rb.position = newPosition;
            }
        }
    }
    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 playerDistance = rb.transform.TransformPoint(player.position);
            if (Vector2.Distance(rb.position, player.position) < 50 && Vector2.Distance(rb.position, player.position) > 20 && !chasingPlayer)
            {
                Vector2 newPosition = new Vector2(rb.position.x, player.position.y + 4);
                this.transform.position = newPosition;
            }
            if (chasingPlayer && rb.transform.position.y > player.position.y /*Vector2.Distance(rb.position, player.position) < 5f*/)
            {

                //Debug.Log("COOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM");
                Vector2 newTargetPosition = Vector2.MoveTowards(rb.position, new Vector2(player.position.x + 6, targetPosition.y), moveSpeed * 3f * Time.deltaTime);
                this.transform.position = newTargetPosition;
                //rb.MovePosition(newTargetPosition);
            }
            else
            {
                if (chasingPlayer)
                    chasingPlayer = false;
                rb.linearVelocity = Vector2.left * moveSpeed;
            }
        }
    }
    bool PlayerDetected()
    {
        Vector2 playerDistance = rb.transform.TransformPoint(player.position);
        if (Mathf.Abs(playerDistance.x) <= 12 && rb.position.x > player.position.x)
            return true;
        else
            return false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
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
    public void SwoopIn()
    {
        if (PlayerDetected())
            chasingPlayer = true;
    }
}
