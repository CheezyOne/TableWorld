using UnityEngine;
using System.Collections.Generic;

public class LegsSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _legsPositions;
    [SerializeField] private int _levelsForLegPair;
    [SerializeField] private int _minimumLegsPairs;
    [SerializeField] private List<Vector3> _angles;
    [SerializeField] private Legs _legs;

    private void Awake()
    {
        int legsNumber = 0;

        if (SaveLoadSystem.data.Level > 0)
            legsNumber++;

        legsNumber += _minimumLegsPairs + (SaveLoadSystem.data.Level / _levelsForLegPair);
        legsNumber = Mathf.Clamp(legsNumber, _minimumLegsPairs, _legsPositions.Count);

        for(int i = 0;i<legsNumber;i++)
        {
            int pairNumber = Random.Range(0, _legsPositions.Count);
            Instantiate(_legs, _legsPositions[pairNumber].position, Quaternion.Euler(_angles[pairNumber]));
            _legsPositions.RemoveAt(pairNumber);
            _angles.RemoveAt(pairNumber);
        }
    }
}