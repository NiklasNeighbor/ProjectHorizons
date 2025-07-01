using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateChecker : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManagerRef;
    [SerializeField] Animator anim;
    [SerializeField] float SlowDownAfterSec;
    [SerializeField] float EndGameSlowSpeed;
    bool hasDied = false;
    bool hasStopped = false;
    float speedScale = 1;
    MovementAndShooting movementAndShooting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        movementAndShooting = GetComponent<MovementAndShooting>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Damager")
        {
            anim.SetBool("Dead", true);
            KillPlayer();
        }
        if(collision.gameObject.tag == "DeathZone")
        {
            Time.timeScale = 0;
            scoreManagerRef.gameActive = false;
            scoreManagerRef.EndRun();
            Destroy(this.gameObject);
            KillPlayer();
        }
    }

    private void Update()
    {
        if(hasDied && !hasStopped)
        {
            Dying();
        }
    }

    void Dying()
    {
        SlowDownAfterSec -= Time.unscaledDeltaTime;
        if (SlowDownAfterSec <= 0)
        {
            speedScale -= Time.unscaledDeltaTime * EndGameSlowSpeed;
            speedScale = Mathf.Clamp(speedScale, 0, 1);
            movementAndShooting.AdjustSpeed(speedScale);
        }

        if(speedScale == 0)
        {
            scoreManagerRef.EndRun();
            hasStopped = true;
            Time.timeScale = 0;
        }
    }

    public void KillPlayer()
    {
        scoreManagerRef.gameActive = false;
        //start falling animation
        hasDied = true;
        //Destroy(this.gameObject);
    }


}
