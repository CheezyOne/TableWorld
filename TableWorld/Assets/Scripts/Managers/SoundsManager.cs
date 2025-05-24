using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Button,
    CupBreak,
    Victory,
    TrainMouseSpawn,
    MouseHit,
    MouseGotHit,
    DirtySpot,
    Stomp,
    MousetrapHit,
    WaterDropPickUp,
    DecoyPickUp,
    DecoyThrow,
}

public class SoundsManager : SingletonDontDestroyOnLoad<SoundsManager>
{
    [SerializeField] private SoundsData[] _soundsDatas;
    [SerializeField] private AudioSource _musicObject;
    private Dictionary<SoundType, AudioClipData> _soundIdPairs = new();

    private WaitForSecondsRealtime _waitForCoolDown = new (0.1f);

    private void Start()
    {
        foreach (SoundsData soundData in _soundsDatas)
            _soundIdPairs.Add(soundData.SoundType, soundData.AudioClipData);
    }

    public void ToggleMusic()
    {
        _musicObject.gameObject.SetActive(!_musicObject.gameObject.activeSelf);
    }

    public void PlaySound(SoundType soundType)
    {
        if (!SaveLoadSystem.data.SoundsOn)
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

    private void OnEnable()
    {
        EventBus.OnMusicToggle += ToggleMusic;
    }

    private void OnDisable()
    {
        EventBus.OnMusicToggle -= ToggleMusic;
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