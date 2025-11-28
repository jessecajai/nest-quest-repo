using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [Header("Following Settings")]
    public float spacing = 1.5f;   // distance between swans

    // List of all collected baby swans
    public List<BabySwan> babies = new List<BabySwan>();

    // Called by BabySwan when the player collects it
    public void AddBaby(BabySwan baby)
    {
        // Who should this baby follow?
        Transform followTarget;

        if (babies.Count == 0)
        {
            // First baby follows the player
            followTarget = transform;
        }
        else
        {
            // Next babies follow the last baby in the list
            followTarget = babies[babies.Count - 1].transform;
        }

        // Tell the baby to start following that target
        baby.StartFollowing(followTarget, spacing);

        // Add to our list
        babies.Add(baby);
    }
      // === NEW: scatter ALL current followers ===
    public void ScatterAll(float radius, Vector3 center)
    {
        foreach (var baby in babies)
        {
            if (baby == null) continue;

            baby.StopFollowing();

            Vector2 random2D = Random.insideUnitCircle.normalized;
            Vector3 randomDir = new Vector3(random2D.x, 0f, random2D.y);

            baby.transform.position = center + randomDir * radius;
        }

        babies.Clear();
    }

    // === NEW: scatter baby that was hit + all behind it ===
    public void ScatterFromBaby(BabySwan hitBaby, float radius, Vector3 center)
    {
        int index = babies.IndexOf(hitBaby);
        if (index == -1) return; // not in the list, nothing to do

        // Work on the tail segment [index .. end]
        for (int i = index; i < babies.Count; i++)
        {
            BabySwan baby = babies[i];
            if (baby == null) continue;

            baby.StopFollowing();

            Vector2 random2D = Random.insideUnitCircle.normalized;
            Vector3 randomDir = new Vector3(random2D.x, 0f, random2D.y);

            baby.transform.position = center + randomDir * radius;
        }

        // Remove that tail segment from the list
        babies.RemoveRange(index, babies.Count - index);
    }
}
