using UnityEngine;

public class ArmCollider : MonoBehaviour
{
    [SerializeField] private string _playerTag;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag(_playerTag))
           EventBus.OnMenuCupHit?.Invoke();
    }
}