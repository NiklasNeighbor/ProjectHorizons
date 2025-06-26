using UnityEngine;
using UnityEngine.Rendering;

public class MovementAndShooting : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float AimingSpeed = 1f;
    public float JumpForce = 1f;
    public float JumpRaycastLength = 1f;
    public float PlayerDragSize = 1f;
    float regularGravity;
    public float FallGravityMultiplier;
    public GameObject JumpParticle; // the effect prefab to spawn
    bool aiming = false;
    Vector2 aimStart;
    Vector2 aimEnd;
    public float AimMultiplier = 1f;
    public GameObject ProjectilePrefab;
    public LevelGeneration LevelGeneration;
    Rigidbody2D rb;
    LineRenderer arrow;
    public Animator animator;
    public LayerMask Ground;

    [SerializeField] bool UseAltControls;
    [SerializeField] BackgroundScript BackgroundScript;
    [SerializeField] float BgSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        regularGravity = rb.gravityScale;
        arrow = GetComponent<LineRenderer>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        arrow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ControlsManager();
    }

    void ControlsManager()
    {
        /*
        if(Input.GetMouseButtonDown(0))
        {
            if(UseAltControls && MouseOnRightSide(false))
            {
                aimStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }else
            if(MouseNearPlayer())
            {
                aimStart = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            }
            aiming = true;
            arrow.enabled = true;
        }
        */

        if (Input.GetMouseButtonDown(0) && UseAltControls && MouseOnRightSide(false))
        {
            aimStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aiming = true;
            arrow.enabled = true;
        }
        else
if (Input.GetMouseButtonDown(0) && MouseNearPlayer())
        {
            aiming = true;
            aimStart = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            arrow.enabled = true;
        }

        if (aiming)
        {
            ApplyAim();
        }

        if (Input.GetMouseButtonUp(0) && aiming)
        {
            aiming = false;
            FireProjectile();
            arrow.enabled = false;
        }


        if (!aiming)
        {
            ApplyMovement();
        }

        if (Input.GetMouseButtonDown(0) && !aiming)
        {
            if (UseAltControls && MouseOnRightSide(true))
            {
                DoJump();
            }
            else
            if (!UseAltControls)
            {
                DoJump();
            }
        }

        if ((Input.GetMouseButton(0) && !aiming))
        {
            rb.gravityScale = regularGravity;
        } else
        {
            rb.gravityScale = regularGravity * FallGravityMultiplier;
        }

        AnimationControl();



    }

    void ApplyMovement()
    {
        if (!CollisionOnRight())
        {
            LevelGeneration.ScrollAdvance(MoveSpeed * Time.deltaTime);
            BackgroundScript.ScrollAdvance(MoveSpeed * BgSpeed * Time.deltaTime);
        }
    }

    void DoJump()
    {
        Debug.Log("Try Jump");
        if (Physics2D.Raycast(transform.position, Vector2.down, JumpRaycastLength, Ground))
        {
            GameObject spawned = Instantiate(JumpParticle, transform.position, Quaternion.identity);
            spawned.SetActive(true);//spawns jump effect
            rb.AddForce(Vector2.up * JumpForce);
            Debug.Log("Jumped");
        }
    }

    void ApplyAim()
    {
        if (!CollisionOnRight())
        {
            LevelGeneration.ScrollAdvance(AimingSpeed * Time.deltaTime);
            BackgroundScript.ScrollAdvance(AimingSpeed * BgSpeed * Time.deltaTime);
        }
        
        aimEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 ownPosition = new Vector2(transform.position.x, transform.position.y);

        if (UseAltControls)
        {
            DebugExtension.DebugArrow(transform.position, (ownPosition - aimStart) * AimMultiplier, Color.green);
            arrow.SetPosition(1, (aimStart - aimEnd) * AimMultiplier);
        }
        else
        {
            DebugExtension.DebugArrow(transform.position, ((ownPosition + aimStart) - (aimEnd)) * AimMultiplier, Color.green);
            arrow.SetPosition(1, ((ownPosition + aimStart) - aimEnd) * AimMultiplier);
        }

        Debug.Log("Aiming.");
    }

    bool MouseNearPlayer()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(mousePos, transform.position) < PlayerDragSize)
        {
            return true;
        } 
        return false;
    }

    void FireProjectile()
    {
        Vector2 ownPosition = new Vector2(transform.position.x, transform.position.y);
        GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        if (UseAltControls)
        {
            projectileRb.AddForce((aimStart - aimEnd) * AimMultiplier * 100);
        }
        else
        {
            projectileRb.AddForce(((ownPosition + aimStart) - aimEnd) * AimMultiplier * 100);
        }
    }

    bool CollisionOnRight()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, 0.6f, Ground))
        {
            return true ;
        }
        return false;
    }

    public void AnimationControl()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, JumpRaycastLength, Ground))
        {
            animator.SetBool("Grounded", true);
        }
        else
        {
            animator.SetBool("Grounded", false);
        }

        animator.SetBool("Aiming", aiming);
        animator.SetFloat("VerticalSpeed", rb.linearVelocityY);
    }

    //check on what side of screen mouse is
    bool MouseOnRightSide(bool right)
    {
        Vector2 mouseScreenPos = Input.mousePosition;

        if (right && mouseScreenPos.x > Screen.width / 2)
        {
            return true;
        }

        if (!right && mouseScreenPos.x < Screen.width / 2)
        {
            return true;
        }
        return false;
    }
}
