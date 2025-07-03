using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MovementAndShooting : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float AimingSpeed = 1f;
    public bool SlowdownThroughTimescale = true;
    public float AimingTimeScale = 0.25f;
    public float ShootingCooldown;
    public float JumpForce = 1f;
    public float JumpRaycastLength = 1f;
    public float PlayerDragSize = 1f;
    float regularGravity;
    public float FallGravityMultiplier;
    public GameObject JumpParticle; // the effect prefab to spawn
    public GameObject RechargeParticle;
    bool aiming = false;
    bool canShoot = true;
    Vector2 aimStart;
    Vector2 aimEnd;
    public float AimMultiplier = 1f;
    public GameObject ProjectilePrefab;
    public Vector3 ProjectileOriginOffset;
    LevelGeneration levelGeneration;
    Rigidbody2D rb;
    LineRenderer arrow;
    public Animator animator;
    public LayerMask Ground;

    [SerializeField] bool UseAltControls;
    BackgroundScript backgroundScript;
    [SerializeField] float BgSpeed;

    [SerializeField] GameObject GameManager;
    ScoreManager scoreManager;
    DifficultyManager difficultyManager;

    bool stopMoving = false;

    [SerializeField] float addPointsPerSec;
    float pointTimer = 0;
    float speedDeathMultiplier = 1;
    [SerializeField] bool noPoints;
    public bool IsDead;
    [SerializeField] Vector2 leftnRightBounds;
    Transform childTf;
    bool isGrounded = false;
    [SerializeField] float MaxExtraSpeed;
    [SerializeField] public bool dissableMovemement; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ProjectileOriginOffset.z = 0;
        rb = GetComponent<Rigidbody2D>();
        regularGravity = rb.gravityScale;
        arrow = GetComponent<LineRenderer>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        arrow.enabled = false;

        levelGeneration = GameManager.GetComponent<LevelGeneration>();
        backgroundScript = GameManager.GetComponent<BackgroundScript>();
        scoreManager = GameManager.GetComponent<ScoreManager>();

        if (levelGeneration.generateFlat)
        {
            MoveSpeed *= 0.5f;
        }
        difficultyManager = GameManager.GetComponent<DifficultyManager>();
       

        pointTimer = addPointsPerSec;

        childTf = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();

        if (!IsDead)
        {
            ControlsManager();
        }

        RotatePlayer();
        ApplyControls();
    }

    private void FixedUpdate()
    {
        if (!dissableMovemement)
        {
            if (!noPoints)
            {
                UpdatePoints();
            }
            UpdateDifficulty();
        }
    }

    void CheckGrounded()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, JumpRaycastLength, Ground))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void UpdatePoints()
    {
        if (!aiming)
        {
            pointTimer -= Time.deltaTime;
        }
        else
        {
            pointTimer -= Time.deltaTime * (AimingSpeed / MoveSpeed);
        }


        if (pointTimer <= 0)
        {
            scoreManager.IncreaseScore(1);
            pointTimer = addPointsPerSec;
        }
    }

    void UpdateDifficulty()
    {
        if (!aiming)
        {
            difficultyManager.IncreaseDifficulty(4);
        }
        else
        {
            difficultyManager.IncreaseDifficulty(2);
        }
    }


    void ControlsManager()
    {
        if (!dissableMovemement)
        {
            if (canShoot)
            {
                if (Input.GetMouseButtonDown(0) && UseAltControls && MouseOnRightSide(false))
                {
                    aimStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    aiming = true;
                    arrow.enabled = true;
                }
                else if (Input.GetMouseButtonDown(0) && MouseNearPlayer())
                {
                    aiming = true;
                    aimStart = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    arrow.enabled = true;
                }
            }

            if (canShoot)
            {
                if (Input.GetMouseButtonUp(0) && aiming)
                {
                    aiming = false;
                    Time.timeScale = 1;
                    canShoot = false;
                    FireProjectile();
                    StartCoroutine(StartShotCooldown());
                    arrow.enabled = false;
                }
            }
            
            if(isGrounded)
            {
                            if (Input.GetMouseButtonDown(0) && !aiming)
            {
                if (UseAltControls && MouseOnRightSide(true))
                {
                    DoJump(JumpForce);
                }
                else
                if (!UseAltControls)
                {
                    DoJump(JumpForce);
                }
            }
            }

            if ((Input.GetMouseButton(0) && !aiming))
            {
                rb.gravityScale = regularGravity;
            }
            else
            {
                rb.gravityScale = regularGravity * FallGravityMultiplier;
            }
        }
        AnimationControl();
    }

    void ApplyControls()
    {
        if (aiming)
        {
            ApplyAim();
        }

        if (!aiming && !stopMoving)
        {
            ApplyMovement();
        }
    }

    void ApplyMovement()
    {
        if (!CollisionOnRight())
        {
            float extraSpeed = MaxExtraSpeed * difficultyManager.addedSpeed;
            float speed = (MoveSpeed + extraSpeed) * speedDeathMultiplier;
            levelGeneration.ScrollAdvance(speed * Time.deltaTime);
            if (backgroundScript != null)
            {
                backgroundScript.ScrollAdvance(speed * BgSpeed * Time.deltaTime);
            }
        }
    }

    public void DoJump(float force)
    {
        GameObject spawned = Instantiate(JumpParticle, transform.position, Quaternion.identity);
        spawned.SetActive(true);//spawns jump effect
        rb.AddForce(Vector2.up * force);
        //Debug.Log("Jumped");
    }

    void ApplyAim()
    {
        if (!CollisionOnRight())
        {
            float extraSpeed = MaxExtraSpeed * difficultyManager.addedSpeed;
            float speed = (MoveSpeed + extraSpeed) * speedDeathMultiplier;
            if (SlowdownThroughTimescale)
            {
                levelGeneration.ScrollAdvance(speed * Time.deltaTime);
                Time.timeScale = AimingTimeScale;
            }
            else
            {
                levelGeneration.ScrollAdvance((AimingSpeed + extraSpeed) * Time.deltaTime);
            }


            if (backgroundScript != null)
            {
                backgroundScript.ScrollAdvance((AimingSpeed + extraSpeed) * BgSpeed * Time.deltaTime);
            }
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

        //Debug.Log("Aiming.");
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
        GameObject projectile = Instantiate(ProjectilePrefab, transform.position + ProjectileOriginOffset, Quaternion.identity);
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
            return true;
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
    public bool MouseOnRightSide(bool right)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + ProjectileOriginOffset, 0.2f);
    }

    public void StopMoving()
    {
        stopMoving = true;
    }

    public void AdjustSpeed(float multiplier)
    {
        speedDeathMultiplier = multiplier;
    }

    void RotatePlayer()
    {
        /*
        Vector2 leftBound = transform.position + new Vector3(leftnRightBounds.x, 0, 0);
        Vector2 rightBound = transform.position + new Vector3(leftnRightBounds.y, 0, 0);
        RaycastHit2D leftRay = Physics2D.Raycast(leftBound, Vector2.down, JumpRaycastLength + 0.5f, Ground);
        RaycastHit2D rightRay = Physics2D.Raycast(rightBound, Vector2.down, JumpRaycastLength + 0.5f, Ground);
        */

        //float angle = Mathf.Atan2(leftRay.point.y - rightRay.point.y, leftRay.point.x - rightRay.point.x) * Mathf.Rad2Deg + 180;
        //if (!Physics2D.Raycast(transform.position, Vector2.down, JumpRaycastLength + 0.5f, Ground))
        //{
        //    angle = 0;
        //}

        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, JumpRaycastLength + 0.75f, Ground);
        float angle = Mathf.Atan2(hit2D.normal.x, hit2D.normal.y) * Mathf.Rad2Deg;
        childTf.eulerAngles = new Vector3(childTf.eulerAngles.x, childTf.eulerAngles.y, -angle);
    }
    public void StartGame()
    {
        dissableMovemement = false;
        MoveSpeed *= 2;
        scoreManager = GameManager.GetComponent<ScoreManager>();
        difficultyManager = GameManager.GetComponent<DifficultyManager>();
    }

    public IEnumerator StartShotCooldown()
    {
        yield return new WaitForSeconds(ShootingCooldown);
        canShoot = true;
        if (RechargeParticle != null)
        {
            Instantiate(RechargeParticle, transform.position, Quaternion.identity);
        }
    }
}

