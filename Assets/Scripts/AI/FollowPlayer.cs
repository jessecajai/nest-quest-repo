using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float followSpeed = 2f;       // speed of following
    public float followDistance = 1.5f;  // minimum distance to the player
    public float separationDistance = 0.5f; // distance from other babies

    private Transform playerDuck;

    void Start()
    {
        // Automatically find the player duck in the scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerDuck = playerObj.transform;
        else
            Debug.LogWarning("No GameObject with tag 'Player' found for baby duck to follow.");
    }

    void Update()
    {
        if (playerDuck == null) return;

        Vector3 targetPos = playerDuck.position;

        // Keep a minimum distance from player
        Vector3 toPlayer = targetPos - transform.position;
        float distToPlayer = toPlayer.magnitude;
        if (distToPlayer > followDistance)
        {
            Vector3 moveDir = toPlayer.normalized;

            // Separation from other babies
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, separationDistance);
            foreach (Collider col in hitColliders)
            {
                if (col.CompareTag("Baby") && col.gameObject != this.gameObject)
                {
                    Vector3 away = (transform.position - col.transform.position).normalized;
                    moveDir += away * 0.5f; // separation strength
                }
            }

            moveDir.Normalize();
            transform.position += moveDir * followSpeed * Time.deltaTime;
        }

        // Rotate to face movement direction
        if (toPlayer != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(toPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
        }
    }
}