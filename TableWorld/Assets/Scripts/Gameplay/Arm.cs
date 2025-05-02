using UnityEngine;

public class Arm : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() == null)
            return;

        LevelsController.Instance.CompleteLevel();
    }
}