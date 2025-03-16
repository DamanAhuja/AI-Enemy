using UnityEngine;

public class GameStatistics : MonoBehaviour
{
    public static GameStatistics Instance { get; private set; }

    // Calculation Stats
    public int HeadshotCount, BodyshotCount, ShotsCount, ShotsHit, Deaths = 0;
    public float StartTime;

    // Main Stats
    public int Damage, Kills;
    public float HeadshotPercentage, BodyshotPercentage, SurvivalTime, Rating, Accuracy = 0.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartTime = Time.time; // Start survival timer
    }

    void Update()
    {
        // Convert to percentage
        HeadshotPercentage = (ShotsHit > 0) ? ((float)HeadshotCount / ShotsHit) * 100f : 0f;
        BodyshotPercentage = (ShotsHit > 0) ? ((float)BodyshotCount / ShotsHit) * 100f : 0f;
        Accuracy = (ShotsCount > 0) ? ((float)ShotsHit / ShotsCount) * 100f : 0f;
    }

    public void OnPlayerDeath()
    {
        Deaths += 1;
        SurvivalTime = Time.time - StartTime; // Stop survival timer
        CalculateRating(); // Calculate rating at the end of the match
    }

    public void OnMatchEnd()
    {
        SurvivalTime = Time.time - StartTime; // Stop survival timer
        CalculateRating(); // Calculate rating at the end of the match
    }

    void CalculateRating()
    {
        Rating =
            Mathf.Clamp((Kills / 10f) * 30f, 0, 30) +   // Max 30 points
            Mathf.Clamp((Damage / 1000f) * 20f, 0, 20) + // Max 20 points
            Mathf.Clamp((HeadshotPercentage / 100f) * 20f, 0, 20) + // Max 20 points
            Mathf.Clamp((Accuracy / 100f) * 20f, 0, 20) + // Max 20 points
            Mathf.Clamp((SurvivalTime / 600f) * 10f, 0, 10); // Max 10 points
    }
}
