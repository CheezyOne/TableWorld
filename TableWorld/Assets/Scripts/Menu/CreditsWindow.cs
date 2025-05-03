using System.Collections;
using UnityEngine;
using System;

public class CreditsWindow : BaseWindow
{
    [SerializeField] private string[] _credits;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private MovingCredit _movingCredit;

    private int _creditIndex;

    public override void Init()
    {
        base.Init();
        StartCoroutine(CreditsRoutine());
    }

    private IEnumerator CreditsRoutine()
    {
        _creditIndex = 0;

        while (_creditIndex < _credits.Length)
        {
            SpawnCredit();
            _creditIndex++;
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private void SpawnCredit()
    {
        Instantiate(_movingCredit, _spawnPoint.position, Quaternion.identity, transform).SetText(_credits[_creditIndex]);
    }
}