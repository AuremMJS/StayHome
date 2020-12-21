using ColorSnipersU.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class PopUpButtonScript : MonoBehaviour
{

    public static PopUpButtonScript Instance;
    public GameObject PausePopUp;
    public Slider TouchSensitivity;
    public Slider MusicVolume;
    public Slider SFXVolume;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        TouchSensitivity.value = GameSettingsManager.Instance.TouchSensitivity;
        MusicVolume.value = GameSettingsManager.Instance.MusicVolume;
        SFXVolume.value = GameSettingsManager.Instance.SFXVolume;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBackButtonPressed()
    {
        if (PausePopUp.activeSelf)
            PlayButtonCallback();
        else if (GameOverCheckScript.Instance.WinnerPopUp.activeSelf || GameOverCheckScript.Instance.WinnerPopUp2.activeSelf || GameOverCheckScript.Instance.GameOverPopUp.activeSelf)
            ReturnToMainMenuCallback();
        else if(!GameTicketManager.Instance.LoadingScreen.activeSelf)
            PauseButtonCallback();
    }

    public void ReturnToMainMenuCallback()
    {
        HelperFunctions.LoadSceneAsync(ColorSnipersU.Utilities.Enumerations.GameScenes.Levels, this, new WaitForSeconds(0.0f));
        GameTicketManager.Instance.MenuBGSound.Play(44100/2);
        Time.timeScale = 1;
    }

    public void PauseButtonCallback()
    {
        PausePopUp.SetActive(true);
        MainGameSceneMonoBehavior.Instance.SpeedMultiplier = 0;
        Time.timeScale = 0;

    }

    public void PlayButtonCallback()
    {
        PausePopUp.SetActive(false);
        if (!MainGameSceneMonoBehavior.Instance.IntroAnimTextAnimator.enabled)
            MainGameSceneMonoBehavior.Instance.SpeedMultiplier = 1;
        Time.timeScale = 1;
    }

    public void ReplayButtonCallback()
    {
        HelperFunctions.LoadSceneAsync(ColorSnipersU.Utilities.Enumerations.GameScenes.Levels, this, null);
        GameTicketManager.Instance.isReplayGame = true;
        GameTicketManager.Instance.MenuBGSound.Play(44100/2);
        Time.timeScale = 1;
        //HelperFunctions.LoadScene(ColorSnipersU.Utilities.Enumerations.GameScenes.Levels);
        //GameTicketManager.Instance.ReplayGame();
    }

    public void ShareButtonCallback()
    {
        HelperFunctions.ShareScreenshot();
    }

    public void PlayNextButtonCallback()
    {
        if (GameTicketManager.Instance.IsPlayingLastUnlockedLevel)
        {
            LevelSceneMonoBehavior.shouldUnlockLevel = true;
        }

        HelperFunctions.LoadSceneAsync(ColorSnipersU.Utilities.Enumerations.GameScenes.Levels, this, null);
    }

    public void TouchSensitivityChanged(Slider slider)
    {
        GameSettingsManager.Instance.TouchSensitivity = slider.value;
    }

    public void MusicVolumeChanged(Slider slider)
    {
        GameSettingsManager.Instance.MusicVolume = slider.value;
        SoundController.Instance.MusicVolumeChanged();
    }

    public void SFXVolumeChanged(Slider slider)
    {
        GameSettingsManager.Instance.SFXVolume = slider.value;
        SoundController.Instance.SFXVolumeChanged();
    }

}
