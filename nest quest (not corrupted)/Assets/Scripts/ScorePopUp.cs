using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float lifetime = 1f;
    public float moveUpSpeed = 1f;

    private float timer = 0f;
    private CanvasGroup canvasGroup;
    private Camera mainCam;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        mainCam = Camera.main;
    }

    public void Setup(int points, Color color)
    {
        if (text != null)
        {
            text.text = "+" + points;
            text.color = color;
        }
    }

    private void Update()
    {
        // float upward
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // face the camera so it’s always readable
        if (mainCam != null)
        {
            transform.forward = mainCam.transform.forward;
        }

        // fade out over lifetime
        timer += Time.deltaTime;
        float t = timer / lifetime;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
