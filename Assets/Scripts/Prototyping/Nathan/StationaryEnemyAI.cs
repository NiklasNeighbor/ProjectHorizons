using TMPro;
using UnityEngine;

public class StationaryEnemyAI : MonoBehaviour
{
    public bool canShoot;
    public Transform player;
    [SerializeField] GameObject projectile;
    bool detectedPlayer;
    [SerializeField] int pointsOnDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        detectedPlayer = false;
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (Vector2.Distance(this.transform.position, player.position) <= 15 && !detectedPlayer)
            {
                ThrowProjectile();
                detectedPlayer = true;
                //moveSpeed = moveSpeed * 2;
            }
        }
        // rb.linearVelocityX = moveSpeed;
    }

    void ThrowProjectile()
    {
        GameObject spawnedProjectile = Instantiate(projectile, new Vector2(transform.position.x - 1, transform.position.y + 1), Quaternion.identity);
        Rigidbody2D projectileRB = spawnedProjectile.GetComponent<Rigidbody2D>();
        Vector2 throwForce = new Vector2(-8, 3);
        projectileRB.AddForce(throwForce, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("boop");
        }
        if (collision.gameObject.layer == 7)
        {
            Destroy(collision.gameObject);
            
        }
    }

    void Death()
    {
        GameObject.FindWithTag("GameController").GetComponent<ScoreManager>().IncreaseScore(pointsOnDeath);
        Destroy(this.gameObject);
    }
}
