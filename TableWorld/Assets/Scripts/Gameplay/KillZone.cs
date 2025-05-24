using UnityEngine;

public class KillZone : MonoBehaviour
{
    private const float KILL_ZONE_DAMAGE = 10000f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out PlayerHealth playerHealth))
            playerHealth.TakeDamage(KILL_ZONE_DAMAGE);
    }
}