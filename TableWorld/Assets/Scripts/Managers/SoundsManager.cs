using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SoundType
{
    Button,
    CupBreak,
    Victory,
    TrainMouseSpawn,
    MouseHit,
    DirtySpot,
    Stomp,
    MousetrapHit,
}

public class SoundsManager : SingletonDontDestroyOnLoad<SoundsManager>
{
    [SerializeField] private Sprite _soundsOff;
    [SerializeField] private Sprite _soundsOn;
    [SerializeField] private Sprite _musicOn;
    [SerializeField] private Sprite _musicOff;
    [SerializeField] private Image _musicImage;
    [SerializeField] private Image _soundsImage;
    [SerializeField] private SoundsData[] _soundsDatas;
    [SerializeField] private AudioSource _musicObject;
    private Dictionary<SoundType, AudioClipData> _soundIdPairs = new();

    private bool _isSoundOn = true;
    private WaitForSeconds _waitForCoolDown = new (0.1f);

    private void Start()
    {
        foreach (SoundsData soundData in _soundsDatas)
            _soundIdPairs.Add(soundData.SoundType, soundData.AudioClipData);
    }

    public void ToggleSounds()
    {
        _isSoundOn = !_isSoundOn;
        _soundsImage.sprite = _isSoundOn? _soundsOn : _soundsOff;
    }

    public void ToggleMusic()
    {
        _musicObject.gameObject.SetActive(!_musicObject.gameObject.activeSelf);
        _musicImage.sprite = _musicObject.gameObject.activeSelf ? _musicOn : _musicOff;
    }

    public void PlaySound(SoundType soundType)
    {
        if (!_isSoundOn)
            return;

        if(!_soundIdPairs.ContainsKey(soundType))
        {
            Debug.LogWarning($"There's no audioclip for sound type {soundType}!");
            return;
        }

        if (_soundIdPairs[soundType].IsOnCooldown)
            return;

        _soundIdPairs[soundType].IsOnCooldown = true;
        StartCoroutine(ResetCoolDown(soundType));
        _soundIdPairs[soundType].AudioSource?.PlayOneShot(_soundIdPairs[soundType].AudioClip);
    }

    private IEnumerator ResetCoolDown(SoundType soundType)
    {
        yield return _waitForCoolDown;
        _soundIdPairs[soundType].IsOnCooldown = false;
    }

    [Serializable]
    public class AudioClipData
    {
        public AudioClip AudioClip;
        public AudioSource AudioSource;
        [HideInInspector] public bool IsOnCooldown;
    }

    [Serializable] 
    public class SoundsData
    {
        public SoundType SoundType;
        public AudioClipData AudioClipData;
    }
}