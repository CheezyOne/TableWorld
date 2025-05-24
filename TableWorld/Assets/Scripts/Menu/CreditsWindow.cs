using System.Collections;
using UnityEngine;

public class CreditsWindow : BaseWindow
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private MovingCredit _movingCredit;

    private int _creditIndex;

    private const string CREDITS_KEY = "credits";

    public override void Init()
    {
        base.Init();
        StartCoroutine(CreditsRoutine());
    }

    private IEnumerator CreditsRoutine()
    {
        _creditIndex = 0;

        while (_creditIndex < LanguageSystem.Instance.GetTranslatedArray(CREDITS_KEY).Length)
        {
            SpawnCredit();
            _creditIndex++;
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private void SpawnCredit()
    {
        Instantiate(_movingCredit, _spawnPoint.position, Quaternion.identity, transform).SetText(LanguageSystem.Instance.GetTranslatedTextFromArrayByID(CREDITS_KEY, _creditIndex));
    }
}