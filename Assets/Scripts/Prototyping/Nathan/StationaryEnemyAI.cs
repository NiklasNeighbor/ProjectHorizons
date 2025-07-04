using TMPro;
using UnityEngine;

public class StationaryEnemyAI : MonoBehaviour
{
    public Transform player;
    [SerializeField] GameObject projectile;
    bool detectedPlayer;
    [SerializeField] int pointsOnDeath;
    [SerializeField] Animator anim;
    [SerializeField] Transform throwPoint;
    [SerializeField] Vector2 throwForce;
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
                detectedPlayer = true;
                anim.SetBool("PlayerDetected", detectedPlayer);
                //moveSpeed = moveSpeed * 2;
            }
        }
        // rb.linearVelocityX = moveSpeed;
    }

    void ThrowProjectile()
    {
        GameObject spawnedProjectile = Instantiate(projectile, throwPoint.position, Quaternion.identity);
        Rigidbody2D projectileRB = spawnedProjectile.GetComponent<Rigidbody2D>();
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
        ScoreManager scoreManager = GameObject.FindWithTag("GameController").GetComponent<ScoreManager>();
        scoreManager.IncreaseScore(pointsOnDeath);
        scoreManager.LightUpScoreText();
        Destroy(this.gameObject);
    }
}
