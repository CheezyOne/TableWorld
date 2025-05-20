using UnityEngine;
using DG.Tweening;

public class StartingSequence : MonoBehaviour
{
    [SerializeField] private Transform _arm;
    [SerializeField] private Transform _armTarget;
    [SerializeField] private float _armMovementTime;
    [SerializeField] private Ease _armEase;
    [SerializeField] private LoadScene _loadScene;
    [SerializeField] private Vector3 _cupForce;
    [SerializeField] private ConstantForce _forceComponent;
    [SerializeField] private string _playerTag;

    public void OnPlayButton()
    {
        _arm.DOMove(_armTarget.position,_armMovementTime).SetEase(_armEase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
            LoadScene();
    }

    private void LoadScene()
    {
        _loadScene.LoadTheScene();
    }

    private void ApplyCupForce()
    {
        _forceComponent.force = _cupForce;
    }

    private void OnEnable()
    {
        EventBus.OnMenuCupHit += ApplyCupForce;
    }

    private void OnDisable()
    {
        EventBus.OnMenuCupHit -= ApplyCupForce;
    }
}