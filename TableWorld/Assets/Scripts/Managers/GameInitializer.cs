using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameObject _lateLoadPanel;
    [SerializeField] private SoundsButtons _soundsButtons;

    private static bool FirstStart = true;

    private void Awake()
    {
        SaveLoadSystem.OnSavesLoaded += OnSavesLoaded; // Must subscribe in awake or else SavesLoaded will be called before we subscribe and we won't call OnSavesLoaded
        _lateLoadPanel.SetActive(true);

        if (SaveLoadSystem.SavesAreReady && !FirstStart)
        {
            SaveLoadSystem.Instance.SavesLoaded();
        }

        FirstStart = false;
    }

    private void OnDestroy()
    {
        SaveLoadSystem.OnSavesLoaded -= OnSavesLoaded;
    }

    private void OnSavesLoaded()
    {
        _lateLoadPanel.SetActive(false);
        _soundsButtons.Initialize();
    }
}