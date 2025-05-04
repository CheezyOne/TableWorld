using UnityEngine;

public class PickableDecoy : PickableObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventory playerInventory))
        {
            playerInventory.AddDecoy();
            PickedUpEffect();
        }
    }
}