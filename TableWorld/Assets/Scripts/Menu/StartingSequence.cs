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
    [SerializeField] private NoTutorialWarningWindow _noTutorialWarningWindow;

    private bool _isInAction;

    public bool IsInAction => _isInAction;

    public void OnPlayButton()
    {
        if(!SaveLoadSystem.data.IsTutorialComplete)
        {
            SaveLoadSystem.data.IsTutorialComplete = true;
            SaveLoadSystem.Instance.Save();
            WindowsManager.Instance.OpenWindow(_noTutorialWarningWindow);
            return;
        }

        StartSequence();
    }

    private void StartSequence()
    {
        _isInAction = true;
        _arm.DOMove(_armTarget.position, _armMovementTime).SetEase(_armEase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
            LoadScene();
    }

    private void LoadScene()
    {
        AdsManager.Instance.ShowInter();
        _loadScene.LoadTheScene();
    }

    private void ApplyCupForce()
    {
        _forceComponent.force = _cupForce;
    }

    private void OnEnable()
    {
        EventBus.OnMenuCupHit += ApplyCupForce;
        EventBus.OnTutorialDecline += StartSequence;
    }

    private void OnDisable()
    {
        EventBus.OnMenuCupHit -= ApplyCupForce;
        EventBus.OnTutorialDecline -= StartSequence;
    }
}