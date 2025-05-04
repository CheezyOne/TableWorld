using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButtonSound()
    {
        SoundsManager.Instance.PlaySound(SoundType.Button);
    }
}