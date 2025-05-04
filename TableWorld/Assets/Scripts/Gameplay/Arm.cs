using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] private string _playerTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
            LevelsController.Instance.CompleteLevel();
    }
}