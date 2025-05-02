using UnityEngine;

public class ArmSpawner : MonoBehaviour
{
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _upperBorder;
    [SerializeField] private Transform _lowerBorder;
    [SerializeField] private Arm _arm;

    [SerializeField] private Vector3 _rightBorderAngle;
    [SerializeField] private Vector3 _leftBorderAngle;
    [SerializeField] private Vector3 _upperBorderAngle;
    [SerializeField] private Vector3 _lowerBorderAngle;

    private const float FIFTY_PERCENT = 0.5f;

    public void SpawnArm()
    {
        Vector3 armPosition;
        Vector3 armRotation;

        if (Random.value >= FIFTY_PERCENT) //horizontal
        {
            if (Random.value >= FIFTY_PERCENT)
            {
                armPosition = _rightBorder.position;
                armRotation = _rightBorderAngle;
            }
            else
            {
                armPosition = _leftBorder.position;
                armRotation = _leftBorderAngle;
            }

            float maximumVerticalOffset = (_upperBorder.position.z - _lowerBorder.position.z) / 2;
            Vector3 verticalOffset = new(0, 0, Random.Range(-maximumVerticalOffset, maximumVerticalOffset));
            armPosition += verticalOffset;
        }
        else //vertical
        {
            if (Random.value >= FIFTY_PERCENT)
            {
                armPosition = _upperBorder.position;
                armRotation = _upperBorderAngle;
            }
            else
            {
                armPosition = _lowerBorder.position;
                armRotation = _lowerBorderAngle;
            }

            float maximumHorizontalOffset = (_rightBorder.position.x - _leftBorder.position.x) / 2;
            Vector3 horizontalOffset = new(Random.Range(-maximumHorizontalOffset, maximumHorizontalOffset), 0, 0);
            armPosition += horizontalOffset;
        }

        Instantiate(_arm, armPosition, Quaternion.Euler(armRotation));
    }
}
