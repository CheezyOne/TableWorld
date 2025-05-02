using UnityEngine;
using TMPro;

public class PlayerIntentory : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Decoy _decoy;
    [SerializeField] private TMP_Text _decoyAmountText;
    [SerializeField] private int _decoyAmount;
    [SerializeField] private float _decoyThrowForce;

    private void Awake()
    {
        _decoyAmountText.text = _decoyAmount.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ThrowDecoy();
    }

    private void ThrowDecoy()
    {
        if (_decoyAmount <= 0)
            return;

        _decoyAmount--;
        _decoyAmountText.text = _decoyAmount.ToString();
        Decoy newDecoy = Instantiate(_decoy, transform.position + _playerCamera.forward, Quaternion.identity);
        newDecoy.Rigidbody.AddForce((_playerCamera.forward + _playerCamera.up / 2) * _decoyThrowForce, ForceMode.Impulse);
    }

    public void AddDecoy()
    {
        _decoyAmount++;
        _decoyAmountText.text = _decoyAmount.ToString();
    }
}