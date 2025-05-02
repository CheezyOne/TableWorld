using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private GameObject _deathEffect;
    [SerializeField] private float _deathEffectLifetime;
    [SerializeField] private LoseWindow _loseWindow;
    [SerializeField] private Image _healthBar;

    private const float MAX_HEALTH = 100f;

    private void Awake()
    {
        _currentHealth = MAX_HEALTH;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBar.transform.localScale = new(_currentHealth/MAX_HEALTH, 1,1);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(Instantiate(_deathEffect, transform.position, Quaternion.identity), _deathEffectLifetime);
        WindowsManager.Instance.OpenWindow(_loseWindow);
        gameObject.SetActive(false);
    }
}