using ColorSnipersU.MonoBehaviors.MainGameScene;
using ColorSnipersU.Utilities;
using UnityEngine;

public class GameOverCheckScript : MonoBehaviour
{
    public static GameOverCheckScript Instance;

    public GameObject GameOverPopUp;
    public GameObject WinnerPopUp;
    public GameObject WinnerPopUp2;
    public GameObject ColorBallsParent;
    public MainGameSceneMonoBehavior mainGameSceneMB;
    public bool FadeColorBallsOnShoot;
    public GameObject SplashRef;
    public Transform SplashObjsParent;
    public double EndPositionXOffset = 2.0;
    public BackgroundCollisionScript wallDestroyer;
    public Transform WallTransform;

    private Vector3 StartPosition;
    private Vector3 newPosition;
    private Vector3 TargetPosition;
    private double currentTime;
    // Use this for initialization
    void Start()
    {
        Instance = this;

        FadeColorBallsOnShoot = false;


        StartPosition = WallTransform.localPosition;

        //mainGameSceneMB.TotalTime = Random.Range(2, 7)/2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainGameSceneMB.IsLoadingComplete)
            return;
        double timePassed = Time.time - currentTime;
        double alpha = timePassed / mainGameSceneMB.TotalTime;
        newPosition = Vector3.Lerp(StartPosition, TargetPosition, (float)alpha);
        WallTransform.localPosition = newPosition;

        /*if (newPosition == TargetPosition && currentTime != 0)
        {
            if (StartPosition.x < TargetPosition.x)
            {
                int checkpointRand = UnityEngine.Random.Range(1, 10);
                checkpointRand = checkpointRand > 3 ? 1 : checkpointRand;
                StartPosition = new Vector3(TargetPosition.x - (float) EndPositionXOffset / Mathf.Pow(2.0f,checkpointRand), StartPosition.y, StartPosition.z);
            }
            else
                mainGameSceneMB.TotalTime = Random.Range(2, 7)/2;
                
            Vector3 temp = StartPosition;
            StartPosition = TargetPosition;
            TargetPosition = temp;
            currentTime = Time.time;

            //((mainGameSceneMB.TotalTime / 2.0) >= 0.5) ? mainGameSceneMB.TotalTime / 2 : mainGameSceneMB.TotalTime;

        }*/
    }

    public void SetTargetPositionAndCurrentTime()
    {
        TargetPosition = StartPosition + new Vector3((float)EndPositionXOffset, 0, 0);
        currentTime = Time.time;
    }

    // Collision check to verify whether the player got hurt by the paint ball
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("WallMidPoint"))
        {
            GameObject wallGO = collision.collider.transform.parent.gameObject;
            if (wallGO != null)
            {
                WallBase wallScript = wallGO.GetComponent<WallBase>();
                if (wallScript != null && !wallScript.IsNonWindowWall && !wallScript.Closed && !wallScript.ColorBallTriggered && !wallScript.ConvertColorBall)
                {
                    GameObject ColorBall = wallScript.GetRandomColorBall();
                    ColorBall.SetActive(true);
                    ColorBall.GetComponent<Animator>().SetBool("Fade", FadeColorBallsOnShoot);
                    ColorBall.transform.parent = ColorBallsParent.transform;
                    SoundController.Instance.GunShot.Play();
                    var TargetPosition = Camera.main.transform.position;
                        //new Vector3(4.15f, 5.51f, -3.54f);
                    HelperFunctions.MoveObejectTowardsPosition(this, ColorBall, TargetPosition, 20.0f);
                    wallScript.ColorBallTriggered = true;
                    //AchievementRecordController.Instance.IncrementMissWindowAchievements();
                    if (mainGameSceneMB.CurrentLevel.isLoadStatic)
                    {
                        wallDestroyer.DestroyAllWallsInList();
                    }
                }
            }
        }
        else if (collision.collider.CompareTag("WinnerWall"))
        {
            mainGameSceneMB.SpeedMultiplier = 0;

            EndGame();
        }
    }

    public void EndGame()
    {
        mainGameSceneMB.SpeedMultiplier = 0;
        SoundController.Instance.BGLoop.Stop();
        //AchievementRecordController.Instance.ReportToGooglePlayService(mainGameSceneMB.CurrentLevel.Achievement);
        if (mainGameSceneMB.StarCount > 0)
        {
            GameStarManager.Instance.IncrementStarsToLevel(mainGameSceneMB.CurrentLevel.LevelName, mainGameSceneMB.StarCount);
            if (GameStarManager.Instance.TotalStars > mainGameSceneMB.NextLevel.StarsToUnlock)
            {
                WinnerPopUp.SetActive(true);
                SoundController.Instance.Winner.Play();
            }
            else
            {
                WinnerPopUp2.SetActive(true);
                SoundController.Instance.Winner.Play();
            }
            mainGameSceneMB.ReportScoreToLeaderBoard();
        }
        else
        {
            GameTicketManager.Instance.DecrementTicket();
            GameOverPopUp.SetActive(true);
            SoundController.Instance.GameOver.Play();
        }
        GemScript.Instance.TotalGems = mainGameSceneMB.GetTotalGems();
        Debug.Log("CSU : Total Gems+" + GemScript.Instance.TotalGems);
    }

    public void DisplayPaintSplash(ColorBallTriggerCallback.ColorEnum color)
    {
        GameObject Splash = Instantiate(SplashRef);
        SpriteRenderer spriteRenderer = Splash.GetComponent<SpriteRenderer>();
        Color[] colors = new Color[5]
        {
            new Color(1.0f,1.0f,0.0f,0.65f),
            new Color(0.0f,0.0f,1.0f,0.65f),
            new Color(0.0f,1.0f,0.0f,0.65f),
            new Color(1.0f, 0.435f, 0.0f, 0.65f),
            new Color(1.0f, 0.0f, 0.598f, 0.65f)
        };
        Color colorToUse = colors[(int)color];
        spriteRenderer.color = colorToUse;
        int xr = Random.Range(-1, 1);
        float xsize = Mathf.Pow(2.0f, xr);
        int yr = Random.Range(-1, 1);
        float ysize = Mathf.Pow(2.0f, yr);
        float zrotation = Random.Range(0.0f, 360.0f);
        //Splash.transform.localScale = new Vector3(xsize, ysize, 1.0f);
        Splash.transform.rotation = Quaternion.Euler(0.0f, 0.0f, zrotation);
        Splash.transform.parent = SplashObjsParent;
        float xPos = Random.Range(-5.5f, 5.5f);
        float yPos = Random.Range(-2.0f, 2.0f);
        Splash.transform.localPosition = new Vector3(xPos, yPos, 0.0f);
    }

}
