using UnityEngine;

public class SoundController : MonoBehaviour
{

    public static SoundController Instance;

    public AudioSource BGLoop;
    public AudioSource Winner;
    public AudioSource GameOver;
    public AudioSource GunShot;
    public AudioSource Splash;
    public AudioSource GemCollecting;
    public AudioSource CloseWindow;
    public AudioSource Warning;
    public AudioSource IntroBeep1;
    public AudioSource IntroBeep2;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        MusicVolumeChanged();
        SFXVolumeChanged();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MusicVolumeChanged()
    {
        BGLoop.volume = GameSettingsManager.Instance.MusicVolume;
        Winner.volume = GameSettingsManager.Instance.MusicVolume;
        GameOver.volume = GameSettingsManager.Instance.MusicVolume;
    }

    public void SFXVolumeChanged()
    {
        GunShot.volume = GameSettingsManager.Instance.SFXVolume;
        Splash.volume = GameSettingsManager.Instance.SFXVolume;
        GemCollecting.volume = GameSettingsManager.Instance.SFXVolume;
        CloseWindow.volume = GameSettingsManager.Instance.SFXVolume;
        Warning.volume = GameSettingsManager.Instance.SFXVolume;
    }
}
