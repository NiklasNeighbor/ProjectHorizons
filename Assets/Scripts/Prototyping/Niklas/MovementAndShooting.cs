using UnityEngine;
using UnityEngine.Rendering;

public class MovementAndShooting : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float PlayerDragSize = 1f;
    bool aiming = false;
    Vector2 aimStart;
    Vector2 aimEnd;
    public enum ControlScheme {HoldToWalk, TapJump};
    public ControlScheme Scheme = ControlScheme.HoldToWalk;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            ApplyAim();
        } 
        if (Input.GetMouseButtonUp(0) && aiming)
        {
            aiming = false;
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
        Debug.Log("Aiming...");
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
}
