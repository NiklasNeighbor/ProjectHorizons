using UnityEngine;

public class PlayerStateChecker : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManagerRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damager")
        {
            Time.timeScale = 0;
            scoreManagerRef.gameActive = false;
            scoreManagerRef.EndRun();
            Destroy(this.gameObject);
        }
    }
}
