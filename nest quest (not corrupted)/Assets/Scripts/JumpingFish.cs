using System.Collections;
using UnityEngine;

public class JumpingFish : MonoBehaviour
{
    [Header("References")]
    public Transform fishBody;    // object with collider + HazardFish
    public Transform shadow;      // flat shadow at water surface

    [Header("Timing")]
    public float restTime = 2f;       // time hidden between jumps
    public float warningTime = 0.8f;  // time shadow shows before jump
    public float jumpDuration = 1.2f; // how long the arc lasts

    [Header("Jump Shape")]
    public float jumpHeight = 3f;     // height of the arc
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] jumpClips;     // <-- array for multiple splashes

    private Vector3 startPosition;
    private Collider fishCollider;

    private void Awake()
    {
        if (fishBody == null)
            fishBody = transform;  // fallback

        startPosition = fishBody.position;
        fishCollider = fishBody.GetComponent<Collider>();
    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        StartCoroutine(JumpLoop());
    }

    private IEnumerator JumpLoop()
    {
        while (true)
        {
            // 1) Hidden / resting
            SetFishVisible(false);
            SetShadowVisible(false);
            yield return new WaitForSeconds(restTime);

            // 2) Warning — shadow appears where fish will pop up
            SetShadowVisible(true);
            yield return new WaitForSeconds(warningTime);

            // 3) Jump — fish follows an arc and is hazardous
            SetFishVisible(true);

            // Play random jump sound
            if (audioSource != null && jumpClips != null && jumpClips.Length > 0)
            {
                int index = Random.Range(0, jumpClips.Length); // upper bound exclusive
                audioSource.PlayOneShot(jumpClips[index]);
            }

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / jumpDuration;
                t = Mathf.Clamp01(t);

                // simple parabola: peak at t = 0.5
                float yOffset = 4f * jumpHeight * t * (1f - t);

                Vector3 pos = startPosition;
                pos.y = startPosition.y + yOffset;

                fishBody.position = pos;

                yield return null;
            }

            // 4) Return to start position (under water / at surface)
            fishBody.position = startPosition;
            SetFishVisible(false);

            // shadow will be hidden next loop iteration (we hide it at start of next loop)
        }
    }

    private void SetFishVisible(bool visible)
    {
        if (fishBody != null)
        {
            fishBody.gameObject.SetActive(visible);
        }

        if (fishCollider != null)
        {
            fishCollider.enabled = visible;
        }
    }

    private void SetShadowVisible(bool visible)
    {
        if (shadow != null)
        {
            shadow.gameObject.SetActive(visible);
        }
    }
}
