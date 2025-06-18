using UnityEngine;
using UnityEngine.Rendering;

public class Full2Dmovement : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float PlayerDragSize = 1f;
    bool aiming = false;
    Vector2 aimStart;
    Vector2 aimEnd;
    public float AimMultiplier = 1f;
    public GameObject ProjectilePrefab;
    Rigidbody2D rb;
    LineRenderer arrow;
    public enum ControlScheme { HoldToWalk, TapJump };
    public ControlScheme Scheme = ControlScheme.HoldToWalk;


    Vector2 startMousePos;
    [SerializeField] float JumpClickTime;
    [SerializeField] float JumpStrength;
    float jumpTimer;
    bool canJump;
    [SerializeField] LayerMask floorLayers;
    [SerializeField] float GroundCheckCastLength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        arrow = GetComponent<LineRenderer>();
        arrow.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        ControlsManager();
    }

    void ControlsManager()
    {
        //shooting
        if (Input.GetMouseButtonDown(0) && MouseOnSide(true))
        {
            //aiming = true;
            aimStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //arrow.enabled = true;
            jumpTimer = JumpClickTime;


        }

        //jump
        if(jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime; 
            if(jumpTimer < 0)
            {
                aiming = true;
                arrow.enabled = true;

            }

            if(Input.GetMouseButtonUp(0))
            {
                jumpTimer = 0;
                DoJump();
            }
        }

        //shooting
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
        
        //movement
        if(Input.GetMouseButtonDown(0) && MouseOnSide(false))
        {
            startMousePos = Input.mousePosition;
        }

        if(Input.GetMouseButton(0) && startMousePos != Vector2.zero)
        {
            DoMovement();
        }else
        {
            startMousePos = Vector2.zero;
        }


    }

    void DoMovement()
    {
        Vector2 currentMousePos = Input.mousePosition;
        Vector2 differenceV = currentMousePos - startMousePos;

        transform.position += new Vector3(differenceV.x, 0, 0) * MoveSpeed * 0.0001f;
    }

    void DoJump()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, GroundCheckCastLength, floorLayers) == true)
        {
            rb.AddForce(Vector2.up * JumpStrength);
        }
        
    }

    void ApplyAim()
    {
        aimEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        DebugExtension.DebugArrow(transform.position, (aimStart - aimEnd) * AimMultiplier, Color.green);
        arrow.SetPosition(1, (aimStart - aimEnd) * AimMultiplier);
        Debug.Log("Aiming.");
    }

    bool MouseOnSide(bool right)
    {
        Vector2 mouseScreenPos = Input.mousePosition;

        if (right && mouseScreenPos.x > Screen.width / 2)
        {
            return true;
        }

        if(!right && mouseScreenPos.x < Screen.width / 2)
        {
            return true;
        }
        return false;
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddForce((aimStart - aimEnd) * AimMultiplier * 100);
    }
}

