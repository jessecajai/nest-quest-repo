using UnityEngine;

public class HazardFish : MonoBehaviour
{
    [Header("Scatter Settings")]
    public float scatterRadius = 3f;

    private PlayerFollower playerFollower;
    private Transform playerTransform;
    private int hitsOnPlayer = 0;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerFollower = player.GetComponent<PlayerFollower>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the game is already over, ignore
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        if (other.CompareTag("Player"))
        {
            HandleHitPlayer();
        }
        else
        {
            // Check if we hit a baby swan
            BabySwan baby = other.GetComponent<BabySwan>();
            if (baby != null)
            {
                HandleHitBaby(baby);
            }
        }
    }

    private void HandleHitPlayer()
    {
        if (playerFollower == null || playerTransform == null) return;

        hitsOnPlayer++;

        // On the second hit, scatter ALL babies
        if (hitsOnPlayer >= 2)
        {
            playerFollower.ScatterAll(scatterRadius, playerTransform.position);
            hitsOnPlayer = 0; // reset the counter
        }
    }

    private void HandleHitBaby(BabySwan baby)
    {
        if (playerFollower == null || playerTransform == null) return;

        // This will scatter the baby that was hit AND all behind it
        playerFollower.ScatterFromBaby(baby, scatterRadius, playerTransform.position);
    }
}
