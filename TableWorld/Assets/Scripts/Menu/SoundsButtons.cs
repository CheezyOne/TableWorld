using UnityEngine;
using UnityEngine.UI;

public class SoundsButtons : MonoBehaviour
{
    [SerializeField] private Sprite _soundsOff;
    [SerializeField] private Sprite _soundsOn;
    [SerializeField] private Sprite _musicOn;
    [SerializeField] private Sprite _musicOff;
    [SerializeField] private Image _musicImage;
    [SerializeField] private Image _soundsImage;

    public void Initialize()
    {
        _soundsImage.sprite = SaveLoadSystem.data.SoundsOn ? _soundsOn : _soundsOff;
        _musicImage.sprite = SaveLoadSystem.data.MusicOn ? _musicOn : _musicOff;
    }

    public void ToggleMusic()
    {
        SaveLoadSystem.data.MusicOn = !SaveLoadSystem.data.MusicOn;
        SaveLoadSystem.Instance.Save();
        _musicImage.sprite = SaveLoadSystem.data.MusicOn ? _musicOn : _musicOff;
        EventBus.OnMusicToggle?.Invoke();
    }

    public void ToggleSounds()
    {
        SaveLoadSystem.data.SoundsOn = !SaveLoadSystem.data.SoundsOn;
        SaveLoadSystem.Instance.Save();
        _soundsImage.sprite = SaveLoadSystem.data.SoundsOn ? _soundsOn : _soundsOff;
        EventBus.OnSoundsToggle?.Invoke();
    }
}