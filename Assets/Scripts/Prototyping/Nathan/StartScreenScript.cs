using UnityEngine;
using TMPro;
public class StartScreenScript : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    [SerializeField] Animator playerAnim;
    [SerializeField] MovementAndShooting movementRef;
    [SerializeField] GameObject score;
    [SerializeField] Animator startScreenAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        score.SetActive(false);
        playerAnim.SetBool("GameStarted", false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void FadeOutUI()
    {
        Debug.Log("Fade out?");
        startScreenAnim.Play("Fade Out");
    }
    public void StartGame()
    {
        score.SetActive(true);
        playerAnim.SetBool("GameStarted", true);
        startScreen.SetActive(false);
        movementRef.StartGame();
    }
}
