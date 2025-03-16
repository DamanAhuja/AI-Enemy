using UnityEngine;

public class CareerStatistics : MonoBehaviour
{
    public static CareerStatistics Instance { get; private set; }

    // Calculation Stats
    public int TotalShots, TotalShotsHit, TotalDeaths;
    public float TotalSurvivalTime, TotalRating;

    //Main Stats
    public int Matches, TotalKills, TotalDamage, HighestKills, HighestDamage;
    public float AvgDamage, AvgSurvivalTime, Accuracy, AvgRating, KillDeathRatio;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStats(); // Load saved stats
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCareerStats(GameStatistics lastGameStats)
    {
        Matches++;
        TotalKills += lastGameStats.Kills;
        TotalDamage += lastGameStats.Damage;
        TotalDeaths += lastGameStats.Deaths;    
        TotalSurvivalTime += lastGameStats.SurvivalTime;
        TotalShots += lastGameStats.ShotsCount;
        TotalShotsHit += lastGameStats.ShotsHit;
        TotalRating += lastGameStats.Rating;

        if (lastGameStats.Kills > HighestKills)
            HighestKills = lastGameStats.Kills;

        if (lastGameStats.Damage > HighestDamage)
            HighestDamage = lastGameStats.Damage;

        // Calculate Averages
        AvgDamage = (Matches > 0) ? (float)TotalDamage / Matches : 0f;
        AvgSurvivalTime = (Matches > 0) ? TotalSurvivalTime / Matches : 0f;
        Accuracy = (TotalShots > 0) ? (float)TotalShotsHit / TotalShots : 0f;
        AvgRating = (Matches > 0) ? TotalRating / Matches : 0f;
        KillDeathRatio = (Matches > 0) ? (float)TotalKills / TotalDeaths : (float)TotalKills;

        SaveStats(); // Save updated stats
    }

    void SaveStats()
    {
        PlayerPrefs.SetInt("Matches", Matches);
        PlayerPrefs.SetInt("TotalKills", TotalKills);
        PlayerPrefs.SetInt("TotalDamage", TotalDamage);
        PlayerPrefs.SetInt("TotalDeaths", TotalDeaths);
        PlayerPrefs.SetInt("HighestKills", HighestKills);
        PlayerPrefs.SetInt("HighestDamage", HighestDamage);
        PlayerPrefs.SetInt("TotalShots", TotalShots);
        PlayerPrefs.SetInt("TotalShotsHit", TotalShotsHit);

        PlayerPrefs.SetFloat("TotalSurvivalTime", TotalSurvivalTime);
        PlayerPrefs.SetFloat("TotalRating", TotalRating);
        PlayerPrefs.SetFloat("AvgDamage", AvgDamage);
        PlayerPrefs.SetFloat("AvgSurvivalTime", AvgSurvivalTime);
        PlayerPrefs.SetFloat("Accuracy", Accuracy);
        PlayerPrefs.SetFloat("AvgRating", AvgRating);
        PlayerPrefs.SetFloat("KillDeathRatio", KillDeathRatio);

        PlayerPrefs.Save();
    }

    void LoadStats()
    {
        Matches = PlayerPrefs.GetInt("Matches", 0);
        TotalKills = PlayerPrefs.GetInt("TotalKills", 0);
        TotalDamage = PlayerPrefs.GetInt("TotalDamage", 0);
        TotalDamage = PlayerPrefs.GetInt("TotalDeaths", 0);
        HighestKills = PlayerPrefs.GetInt("HighestKills", 0);
        HighestDamage = PlayerPrefs.GetInt("HighestDamage", 0);
        TotalShots = PlayerPrefs.GetInt("TotalShots", 0);
        TotalShotsHit = PlayerPrefs.GetInt("TotalShotsHit", 0);

        TotalSurvivalTime = PlayerPrefs.GetFloat("TotalSurvivalTime", 0f);
        TotalRating = PlayerPrefs.GetFloat("TotalRating", 0f);
        AvgDamage = PlayerPrefs.GetFloat("AvgDamage", 0f);
        AvgSurvivalTime = PlayerPrefs.GetFloat("AvgSurvivalTime", 0f);
        Accuracy = PlayerPrefs.GetFloat("Accuracy", 0f);
        AvgRating = PlayerPrefs.GetFloat("AvgRating", 0f);
        KillDeathRatio = PlayerPrefs.GetFloat("KillDeathRatio", 0f);
    }

    public void ResetStats()
    {
        PlayerPrefs.DeleteAll();
        LoadStats();
    }
}
