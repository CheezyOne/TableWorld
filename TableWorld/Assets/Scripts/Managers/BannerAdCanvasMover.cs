using System;
using UnityEngine;

public class BannerAdCanvasMover : MonoBehaviour
{
    [SerializeField] private RootData[] _rootDatas;

    [SerializeField] private CameraController _sceneCamera;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private Vector3 _cameraNormalOffset;  

    private void Start()
    {
        if (AdsManager.Instance.IsShowingBannerAd)
            MoveRootUp();
    }

    private void MoveRootUp()
    {
        if (_sceneCamera != null)
            _sceneCamera.SetOffset(_cameraOffset);

        foreach (RootData rootData in _rootDatas)
        {
            rootData.Root.offsetMin = new Vector2(rootData.Root.offsetMin.x, rootData.RootYOffset);
        }
    }

    private void MoveRootDown()
    {
        if(_sceneCamera!=null)
            _sceneCamera.SetOffset(_cameraNormalOffset);

        foreach (RootData rootData in _rootDatas)
        {
            rootData.Root.offsetMin = new Vector2(rootData.Root.offsetMin.x, rootData.NormalRootYOffset);
        }
    }

    private void OnEnable()
    {
        EventBus.OnBannerAdShown += MoveRootUp;
        EventBus.OnBannerAdHidden += MoveRootDown;
    }

    private void OnDisable()
    {   
        EventBus.OnBannerAdShown -= MoveRootUp;
        EventBus.OnBannerAdHidden -= MoveRootDown;
    }
}

[Serializable]
public struct RootData
{
    public RectTransform Root;
    public float RootYOffset;
    public float NormalRootYOffset;
}