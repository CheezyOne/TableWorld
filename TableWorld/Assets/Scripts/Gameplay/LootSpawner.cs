using UnityEngine;
using System.Collections;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _initialSpawnTime;
    [SerializeField] private float _increaseSpawnTime;
    [SerializeField] private PickableDrop _drop;
    [SerializeField] private PickableDecoy _decoy;
    [SerializeField] private Transform _mapCenter;

    private void Awake()
    {
        StartCoroutine(SpawnLoot());
    }

    private IEnumerator SpawnLoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(_initialSpawnTime + SaveLoadSystem.data.Level * _increaseSpawnTime);
            Vector3 newLootPosition = new(_mapCenter.position.x + Random.Range(-_spawnRadius, _spawnRadius), _mapCenter.position.y, _mapCenter.position.z + Random.Range(-_spawnRadius, _spawnRadius));

            if (Random.Range(0, 100) >= 50)
            {
                Instantiate(_drop, newLootPosition, Quaternion.identity);
            }
            else
            {
                Instantiate(_decoy, newLootPosition, Quaternion.identity);
            }
        }
    }
}