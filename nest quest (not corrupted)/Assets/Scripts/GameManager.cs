using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI messageText;

    [Header("Score Popups")]
    public ScorePopup scorePopupPrefab;


    [Header("Timer")]
    public float levelTime = 60f;   // set in Inspector if you want
    private float timeRemaining;
    private bool gameOver = false;

    private int babiesDelivered = 0;
    private int totalBabies = 0;

    private int score = 0;

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

    public void DeliverBabies(List<BabySwan> deliveredBabies, Vector3 popupPosition)
    {
        if (gameOver) return;
        if (deliveredBabies == null || deliveredBabies.Count == 0) return;

        int amount = deliveredBabies.Count;

        // 1) Update delivered count
        babiesDelivered += amount;
        if (babiesDelivered > totalBabies)
            babiesDelivered = totalBabies;

        // 2) Sum base scores
        int basePoints = 0;
        foreach (var baby in deliveredBabies)
        {
            if (baby != null)
                basePoints += baby.baseScore;
        }

        // 3) Group multiplier (risk–reward)
        // 1 baby  = x1.0
        // 2–3     = x1.5
        // 4+      = x2.0
        float groupMultiplier = 1f;
        if (amount >= 2 && amount <= 3)
            groupMultiplier = 1.5f;
        else if (amount >= 4)
            groupMultiplier = 2f;

        int pointsEarned = Mathf.RoundToInt(basePoints * groupMultiplier);

        // 4) Add to total score
        score += pointsEarned;

        UpdateScoreUI();

        // 4b) Show score popup at the given world position
        ShowScorePopup(pointsEarned, popupPosition);

        // 5) Win check (same as before)
        if (!gameOver && totalBabies > 0 && babiesDelivered >= totalBabies)
        {
            WinGame();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Babies delivered: {babiesDelivered} / {totalBabies}\nScore: {score}";
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
    public void ShowScorePopup(int points, Vector3 worldPosition)
    {
        if (scorePopupPrefab == null || points <= 0)
            return;

        ScorePopup popup = Instantiate(
            scorePopupPrefab,
            worldPosition,
            Quaternion.identity
        );

        popup.Setup(points, Color.yellow);
    }

}
