using UnityEngine;

public class JumpOrb : MonoBehaviour
{
    [SerializeField] float force;
    bool hasJumped = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!hasJumped)
        {
            if (collision.CompareTag("Player"))
            {
                
                MovementAndShooting movementAndShooting = collision.gameObject.GetComponent<MovementAndShooting>();
                if (Input.GetMouseButton(0) && movementAndShooting.MouseOnRightSide(true))
                {
                    
                    movementAndShooting.DoJump(force);
                }
            }

            hasJumped = true;
        }
    }
}
