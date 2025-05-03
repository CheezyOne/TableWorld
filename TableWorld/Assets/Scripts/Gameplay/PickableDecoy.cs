using UnityEngine;

public class PickableDecoy : PickableObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerIntentory playerInventory))
        {
            playerInventory.AddDecoy();
            PickedUpEffect();
        }
    }
}