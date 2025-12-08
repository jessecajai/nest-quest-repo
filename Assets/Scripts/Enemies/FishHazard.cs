using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHazard : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;       // Swimming speed
    public float range = 3f;       // How far from starting point fish can swim

    [Header("Push Settings")]
    public float pushForce = 2f;   // Force applied to baby ducks

    private Vector3 startPos;
    private Vector3 targetPos;

    void Start()
    {
        startPos = transform.position;
        PickNewTarget();
    }

    void Update()
    {
        MoveFish();
    }

    void MoveFish()
    {
        // Move towards target
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Pick a new random target if reached
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            PickNewTarget();
    }

    void PickNewTarget()
    {
        float offsetX = Random.Range(-range, range);
        float offsetZ = Random.Range(-range, range);
        targetPos = startPos + new Vector3(offsetX, 0, offsetZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Baby"))  // Make sure baby ducklings are tagged "Baby"
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Push the baby away from the fish
                Vector3 pushDir = (other.transform.position - transform.position).normalized;
                rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
            }
        }
    }
}