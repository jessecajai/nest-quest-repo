using UnityEngine;

public class BabySwan : MonoBehaviour
{
    public float followSpeed = 5f;

    private bool isFollowing = false;
    private Transform followTarget;
    private float spacing = 1.5f;
    public int baseScore = 10;   // you can tweak this per-baby in the Inspector


    private void OnTriggerEnter(Collider other)
    {
        // Only react if we hit the player and we're not already following
        if (!isFollowing && other.CompareTag("Player"))
        {
            PlayerFollower playerFollower = other.GetComponent<PlayerFollower>();
            if (playerFollower != null)
            {
                // Let the player decide how to add us
                playerFollower.AddBaby(this);
            }
        }
    }

    // Called by PlayerFollower when we are collected
    public void StartFollowing(Transform newTarget, float spacing)
    {
        isFollowing = true;
        followTarget = newTarget;
        this.spacing = spacing;

        // Optional: change color so we can see we’re collected
        GetComponent<Renderer>().material.color = Color.yellow;
    }
    public void StopFollowing()
    {
        isFollowing = false;
        followTarget = null;

        // Optional: change color back to show it’s not in your train
        GetComponent<Renderer>().material.color = Color.white;
    }


    private void Update()
    {
        if (!isFollowing || followTarget == null) return;

        // Direction from target to this baby (flat on XZ plane)
        Vector3 direction = (transform.position - followTarget.position);
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            // If we're basically on top of the target, pick "behind" it
            direction = -followTarget.forward;
        }

        direction = direction.normalized;

        // Desired position: spacing units away from the target in that direction
        Vector3 targetPosition = followTarget.position + direction * spacing;

        // Smoothly move toward that position
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}
