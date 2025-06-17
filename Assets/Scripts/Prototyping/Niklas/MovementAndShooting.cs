using UnityEngine;
using UnityEngine.Rendering;

public class MovementAndShooting : MonoBehaviour
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
    public enum ControlScheme {HoldToWalk, TapJump};
    public ControlScheme Scheme = ControlScheme.HoldToWalk;

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
        if (Input.GetMouseButtonDown(0) && MouseNearPlayer())
        {
            aiming = true;
            aimStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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


        if (Input.GetMouseButton(0) && !aiming)
        {
            ApplyMovement();
        }
    }

    void ApplyMovement()
    {
        if (Scheme == ControlScheme.HoldToWalk)
        {
            transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
        }
    }

    void ApplyAim()
    {
        aimEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        DebugExtension.DebugArrow(transform.position, (aimStart - aimEnd) * AimMultiplier, Color.green);
        arrow.SetPosition(1, (aimStart - aimEnd) * AimMultiplier);
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
        GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddForce((aimStart - aimEnd) * AimMultiplier * 100);
    }
}
