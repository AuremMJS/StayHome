using ColorSnipersU.MonoBehaviors.MainGameScene;
using ColorSnipersU.ScriptableObjects;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MainGameSceneMonoBehavior : MonoBehaviour
{

    public static MainGameSceneMonoBehavior Instance;
    public bool IsLoadingComplete;
    public GameObject BackgroundsParent;
    public GameObject WallPrefab;
    public Text GemMeter;
    public Text GemAddText;
    public Text TicketMeter;
    public Text[] PopUpScoreMeters;
    public Text LevelText;
    public Text WindowProgressText;
    public Text ClosedWindowCountText;
    public Slider WindowProgressSlider;
    public Animator WindowStarAnimator;
    public Sprite[] sprites;
    public LevelsSO levelsSO;
    public Text logger;
    public float SpeedMultiplier;

    public GameObject GemRef;
    public GameObject WallRefForStaticLoading;
    public Animator IntroAnimTextAnimator;
    public GameObject IntroMissionText;
    public GameObject[] ItemsToGenerate;
    public int TotalWallsIncludingNonWindowWalls;
    public double TotalTime;
    //public GoogleplayServiceHelper googlePlayServicesHelper;
    private int currentLevel;
    private int TotalGems;
    private int ClosedWindowCount;

    private int starCount;
    public int StarCount
    {
        get
        {
            return starCount;
        }
        set
        {
            starCount = value;
            Debug.Log("No of Stars:" + starCount);
        }
    }
    public LevelBase CurrentLevel
    {
        get
        {
            return levelsSO.Levels[currentLevel - 1];
        }
    }

    public LevelBase NextLevel
    {
        get
        {
            if (currentLevel < levelsSO.Levels.Length)
                return levelsSO.Levels[currentLevel];
            else
                return null; // Current Level must be last Level 
        }
    }

    public float TotalSpeed
    {
        get
        {
            return CurrentLevel.Speed * SpeedMultiplier;
        }
    }

    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        Instance = this;
        currentLevel = PlayerPrefs.GetInt("Level");
        LevelText.text = currentLevel.ToString();
        SpeedMultiplier = 1;
        TotalGems = GemScript.Instance.TotalGems;
        //CurrentLevel.tutoCount = 0;
        CurrentLevel.count = 0;
        ClosedWindowCount = 0;
        TotalWallsIncludingNonWindowWalls = 0;
        ClosedWindowCountText.text = ClosedWindowCount.ToString() + "/" + CurrentLevel.wallLimit.ToString();
        GemMeter.text = TotalGems.ToString();
        TicketMeter.text = GameTicketManager.Instance.NoOfTickets.ToString();
        StarCount = 0;
        int minWallsToClose = (CurrentLevel.wallLimit % 2 == 0) ? CurrentLevel.wallLimit / 2 : CurrentLevel.wallLimit / 2 + 1;
        IntroMissionText.GetComponent<TextMeshProUGUI>().text = "Close " + minWallsToClose + " Windows";
        if (CurrentLevel.isLoadStatic)
        {
            SpeedMultiplier = 0;
            IsLoadingComplete = false;
            StartCoroutine(LoadWallStaticCoroutine());
        }
        else
        {
            SoundController.Instance.BGLoop.Play();
        }
    }

    private IEnumerator LoadWallStaticCoroutine()
    {
        float xPos = WallRefForStaticLoading.transform.position.x;


        GameObject wall;
        do
        {
            wall = GenerateWall(xPos);
            xPos += 10;
            TotalWallsIncludingNonWindowWalls++;
        } while (wall != null && !wall.name.Contains("WinnerWall"));
        yield return new WaitUntil(() => GameTicketManager.Instance.LoadingSlider.value == 100);

        IsLoadingComplete = true;

        GameTicketManager.Instance.LoadingScreen.SetActive(false);
        //StopCoroutine(GameTicketManager.Instance.LoaingScreenCoroutine);
        yield return new WaitForSeconds(0.2f);
        IntroAnimTextAnimator.enabled = true;
        //float currentTime = 0.0f;
        yield return new WaitForSeconds(4.0f);
        //while (currentTime<3.6)
        //{
        //    if (Time.timeScale == 1)
        //        currentTime += 0.25f;
        //    else
        //    {

        //    }
        //    yield return new WaitForSeconds(0.25f);
        //}
        SpeedMultiplier = 1;
        TotalTime = TotalWallsIncludingNonWindowWalls * 10/ TotalSpeed;
        //TotalTime = TotalWallsIncludingNonWindowWalls * (5 / CurrentLevel.WallMovementSpeed) / TotalSpeed;
        GameOverCheckScript.Instance.SetTargetPositionAndCurrentTime();
        SoundController.Instance.BGLoop.Play();
        IntroAnimTextAnimator.enabled = false;
        IntroAnimTextAnimator.gameObject.SetActive(false);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = BackgroundsParent.transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x - 1, currentPosition.y, currentPosition.z);
        BackgroundsParent.transform.position = Vector3.Lerp(currentPosition, newPosition, Time.deltaTime * CurrentLevel.Speed * SpeedMultiplier);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PopUpButtonScript.Instance.OnBackButtonPressed();
        }
    }

    public GameObject InstantitateNextBackground(float prevXPosition, out bool isNoWindowWallGenerated)
    {

        LevelBase l = CurrentLevel;
        GameObject nextWall = Instantiate(WallPrefab);
        nextWall.transform.position = new Vector3(prevXPosition + 5f, 6.2f, 10.8f);
        nextWall.transform.parent = BackgroundsParent.transform;
        GameObject nextBackground = l.InstantiateWall(GemMeter, out isNoWindowWallGenerated);
        nextBackground.transform.position = new Vector3(prevXPosition + 10f, 6.2f, 10.8f);
        nextBackground.transform.parent = BackgroundsParent.transform;
        return nextBackground;
    }

    public GameObject GenerateWall(float xPosition)
    {
        bool isNoWindowWallGenerated = true;
        GameObject nextWall = InstantitateNextBackground(xPosition, out isNoWindowWallGenerated);
        if (isNoWindowWallGenerated)
        {
            int ItemToGenIndex = nextWall.GetComponent<WallBase>().shouldPauseForTrainer ? 0 : Random.Range(0, ItemsToGenerate.Length);
            GameObject item = Instantiate(ItemsToGenerate[ItemToGenIndex]);
            var xPos = 0.0f;
            var yPos = Random.Range(-0.3f, 0.4f);
            if (nextWall.GetComponent<WallBase>().shouldPauseForTrainer)
            {
                xPos = 0.05f;
                yPos = 0.07f;
                item.GetComponent<GemTouchScript>().GemCollected += nextWall.GetComponent<WallBase>().TouchScript_Tapped;
            }

            item.transform.SetParent(nextWall.transform);
            if (ItemToGenIndex != 0)
            {
                //item.transform.localPosition = new Vector3(0, 0, 0);
                item.transform.localPosition = item.GetComponent<GemTouchScript>().ItemPosition;
            }
            else
                item.transform.localPosition = new Vector3(xPos, yPos, 0);
            item.transform.SetParent(nextWall.transform);
        }
        return nextWall;
    }

    public void ReportScore()
    {
        //googlePlayServicesHelper.ReportScore(100,logger);
    }

    public void AddGemToMeter()
    {
        AddGemToMeter(5);
    }

    public void AddGemToMeter(int noOfGems, string GemAddText)
    {
        TotalGems += noOfGems;
        //GemMeter.text = TotalGems.ToString();
        AnimateGemAddText(GemAddText);
        foreach (var item in PopUpScoreMeters)
        {
            item.text = TotalGems.ToString();
        }
    }

    public void AddGemToMeter(int noOfGems)
    {
        AddGemToMeter(noOfGems, "+" + noOfGems);
    }


    public void AnimateGemAddText(string text)
    {
        GemAddText.text = text;
        StartCoroutine(AnimateGemAddTextCoroutine());
    }

    private IEnumerator AnimateGemAddTextCoroutine()
    {
        GemAddText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GemMeter.text = TotalGems.ToString();
        GemAddText.gameObject.SetActive(false);
        GemAddText.text = string.Empty;
    }


    public void ReportScoreToLeaderBoard()
    {

        //if (!Social.localUser.authenticated)
        //{
        //    PlayGamesPlatform.Activate();
        //    PlayGamesPlatform.DebugLogEnabled = true;
        //    if (!Social.localUser.authenticated)
        //    {
        //        Social.localUser.Authenticate((bool success) =>
        //        {
        //            if (success)
        //            {

        //            }
        //            else
        //            {

        //            }
        //        });
        //    }
        //}
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(GameStarManager.Instance.TotalStars, ColorSnipers_U_Resources.leaderboard_star_leaderboard, (bool success) =>
            {
                Debug.Log("Report Star Leaderboard: " + success);
            });

            Social.ReportScore(TotalGems, ColorSnipers_U_Resources.leaderboard_gem_leaderboard, (bool success) =>
            {
                Debug.Log("Report Gem Leaderboard: " + success);
            });
        }
    }

    public void ClearAllFlowingParticleSystems()
    {
        var flowingParticleSystems = GameObject.FindObjectsOfType<FlowingColorParticleController>();
        foreach (var item in flowingParticleSystems)
        {
            var GO = (item as FlowingColorParticleController).gameObject;
            Destroy(GO);
        }
    }

    public void FadeColorBallsOnShoot()
    {
        GameOverCheckScript.Instance.FadeColorBallsOnShoot = true;
    }

    public void DisableFadeColorBallsOnShoot()
    {
        GameOverCheckScript.Instance.FadeColorBallsOnShoot = false;
    }

    public void UpdateWindowMeters()
    {
        ClosedWindowCount++;
        ClosedWindowCountText.text = ClosedWindowCount.ToString() + "/" + CurrentLevel.wallLimit.ToString();
        float percentageWindowsClosed = (float)System.Math.Round((ClosedWindowCount * 100.0f) / CurrentLevel.wallLimit, 2);
        WindowProgressSlider.value = percentageWindowsClosed;
        WindowProgressText.text = percentageWindowsClosed + "%";
        if (percentageWindowsClosed == 100.0f)
            StarCount = 3;
        else if (percentageWindowsClosed >= 80.0f)
            StarCount = 2;
        else if (percentageWindowsClosed >= 50.0f)
            StarCount = 1;
        else
            StarCount = 0;
        WindowStarAnimator.SetInteger("StarCount", StarCount);
        //AchievementRecordController.Instance.IncrementStarCountAchievement(StarCount);
    }

    public int GetTotalGems()
    {
        return TotalGems;
    }


}
