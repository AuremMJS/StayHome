using ColorSnipersU.MonoBehaviors.MainGameScene;
using ColorSnipersU.ScriptableObjects;
using ColorSnipersU.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameTicketManager : MonoBehaviour
{
    private int noOfTickets;
    public int NoOfTickets
    {
        get
        {
            return noOfTickets;
        }
        set
        {
            if (value < 0)
                value = 0;
            if (value > 5)
                value = 5;
            noOfTickets = value;
        }
    }

    private int TimeToIncrement;
    private DateTime TimeWhenTicketUpdated;
    public bool isReplayGame;
    public static GameTicketManager Instance;
    public AudioSource MenuBGSound;
    public int SelectedLevel { get; set; }
    public Text[] TicketTexts;
    public Text[] TimerTexts;
    public Text LevelText;
    public Text MissionText;
    public GameObject TicketDisplay;
    public GameObject PlayGamePopUp;
    public GameObject WatchAdPopUp;
    public GameObject LoadingScreen;
    public Slider LoadingSlider;

    private IEnumerator TimerCoroutineInstance;

    [Serializable]
    public struct StoreObjectIconMap
    {
        public StoreObjectPurchaseController.StoreObject storeObject;
        public GameObject Icon;
    }

    public StoreObjectIconMap[] storeObjectIconMaps;
    public Sprite[] LoadingScreenTips;
    public Image LoadingScreenTipRef;
    public Coroutine LoadingScreenCoroutine;
    public bool IsPlayingLastUnlockedLevel;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {
        TimeToIncrement = 20;
        int noOfTickets = 0;
        isReplayGame = false;
        if (PlayerPrefs.HasKey("NoOfTickets"))
        {
            noOfTickets = PlayerPrefs.GetInt("NoOfTickets");
        }
        else
        {
            noOfTickets = 5;
            PlayerPrefs.SetInt("NoOfTickets", noOfTickets);
            PlayerPrefs.Save();
        }

        Debug.Log("No of Tickets=" + noOfTickets);
        if (noOfTickets < 5)
        {
            if (PlayerPrefs.HasKey("TicketDecrementedTime"))
            {
                DateTime TicketDecrementedTime = DateTime.Now;
                DateTime.TryParse(PlayerPrefs.GetString("TicketDecrementedTime"), out TicketDecrementedTime);
                if (TicketDecrementedTime != DateTime.Now)
                {
                    TimeSpan TimeDiff = DateTime.Now - TicketDecrementedTime;
                    Debug.Log("TimeDiff:" + TimeDiff.Minutes + ": " + TimeDiff.Seconds);
                    while (TimeDiff.TotalSeconds > TimeToIncrement * 60 && noOfTickets < 5)
                    {
                        noOfTickets++;
                        TimeDiff = TimeDiff.Subtract(TimeSpan.FromMinutes(TimeToIncrement));
                    }
                    Debug.Log("TimeDiff:" + TimeDiff.Minutes + ": " + TimeDiff.Seconds);
                    if (noOfTickets < 5)
                    {
                        TimeSpan TimerTime = TimeSpan.FromMinutes(TimeToIncrement) - TimeDiff;
                        if (TimerCoroutineInstance != null)
                            StopCoroutine(TimerCoroutineInstance);
                        TimerCoroutineInstance = TimerCoroutine(TimerTime.Minutes, TimerTime.Seconds);
                        StartCoroutine(TimerCoroutineInstance);
                    }
                }
            }
        }
        NoOfTickets = noOfTickets;
        UpdateTicketText(NoOfTickets.ToString());
        if (noOfTickets == 5)
            UpdateTimerText("Full");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && WatchAdPopUp.activeSelf)
        {
            WatchAdPopUp.SetActive(false);
        }
    }

    public void UpdateTicketText(string text)
    {
        foreach (var item in TicketTexts)
        {
            item.text = text;
        }
    }

    public void UpdateTimerText(string text)
    {
        foreach (var item in TimerTexts)
        {
            item.text = text;
        }
    }

    public void DecrementTicket()
    {
        if (NoOfTickets == 5)
        {
            PlayerPrefs.SetString("TicketDecrementedTime", DateTime.Now.ToString());
            PlayerPrefs.Save();
            if (TimerCoroutineInstance != null)
                StopCoroutine(TimerCoroutineInstance);
            TimerCoroutineInstance = TimerCoroutine(TimeToIncrement, 0);
            StartCoroutine(TimerCoroutineInstance);
        }
        NoOfTickets--;
        UpdateTicketText(NoOfTickets.ToString());
    }
    public void ExitGame()
    {
        PlayerPrefs.SetInt("NoOfTickets", NoOfTickets);
        PlayerPrefs.Save();
    }

    private IEnumerator TimerCoroutine(int startMinutes, int startSeconds)
    {
        WaitForSeconds oneSecondYield = new WaitForSeconds(1.0f);
        int totalSeconds = startMinutes * 60 + startSeconds;
        int currentSeconds = 0;
        while (currentSeconds < totalSeconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(totalSeconds - currentSeconds);
            var minutes = (t.Minutes > 9) ? t.Minutes.ToString() : "0" + t.Minutes.ToString();
            var seconds = (t.Seconds > 9) ? t.Seconds.ToString() : "0" + t.Seconds.ToString();
            var text = minutes + ":" + seconds;
            UpdateTimerText(text);
            currentSeconds++;
            yield return oneSecondYield;
        }
        IncrementTickets();
    }

    public void IncrementTickets()
    {
        if (TimerCoroutineInstance != null)
            StopCoroutine(TimerCoroutineInstance);
        if ((NoOfTickets + 1) < 5)
        {
            TimerCoroutineInstance = TimerCoroutine(TimeToIncrement, 0);
            StartCoroutine(TimerCoroutineInstance);
        }
        else
            UpdateTimerText("Full");
        NoOfTickets++;
        UpdateTicketText(NoOfTickets.ToString());
    }

    public void ShowTicketDisplay(bool show)
    {
        if (isReplayGame && show)
            return;
        TicketDisplay.SetActive(show);
    }

    public void ShowPlayGamePopUp(bool show)
    {
        LevelText.text = "LEVEL " + SelectedLevel.ToString();
        if (show)
        {
            LevelBase level = LevelsSO.instance.Levels[SelectedLevel - 1];
            int minWallsToClose = (level.wallLimit % 2 == 0) ? level.wallLimit / 2 : level.wallLimit / 2 + 1;
            MissionText.text = "Mission: Close " + minWallsToClose + " Windows";
            foreach (var storeObject in storeObjectIconMaps)
            {
                if (StoreObjectPurchaseController.Instance.GetNoOfStoreObjectPurchased(storeObject.storeObject) > 0)
                {
                    storeObject.Icon.SetActive(true);
                }
            }

            ShowTicketDisplay(false);
        }
        PlayGamePopUp.SetActive(show);

    }

    public void ShowStoreObjectIcons()
    {
        foreach (var storeObject in storeObjectIconMaps)
        {
            if (StoreObjectPurchaseController.Instance.GetNoOfStoreObjectPurchased(storeObject.storeObject) > 0)
            {
                storeObject.Icon.SetActive(true);
            }
        }
    }

    public void OnPlayButtonClick()
    {
        if (noOfTickets > 0)
        {
            PlayerPrefs.SetInt("Level", SelectedLevel);
            PlayerPrefs.Save();
            LoadingSlider.value = 0;
            LoadingScreenCoroutine = StartCoroutine(DisplayLoadingScreenCoroutine());
            IsPlayingLastUnlockedLevel = ((LevelSceneMonoBehavior.LastUnlockedLevel + 1) == SelectedLevel);
            ShowPlayGamePopUp(false);
            ShowTicketDisplay(false);
            MenuBGSound.Stop();
            HelperFunctions.LoadSceneAsync(ColorSnipersU.Utilities.Enumerations.GameScenes.MainGame, this, null);
        }

        else
        {
            WatchAdPopUp.SetActive(true);
        }
    }

    private IEnumerator DisplayLoadingScreenCoroutine()
    {
        LoadingScreen.SetActive(true);
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
        LoadingSlider.value = 0;
        int loadingScreenTipIndex = 0;
        LoadingScreenTipRef.sprite = LoadingScreenTips[loadingScreenTipIndex];
        while (LoadingSlider.value != 100)
        {
            LoadingSlider.value += 1.66667f;
            if (LoadingSlider.value < (loadingScreenTipIndex + 1) * 33 + 1 && LoadingSlider.value >= (loadingScreenTipIndex + 1) * 33)
            {
                loadingScreenTipIndex++;
                if (loadingScreenTipIndex > 1)
                {
                    int rand = UnityEngine.Random.Range(0, LoadingScreenTips.Length - 2);
                    loadingScreenTipIndex += rand;
                }
                LoadingScreenTipRef.sprite = LoadingScreenTips[loadingScreenTipIndex];
            }
            yield return waitForSeconds;
        }
        yield return new WaitUntil(() => MainGameSceneMonoBehavior.Instance != null && MainGameSceneMonoBehavior.Instance.IsLoadingComplete);

    }

    public void ReplayGame()
    {
        StopCoroutine(LoadingScreenCoroutine);
        LoadingSlider.value = 0;
        //isReplayGame = true;
        ShowPlayGamePopUp(true);
        ShowTicketDisplay(false);
    }

    public void OnWatchAdExitButtonClick()
    {
        WatchAdPopUp.SetActive(false);
    }

}
