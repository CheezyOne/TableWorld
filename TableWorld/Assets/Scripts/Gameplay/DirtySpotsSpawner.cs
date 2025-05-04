using System.Collections;
using UnityEngine;

public class DirtySpotsSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _initialSpawnTime;
    [SerializeField] private float _decreaseSpawnTime;
    [SerializeField] private float _absoluteMinimumTime;
    [SerializeField] private float _nonPlayerRadius;
    [SerializeField] private int _maxAttempts = 5;
    [SerializeField] private DirtySpot _spot;
    [SerializeField] private Transform _mapCenter;

    private void Awake()
    {
        StartCoroutine(SpawnSpot());
    }

    private IEnumerator SpawnSpot()
    {
        while (true)
        {
            float spawnTime = _initialSpawnTime - GameInfoHolder.Level * _decreaseSpawnTime;

            if (spawnTime < _absoluteMinimumTime)
                spawnTime = _absoluteMinimumTime;

            yield return new WaitForSeconds(spawnTime);
            Vector3 newSpotPosition = FindValidPosition();

            if (newSpotPosition == null)
                continue;

            Instantiate(_spot, newSpotPosition, Quaternion.Euler(new Vector3(90, 0, 0)));
        }
    }

    private Vector3 FindValidPosition()
    {
        int attempts = 0;
        Vector3 newSpotPosition;
        bool positionIsValid;

        do
        {
            newSpotPosition = new Vector3(
                _mapCenter.position.x + Random.Range(-_spawnRadius, _spawnRadius),
                _mapCenter.position.y,
                _mapCenter.position.z + Random.Range(-_spawnRadius, _spawnRadius)
            );
            Collider[] hitColliders = Physics.OverlapSphere(newSpotPosition, _nonPlayerRadius);
            positionIsValid = true;

            foreach (var collider in hitColliders)
            {
                if (collider.GetComponent<PlayerHealth>() != null)
                {
                    positionIsValid = false;
                    break;
                }
            }

            attempts++;
        }
        while (!positionIsValid && attempts < _maxAttempts);

        return newSpotPosition;
    }
}