using UnityEngine;

public class Nest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Only react if the player enters
        if (!other.CompareTag("Player"))
            return;

        PlayerFollower follower = other.GetComponent<PlayerFollower>();
        if (follower == null || follower.babies.Count == 0)
            return;

        int count = follower.babies.Count;

        // Remove the babies that are currently following
        foreach (var baby in follower.babies)
        {
            if (baby != null)
            {
                Destroy(baby.gameObject);   // For now they just disappear at the nest
            }
        }

        follower.babies.Clear();

        // Tell the GameManager we delivered some babies
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DeliverBabies(count);
        }
    }
}
