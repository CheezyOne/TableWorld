using System.Runtime.InteropServices;

public class LeaderboardHandler : Singleton<LeaderboardHandler>
{
    [DllImport("__Internal")]
    private static extern void SubmitLeaderboardScore(int score);

    public void SubmitScore(int score)
    {
        if (!AdsManager.IsWebGL()) 
            return;

        SubmitLeaderboardScore(score);
    }
}