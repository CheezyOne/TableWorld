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

    private void Awake()
    {
        _soundsImage.sprite = GameInfoHolder.SoundOn ? _soundsOn : _soundsOff;
        _musicImage.sprite = GameInfoHolder.MusicOn ? _musicOn : _musicOff;
    }

    public void ToggleMusic()
    {
        GameInfoHolder.MusicOn = !GameInfoHolder.MusicOn;
        _musicImage.sprite = GameInfoHolder.MusicOn ? _musicOn : _musicOff;
        EventBus.OnMusicToggle?.Invoke();
    }

    public void ToggleSounds()
    {
        GameInfoHolder.SoundOn = !GameInfoHolder.SoundOn;
        _soundsImage.sprite = GameInfoHolder.SoundOn ? _soundsOn : _soundsOff;
        EventBus.OnSoundsToggle?.Invoke();
    }
}