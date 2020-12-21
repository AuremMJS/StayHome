using ColorSnipersU.MonoBehaviors.MainGameScene;
using ColorSnipersU.ScriptableObjects;
using ColorSnipersU.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSceneMonoBehavior : MonoBehaviour
{
    public static bool shouldUnlockLevel = false;
    private float xPosition;
    public LevelsSO levels;
    public GameObject MainCamera;
    public GameObject RightWall;
    public Text starCounttText;
    public Text GemText;
    public GameObject Soldiers;
    public PlayerController leftSoldierController;
    public PlayerController rightSoldierController;
    public static int LastUnlockedLevel;
    private GameObject[] levelWalls;
    public AudioSource SoldierWalkSound;
    public AudioSource UnlockSound;
    // Use this for initialization
    void Start()
    {
        leftSoldierController.SetArsenal("Rifle");
        rightSoldierController.SetArsenal("Rifle");
        xPosition = -1.0f;
        LastUnlockedLevel = -1;
        levelWalls = new GameObject[levels.Levels.Length];
        for (int i = 0; i < levels.Levels.Length; i++)
        {
            LevelBase currentLevel = levels.Levels[i];
            bool levelUnlocked;
            GenerateWall(currentLevel, i, out levelUnlocked);
            if (levelUnlocked)
                LastUnlockedLevel = i;
            xPosition += 11.0f;
        }
        Vector3 currentPos = RightWall.transform.position;
        RightWall.transform.position = new Vector3(xPosition - 3.35f, currentPos.y, currentPos.z);
        GameTicketManager.Instance.ShowTicketDisplay(true);
        GameTicketManager.Instance.isReplayGame = false;
        Debug.Log("CSU: TotalStars=" + GameStarManager.Instance.GetTotalStars());
        starCounttText.text = GameStarManager.Instance.GetTotalStars().ToString();
        GemText.text = GemScript.Instance.TotalGems.ToString();
        float soldierAndCameraXPosition = LastUnlockedLevel * 11.0f;
        Vector3 soldierPosition = Soldiers.transform.position + new Vector3(soldierAndCameraXPosition, 0.0f, 0.0f);
        Soldiers.transform.position = soldierPosition;
        Vector3 cameraPosition = MainCamera.transform.position + new Vector3(soldierAndCameraXPosition, 0.0f, 0.0f);
        MainCamera.transform.position = cameraPosition;
        if (shouldUnlockLevel)
        {
            UnlockNextLevel();
            shouldUnlockLevel = false;
        }
    }

    private void GenerateWall(LevelBase level, int index, out bool levelUnlocked)
    {
        GameObject WallRef = level.LevelWall;
        GameObject Wall = Instantiate(WallRef);
        Wall.transform.parent = transform;
        Vector3 currentPos = Wall.transform.position;
        Vector3 Position = new Vector3(xPosition, currentPos.y, currentPos.z);
        int StarCount = GameStarManager.Instance.GetStarsForLevel(level.LevelName);
        Wall.transform.position = Position;
        if (index != 0 && !shouldUnlockLevel)
        {
            LevelBase prevLevel = levels.Levels[index - 1];
            int prevLevelStarCount = GameStarManager.Instance.GetStarsForLevel(prevLevel.LevelName);
            if (prevLevelStarCount > 0)
                level.Locked = false;
        }
        bool LevelUnlocked = (GameStarManager.Instance.GetTotalStars() >= level.StarsToUnlock) && !level.Locked;
        levelUnlocked = LevelUnlocked;
        Wall.GetComponent<Animator>().SetBool("Unlock", LevelUnlocked);
        Wall.GetComponent<Animator>().SetInteger("StarCount", StarCount);
        Wall.GetComponent<LevelButtonScript>().LevelNo.text = "Level " + level.LevelName;
        if (level.AdventureTitle != null)
        {
            Wall.GetComponent<LevelButtonScript>().AdventureTitle.gameObject.SetActive(true);
            Wall.GetComponent<LevelButtonScript>().AdventureTitle.sprite = level.AdventureTitle;
        }
        for (int i = 0; i < StarCount; i++)
        {
            Wall.GetComponent<LevelButtonScript>().LitStars[i].SetActive(true);
            Wall.GetComponent<LevelButtonScript>().UnlitStars[i].SetActive(false);
        }
        levelWalls[index] = Wall;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameTicketManager.Instance.PlayGamePopUp.activeSelf && !GameErrorDisplayManager.Instance.ErrorDisplaying)
                HelperFunctions.LoadSceneAsync(ColorSnipersU.Utilities.Enumerations.GameScenes.Menu, this, null);
            else
            {
                if (!GameErrorDisplayManager.Instance.ErrorDisplaying)
                {
                    GameTicketManager.Instance.ShowPlayGamePopUp(false);
                    GameTicketManager.Instance.ShowTicketDisplay(true);
                }
                else
                {
                    GameErrorDisplayManager.Instance.ErrorDisplay.SetActive(false);
                    GameErrorDisplayManager.Instance.ErrorDisplaying = false;
                }
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{

        //    UnlockNextLevel();
        //}
    }

    public void MoveCameraLeft(float xDiff)
    {
        if (MainCamera.transform.position.x < xPosition - 6)
            MainCamera.transform.Translate(3, 0, 0);
    }

    public void MoveCameraRight(float xDiff)
    {
        if (MainCamera.transform.position.x > 5)
            MainCamera.transform.Translate(-3, 0, 0);
    }

    public void UnlockNextLevel()
    {
        StartCoroutine(UnlockNextLevelCoroutine());
    }

    private IEnumerator UnlockNextLevelCoroutine()
    {
        if (levels == null)
            levels = LevelsSO.instance;
        levels.Levels[LastUnlockedLevel + 1].Locked = false;
        float speed = 10.0f;
        Vector3 startPos = MainCamera.transform.position;
        Vector3 destPos = MainCamera.transform.position + new Vector3(11.0f, 0.0f, 0.0f);
        float step = (speed / (startPos - destPos).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step;
            MainCamera.transform.position = Vector3.Lerp(startPos, destPos, t);
            yield return new WaitForFixedUpdate();
        }
        MainCamera.transform.position = destPos;
        LastUnlockedLevel++;
        Animator levelAnimator = levelWalls[LastUnlockedLevel].GetComponent<Animator>();

        UnlockSound.Play(62000);
        levelAnimator.SetBool("Unlock", true);

        var waitTime = levelAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(waitTime);

        startPos = MainCamera.transform.position;
        destPos = MainCamera.transform.position - new Vector3(11.0f, 0.0f, 0.0f);
        step = (speed / (startPos - destPos).magnitude) * Time.fixedDeltaTime;
        t = 0;
        while (t <= 1.0f)
        {
            t += step;
            MainCamera.transform.position = Vector3.Lerp(startPos, destPos, t);
            yield return new WaitForFixedUpdate();
        }
        MainCamera.transform.position = destPos;
        SoldierWalkSound.Play();
        rightSoldierController.gameObject.GetComponent<Animator>().SetTrigger("Right_Walk");

        leftSoldierController.gameObject.GetComponent<Animator>().SetTrigger("Left_Walk");

        speed = 5.0f;
        startPos = MainCamera.transform.position;
        destPos = MainCamera.transform.position + new Vector3(11.0f, 0.0f, 0.0f);
        Vector3 soldierStartPos = Soldiers.transform.position;
        Vector3 soldierDestPos = Soldiers.transform.position + new Vector3(11.0f, 0.0f, 0.0f);
        step = (speed / (startPos - destPos).magnitude) * Time.fixedDeltaTime;
        t = 0;
        while (t <= 1.0f)
        {
            t += step;
            MainCamera.transform.position = Vector3.Lerp(startPos, destPos, t);
            Soldiers.transform.position = Vector3.Lerp(soldierStartPos, soldierDestPos, t);
            yield return new WaitForFixedUpdate();
        }
        MainCamera.transform.position = destPos;

        rightSoldierController.gameObject.GetComponent<Animator>().SetTrigger("Idle");

        leftSoldierController.gameObject.GetComponent<Animator>().SetTrigger("Idle");
        SoldierWalkSound.Stop();
        GameTicketManager.Instance.MenuBGSound.Play(44100 / 2);
        yield return new WaitForSeconds(1.0f);

        GameTicketManager.Instance.SelectedLevel = LastUnlockedLevel + 1;
        GameTicketManager.Instance.ShowPlayGamePopUp(true);

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        //Debug.Log("Scene Loaded " + scene.name + " shouldUnlock " + shouldUnlockLevel);
        ////if (scene.buildIndex == (int)ColorSnipersU.Utilities.Enumerations.GameScenes.Levels && shouldUnlockLevel)
        ////{
        ////    UnlockNextLevel();
        ////    shouldUnlockLevel = false;
        ////}
        ///
        if (GameTicketManager.Instance.isReplayGame)
            GameTicketManager.Instance.ReplayGame();
    }
}
