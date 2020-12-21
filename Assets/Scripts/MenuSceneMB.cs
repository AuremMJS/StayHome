using ColorSnipersU.Utilities;
using ColorSnipersU.Utilities.Enumerations;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using UnityEngine;
using UnityEngine.UI;
//using google.service.game;
public class MenuSceneMB : MonoBehaviour
{
    public GameObject ButtonPanel;
    public GameObject MoreButtonPanel;
    public Text GemText;
    public Text StarText;
    public Sprite SignOutSprite;
    public Sprite SignInSprite;
    public GameObject SignInButton;
    public GameObject SceneCommonInterfaceRef;
    public GameObject DailyBonusMB;
    public Slider TouchSensitivity;
    public Slider MusicVolume;
    public Slider SFXVolume;

    void Awake()
    {
        if (GameTicketManager.Instance == null)
            Instantiate(SceneCommonInterfaceRef);

    }
    // Use this for initialization
    void Start()
    {
        TouchSensitivity.value = GameSettingsManager.Instance.TouchSensitivity;
        MusicVolume.value = GameSettingsManager.Instance.MusicVolume;
        SFXVolume.value = GameSettingsManager.Instance.SFXVolume;

        GameTicketManager.Instance.ShowTicketDisplay(false);
        GameTicketManager.Instance.ShowPlayGamePopUp(false);
        GameStarManager.Instance.GetStarDataFromSavedGames();
        GemText.text = GemScript.Instance.TotalGems.ToString();
        StarText.text = GameStarManager.Instance.TotalStars.ToString();
        try
        {
            // Create client configuration with saved games enabled
            PlayGamesClientConfiguration config = new
               PlayGamesClientConfiguration.Builder()
               .EnableSavedGames()
               .Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }
        catch (Exception ex)
        {

            Debug.Log("Color Snipers U - Exception\n" + ex.Message);
        }
        if (Social.localUser.authenticated)
            SignInButton.GetComponent<Image>().sprite = SignOutSprite;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MoreButtonPanel.activeSelf)
                MoreButtonPanel.SetActive(false);
            else
                GameExitManager.Instance.ShowExitConfirmation();
        }
    }

    public void ShowButtonPanel()
    {
        ButtonPanel.SetActive(true);
        DailyBonusMB.SetActive(true);
    }

    public void ConnectToGooglePlayServices()
    {
        if (!Social.localUser.authenticated)
        {
            try
            {
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        SignInButton.GetComponent<Image>().sprite = SignOutSprite;

                    }
                    else
                    {

                    }
                    return;
                });
            }
            catch (Exception ex)
            {
                Debug.Log("Color Snipers U - exception in sign in \n " + ex.Message);
            }
        }
        else
        {
            LogOut();
        }
    }

    public void ShowAchievements()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            //AchievementButtonText.text = "Connect to Google Play first";
        }
    }

    public void NavigateToLevelsScene()
    {
        HelperFunctions.LoadSceneAsync(GameScenes.Levels, this, new WaitForSeconds(0.0f));
    }

    public void ShowLeaderBoard()
    {
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(); // Show all leaderboard
        //((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(ColorSnipers_U_Resources.leaderboard_star_leaderboard); // Show current (Active) leaderboard
    }

    public void LogOut()
    {
        Debug.Log("Calling PlayGamesPlatform.Instance.SignOut");
        PlayGamesPlatform.Instance.SignOut();
        SignInButton.GetComponent<Image>().sprite = SignInSprite;

    }

    public void NavigateToRateUs()
    {
        Application.OpenURL("market://details?id=com.AuremGameStudio.ColorSnipersU3D");
    }

    public void NavigateToStore()
    {
        HelperFunctions.LoadSceneAsync(GameScenes.Store, this, null);
    }

    public void ShowMoreButtonPanel()
    {
        MoreButtonPanel.SetActive(true);
    }

    public void TouchSensitivityChanged(Slider slider)
    {
        GameSettingsManager.Instance.TouchSensitivity = slider.value;
    }

    public void MusicVolumeChanged(Slider slider)
    {
        GameSettingsManager.Instance.MusicVolume = slider.value;
    }

    public void SFXVolumeChanged(Slider slider)
    {
        GameSettingsManager.Instance.SFXVolume = slider.value;
    }
}
