using System.Runtime.InteropServices;
using UnityEngine;

public class LeaderboardHandler : Singleton<LeaderboardHandler>
{
    public enum ScoreType 
    {
        Number, 
        Float 
    }

    public static string HIGH_SCORE_LEADERBOARD = "HighScore";

    [DllImport("__Internal")]
    private static extern void SubmitLeaderboardScore(string leaderboardName, int score, string scoreType);

    public void SubmitScore(string leaderboardName, int score, ScoreType scoreType = ScoreType.Number)
    {
        if (AdsManager.IsWebGL())
        {
            SubmitLeaderboardScore(
                leaderboardName,
                score,
                scoreType == ScoreType.Float ? "float" : "number"
            );
        }
        else
        {
            Debug.Log($"WebGL Leaderboard Submit: {leaderboardName} = {score} ({scoreType})");
        }
    }

}