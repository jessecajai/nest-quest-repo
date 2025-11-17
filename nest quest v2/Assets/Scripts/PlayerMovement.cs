using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get WASD / Arrow key input
        float horizontal = Input.GetAxisRaw("Horizontal");  // A/D or Left/Right
        float vertical = Input.GetAxisRaw("Vertical");      // W/S or Up/Down

        // Combine into a movement direction on the XZ plane
        moveInput = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void FixedUpdate()
    {
        // Move the rigidbody based on input
        Vector3 moveDelta = moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDelta);
    }
}
