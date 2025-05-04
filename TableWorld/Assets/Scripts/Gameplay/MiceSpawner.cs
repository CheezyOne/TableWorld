using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MiceSpawner : MonoBehaviour
{
    [SerializeField] private float _minTrainMiceTime;
    [SerializeField] private float _maxTrainMiceTime;
    [SerializeField] private float _trainMiceTimeReduction;
    [SerializeField] private float _absoluteLowestTrainMiceTime;
    [SerializeField] private BorderPair[] _borderPairs;

    [SerializeField] private Transform _player;
    [SerializeField] private TrainMouse _trainMouse;
    [SerializeField] private NormalMouse _normalMouse;
    [SerializeField] private int _levelsForNormalMouse;

    private void Awake()
    {
        StartCoroutine(TrainMiceSpawnRoutine());
        SpawnNormalMice();
    }

    private void SpawnNormalMice()
    {
        int miceAmount = 1 + (GameInfoHolder.Level / _levelsForNormalMouse);

        for(int i = 0;i<miceAmount;i++)
        {
            int borderPairIndex = Random.Range(0, _borderPairs.Length);
            Instantiate(_normalMouse, _borderPairs[borderPairIndex].GetRandomPoint(), Quaternion.Euler(_borderPairs[borderPairIndex].Rotation)).SetPlayer(_player);
        }
    }

    private IEnumerator TrainMiceSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetBetweenMiceTime());
            int borderPairIndex = Random.Range(0, _borderPairs.Length);
            Instantiate(_trainMouse, _borderPairs[borderPairIndex].GetRandomPoint(), Quaternion.Euler(_borderPairs[borderPairIndex].Rotation));
        }
    }


    private float GetBetweenMiceTime()
    {
        float reductedTime = _maxTrainMiceTime - _trainMiceTimeReduction * GameInfoHolder.Level;
        float betweenStompsTime = reductedTime < _minTrainMiceTime ? Random.Range(_absoluteLowestTrainMiceTime, _minTrainMiceTime) : Random.Range(_minTrainMiceTime, reductedTime);
        return betweenStompsTime;
    }
}

[Serializable]
public class BorderPair
{
    [SerializeField] private Transform _firstBorder;
    [SerializeField] private Transform _secondBorder;

    public Vector3 Rotation;

    public Vector3 GetRandomPoint()
    {
        Vector3 posA = _firstBorder.position;
        Vector3 posB = _secondBorder.position;
        float minX = Mathf.Min(posA.x, posB.x);
        float maxX = Mathf.Max(posA.x, posB.x);
        float minZ = Mathf.Min(posA.z, posB.z);
        float maxZ = Mathf.Max(posA.z, posB.z);
        float y = posA.y;
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        return new Vector3(randomX, y, randomZ);
    }
}