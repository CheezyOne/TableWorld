using UnityEngine;
using TMPro;

public class LoseWindow : BaseWindow
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private int _levelScoreMultiplier;

    private const string SCORE_KEY = "score";

    public override void Init()
    {
        base.Init();
        int score = GetFinalScore();
        _scoreText.text = LanguageSystem.Instance.GetTranslatedText(SCORE_KEY) + score;
        LeaderboardHandler.Instance.SubmitScore(score);
    }

    private int GetFinalScore()
    {
        int score = SaveLoadSystem.data.Level * _levelScoreMultiplier + (int)TimerController.Instance.GetPassedTime();
        return score;
    }
}