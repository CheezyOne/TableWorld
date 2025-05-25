using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialWindow : BaseWindow
{
    [SerializeField] private TMP_Text _tutorialText;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private TMP_Text _tutorialNumberText;

    private int _tutorialIndex;
    private int _tutorialsCount;

    private const string TUTORIALS_KEY = "tutorials";
    private const string TUTORIALS_URLS_KEY = "tutorials_URLs";

    public override void Init()
    {
        base.Init();
        _tutorialsCount = LanguageSystem.Instance.GetTranslatedArray(TUTORIALS_KEY).Length;
        SetTutorial();
    }

    public void NextTutorial()
    {
        _tutorialIndex++;

        if (_tutorialIndex >= _tutorialsCount)
        {
            GameAnalytics.TrackEvent("seen_whole_tutorial");
            SaveLoadSystem.data.IsTutorialComplete = true;
            SaveLoadSystem.Instance.Save();
            _tutorialIndex = 0;
        }

        SetTutorial();
    }

    private void SetTutorial()
    {
        _tutorialText.text = LanguageSystem.Instance.GetTranslatedTextFromArrayByID(TUTORIALS_KEY, _tutorialIndex);
        _tutorialNumberText.text = (_tutorialIndex + 1) + "/" + _tutorialsCount;
        _videoPlayer.Stop();
        _videoPlayer.url = LanguageSystem.Instance.GetTranslatedTextFromArrayByID(TUTORIALS_URLS_KEY, _tutorialIndex);
        _videoPlayer.Play();
    }
}