using UnityEngine;

public class CurrentZone : MonoBehaviour
{
    [Header("Current Direction (world space)")]
    public Vector3 currentDirection = new Vector3(1f, 0f, 0f);

    [Header("Strength")]
    public float strength = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.externalPush = currentDirection.normalized * strength;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.externalPush = Vector3.zero;
        }
    }
}
