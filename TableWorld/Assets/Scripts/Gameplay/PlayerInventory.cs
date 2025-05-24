using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Decoy _decoy;
    [SerializeField] private TMP_Text _decoyAmountText;
    [SerializeField] private float _decoyThrowForce;

    private void Awake()
    {
        _decoyAmountText.text = SaveLoadSystem.data.Decoys.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ThrowDecoy();
    }

    private void ThrowDecoy()
    {
        if (SaveLoadSystem.data.Decoys <= 0)
            return;

        SaveLoadSystem.data.Decoys--;
        SaveLoadSystem.Instance.Save();
        _decoyAmountText.text = SaveLoadSystem.data.Decoys.ToString();
        Decoy newDecoy = Instantiate(_decoy, transform.position + _playerCamera.forward, Quaternion.identity);
        newDecoy.Rigidbody.AddForce((_playerCamera.forward + _playerCamera.up / 2) * _decoyThrowForce, ForceMode.Impulse);
        SoundsManager.Instance.PlaySound(SoundType.DecoyThrow);
    }

    public void AddDecoy()
    {
        SaveLoadSystem.data.Decoys++;
        SaveLoadSystem.Instance.Save();
        _decoyAmountText.text = SaveLoadSystem.data.Decoys.ToString();
    }
}