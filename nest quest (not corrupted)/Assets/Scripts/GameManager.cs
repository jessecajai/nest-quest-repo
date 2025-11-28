using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI messageText;

    [Header("Timer")]
    public float levelTime = 60f;   // set in Inspector if you want
    private float timeRemaining;
    private bool gameOver = false;

    private int babiesDelivered = 0;
    private int totalBabies = 0;

    private PlayerMovement playerMovement;   // to disable controls at end

    public bool IsGameOver => gameOver;

    private void Awake()
    {
        // Basic singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Count all babies in the scene at start
        totalBabies = Object.FindObjectsByType<BabySwan>(FindObjectsSortMode.None).Length;

        // Cache player movement script
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Initialize timer
        timeRemaining = levelTime;

        UpdateScoreUI();
        UpdateTimerUI();
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    private void Update()
    {
        if (gameOver) return;

        // Countdown timer
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f)
        {
            timeRemaining = 0f;
        }
        UpdateTimerUI();

        // Lose when time runs out (if not already won)
        if (timeRemaining <= 0f && !gameOver)
        {
            LoseGame();
        }

        // Win if all babies delivered before time is up
        if (!gameOver && totalBabies > 0 && babiesDelivered >= totalBabies)
        {
            WinGame();
        }
    }

    public void DeliverBabies(int amount)
    {
        if (gameOver) return;

        babiesDelivered += amount;
        if (babiesDelivered > totalBabies)
            babiesDelivered = totalBabies;

        UpdateScoreUI();

        // Optional: immediate win check on delivery
        if (!gameOver && totalBabies > 0 && babiesDelivered >= totalBabies)
        {
            WinGame();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Babies delivered: {babiesDelivered} / {totalBabies}";
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = $"Time: {seconds}";
    }

    private void WinGame()
    {
        gameOver = true;
        if (messageText != null)
        {
            messageText.text = "You did it! Your wife is so proud.";
        }
        SetPlayerControl(false);
    }

    private void LoseGame()
    {
        gameOver = true;
        if (messageText != null)
        {
            messageText.text = "Your wife is disappointed,\n but still loves you.";
        }
        SetPlayerControl(false);
    }

    private void SetPlayerControl(bool enabled)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = enabled;
        }
    }
}
