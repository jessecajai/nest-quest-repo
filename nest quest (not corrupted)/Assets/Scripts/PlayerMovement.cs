using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [HideInInspector] public float speedMultiplier = 1f;   // for slow zones
    [HideInInspector] public Vector3 externalPush = Vector3.zero; // for currents

    private Rigidbody rb;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void FixedUpdate()
    {
        // Player-controlled movement
        Vector3 moveVelocity = moveInput * moveSpeed * speedMultiplier;

        // Add any external push (currents)
        Vector3 finalVelocity = moveVelocity + externalPush;

        Vector3 moveDelta = finalVelocity * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDelta);
    }
}
