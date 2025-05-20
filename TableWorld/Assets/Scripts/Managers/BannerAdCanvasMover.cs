using System;
using UnityEngine;

public class BannerAdCanvasMover : MonoBehaviour
{
    [SerializeField] private RootData[] _rootDatas;

    [SerializeField] private Transform _sceneCamera;
    [SerializeField] private float _cameraYOffset;

    private void Start()
    {
        if (AdsManager.Instance.IsShowingBannerAd)
            MoveRootUp();
    }

    private void MoveRootUp()
    {
        if(_sceneCamera != null)
            _sceneCamera.position = new Vector3(_sceneCamera.position.x, _sceneCamera.position.y - _cameraYOffset, _sceneCamera.position.z);

        foreach(RootData rootData in _rootDatas)
        {
            rootData.Root.offsetMin = new Vector2(rootData.Root.offsetMin.x, rootData.RootYOffset);
        }
    }

    private void MoveRootDown()
    {
        if(_sceneCamera!=null)
            _sceneCamera.position = new Vector3(_sceneCamera.position.x, _sceneCamera.position.y + _cameraYOffset, _sceneCamera.position.z);

        foreach (RootData rootData in _rootDatas)
        {
            rootData.Root.offsetMin = new Vector2(rootData.Root.offsetMin.x, rootData.NormalRootYOffset);
        }
    }

    private void OnEnable()
    {
        EventBus.OnBannerAdShown += MoveRootUp;
    }

    private void OnDisable()
    {   
        EventBus.OnBannerAdShown -= MoveRootUp;
    }
}

[Serializable]
public struct RootData
{
    public RectTransform Root;
    public float RootYOffset;
    public float NormalRootYOffset;
}