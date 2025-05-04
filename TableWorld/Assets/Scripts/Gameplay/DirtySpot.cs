using UnityEngine;

public class DirtySpot : MonoBehaviour
{
    [SerializeField] private float _tickDamage;
    [SerializeField] private float _slownessMultiplier;

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(_tickDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementController playerMovementController))
        {
            SoundsManager.Instance.PlaySound(SoundType.DirtySpot);
            playerMovementController.SlowDown(_slownessMultiplier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementController playerMovementController))
        {
            playerMovementController.StopSlowingDown();
        }
    }
}