using UnityEngine;
using DG.Tweening;

public class LevelEndWindow : BaseWindow
{
    [SerializeField] private float _appearTime;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private LoadScene _loadScene;

    private const float MAX_ALHPA = 1f;

    public override void Init()
    {
        base.Init();
        _canvasGroup.DOFade(MAX_ALHPA, _appearTime).OnComplete(()=> _loadScene.LoadTheScene());
    }
}