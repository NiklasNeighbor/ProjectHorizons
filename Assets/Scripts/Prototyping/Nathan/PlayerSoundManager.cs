using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource shootSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource shortDeathSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Jump()
    {
        Debug.Log("hey");
        jumpSound.Play();
    }
    public void Death(bool diedFromEnemy)
    {
        if(diedFromEnemy)
            deathSound.Play();
        else
            shortDeathSound.Play();
    }
    public void Shoot()
    {
        shootSound.Play();
    }
}
