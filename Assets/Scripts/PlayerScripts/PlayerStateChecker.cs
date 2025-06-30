using System.Collections;
using UnityEngine;

public class PlayerStateChecker : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManagerRef;
    [SerializeField] Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
    public void KillPlayer()
    {
        Time.timeScale = 0;
        scoreManagerRef.gameActive = false;
        scoreManagerRef.EndRun();
        //Destroy(this.gameObject);
    }
}
