using UnityEngine;
using DG.Tweening;
using System.Collections;

public class StartingSequence : MonoBehaviour
{
    [SerializeField] private Transform _arm;
    [SerializeField] private Transform _armTarget;
    [SerializeField] private float _armMovementTime;
    [SerializeField] private Ease _armEase;
    [SerializeField] private LoadScene _loadScene;
    [SerializeField] private float _timeBeforeLoadScene;

    public void OnPlayButton()
    {
        _arm.DOMove(_armTarget.position,_armMovementTime).SetEase(_armEase);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(_timeBeforeLoadScene);
        _loadScene.LoadTheScene();
    }
}