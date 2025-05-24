using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject _deathEffect;
    [SerializeField] private float _deathEffectLifetime;
    [SerializeField] private LoseWindow _loseWindow;
    [SerializeField] private Image _healthBar;

    private bool _isInvincible;

    private const float MAX_HEALTH = 100f;

    private void Awake()
    {
        _healthBar.transform.localScale = new(SaveLoadSystem.data.CurrentHP / MAX_HEALTH, 1, 1);
    }

    public void GetHealed(float health)
    {
        SaveLoadSystem.data.CurrentHP += health;
        SaveLoadSystem.Instance.Save();

        if(SaveLoadSystem.data.CurrentHP > MAX_HEALTH)
            SaveLoadSystem.data.CurrentHP = MAX_HEALTH;

        _healthBar.transform.localScale = new(SaveLoadSystem.data.CurrentHP / MAX_HEALTH, 1, 1);
    }

    public void TakeDamage(float damage)
    {
        if (_isInvincible)
            return;

        SaveLoadSystem.data.CurrentHP -= damage;
        SaveLoadSystem.Instance.Save();
        _healthBar.transform.localScale = new(SaveLoadSystem.data.CurrentHP / MAX_HEALTH, 1,1);

        if (SaveLoadSystem.data.CurrentHP <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(Instantiate(_deathEffect, transform.position, Quaternion.identity), _deathEffectLifetime);
        SoundsManager.Instance.PlaySound(SoundType.CupBreak);
        WindowsManager.Instance.OpenWindow(_loseWindow);
        gameObject.SetActive(false);
        SaveLoadSystem.data.ResetData();
        SaveLoadSystem.Instance.Save();
    }

    private void BecameInvinsible() => _isInvincible = true;

    private void OnEnable()
    {
        EventBus.OnGameEnd += BecameInvinsible;
    }

    private void OnDisable()
    {
        EventBus.OnGameEnd -= BecameInvinsible;
    }
}