using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource shootSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource shortDeathSound;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Death();
        }
    }
    public void Jump()
    {
        Debug.Log("hey");
        jumpSound.Play();
    }
    public void Death()
    {
        Debug.Log("DEATH");
        //if(diedFromEnemy)
            deathSound.Play();
       // else
           // shortDeathSound.Play();
    }
    public void Shoot()
    {
        shootSound.Play();
    }
}
