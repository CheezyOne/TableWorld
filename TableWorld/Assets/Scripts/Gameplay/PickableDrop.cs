using UnityEngine;

public class PickableDrop : PickableObject
{
    [SerializeField] private float _healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.GetHealed(_healAmount);
            SoundsManager.Instance.PlaySound(SoundType.WaterDropPickUp);
            PickedUpEffect();
        }
    }
}