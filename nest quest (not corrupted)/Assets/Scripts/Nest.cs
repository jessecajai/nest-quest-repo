using UnityEngine;
using System.Collections.Generic;

public class Nest : MonoBehaviour
{
    [SerializeField] private float popupHeight = 2f;  // how high above the nest the popup appears

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerFollower follower = other.GetComponent<PlayerFollower>();
        if (follower == null || follower.babies.Count == 0)
            return;

        // Copy the list so we can modify the original safely
        List<BabySwan> deliveredBabies = new List<BabySwan>(follower.babies);

        // Remove visual babies from the world
        foreach (var baby in deliveredBabies)
        {
            if (baby != null)
            {
                Destroy(baby.gameObject);
            }
        }

        // Clear the player's train
        follower.babies.Clear();

        // Inform the GameManager (includes score + delivered count + popup)
        if (GameManager.Instance != null)
        {
            Vector3 popupPos = transform.position + Vector3.up * popupHeight;
            GameManager.Instance.DeliverBabies(deliveredBabies, popupPos);
        }
    }
}
