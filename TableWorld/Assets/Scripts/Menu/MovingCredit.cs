using UnityEngine;
using TMPro;
using DG.Tweening;

public class MovingCredit : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _movementTime;
    [SerializeField] private float _movementDistance;

    public void SetText(string text)
    {
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        _text.text = text;
        Vector3 target = new(transform.localPosition.x, transform.localPosition.y +_movementDistance,transform.localPosition.z);
        transform.DOLocalMove(target, _movementTime).OnComplete(()=>Destroy(gameObject));
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}