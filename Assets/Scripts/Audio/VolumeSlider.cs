using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to manage the volumesliders for the games audio
/// </summary>
public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    private enum VolumeType
    {
        MASTER,
        MUSIC,
        SFX,
        AMBIENCE
    }
    [SerializeField] private VolumeType volumeType;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                slider.value = AudioManager.Instance.masterVolume;
                break;
            case VolumeType.MUSIC:
                slider.value = AudioManager.Instance.musicVolume;
                break;
            case VolumeType.SFX:
                slider.value = AudioManager.Instance.sfxVolume;
                break;
            case VolumeType.AMBIENCE:
                slider.value = AudioManager.Instance.ambienceVolume;
                break;
        }
    }

    public void OnValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.Instance.masterVolume = slider.value;
                break;
            case VolumeType.MUSIC:
                AudioManager.Instance.musicVolume = slider.value;
                break;
            case VolumeType.SFX:
                AudioManager.Instance.sfxVolume = slider.value;
                break;
            case VolumeType.AMBIENCE:
                AudioManager.Instance.ambienceVolume = slider.value;
                break;
        }
    }
}
