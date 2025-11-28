using UnityEngine;

public class SlowZone : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float slowFactor = 0.5f;   // 0.5 = half speed

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.speedMultiplier = slowFactor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.speedMultiplier = 1f;   // back to normal
        }
    }
}
