using Assembly_CSharp;
using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour, ISetting
{
    [SerializeField] AudioSource sfxAudioSorce;
    [SerializeField] AudioClip buttonSFX;
    [SerializeField] AudioClip purchaseSFX;
    [SerializeField] AudioClip coinsAddedSFX;
    [SerializeField] Slider volumeSlider;

    Button[] uiButtons;

    public void LoadSetting(SettingsData settingsData)
    {
        volumeSlider.value = settingsData.sfxVolume;
        sfxAudioSorce.volume = settingsData.sfxVolume;
    }

    public void SaveSetting(SettingsData settingsData)
    {
        settingsData.sfxVolume = volumeSlider.value;
    }

    private void Awake()
    {
        uiButtons = Resources.FindObjectsOfTypeAll<Button>();
        if (uiButtons != null)
        {
            foreach (var button in uiButtons)
            {
                button.onClick.AddListener(PlayButtonSound);
            }
        }
    }

    public void ChangeVolume(float value)
    {
        sfxAudioSorce.volume = value;
    }

    private void PlayButtonSound()
    {
        sfxAudioSorce.PlayOneShot(buttonSFX, sfxAudioSorce.volume);
    }
}
