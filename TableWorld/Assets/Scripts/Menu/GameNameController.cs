using UnityEngine;
using DG.Tweening;

public class GameNameController : MonoBehaviour
{
    [SerializeField] private Transform _gameName;
    [SerializeField] private float _scaleTime;
    [SerializeField] private float _scaleSize;

    private void Awake()
    {
        _gameName.DOScale(_scaleSize, _scaleTime).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        _gameName.DOKill();
    }
}