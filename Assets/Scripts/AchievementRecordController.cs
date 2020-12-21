using ColorSnipersU.ScriptableObjects;
using GooglePlayGames;
using UnityEngine;

public class AchievementRecordController : MonoBehaviour
{

    public static AchievementRecordController Instance;

    public int CloseWindowCount, NearMissCount, MissCount, StarCount;

    public string activeCloseWindowAchievement;

    private PlayGamesAchievement closedWindowAchievement;
    public bool IsSignedIn
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }
    // Use this for initialization
    void Start()
    {
        Instance = this;
        CloseWindowCount = 0;
        NearMissCount = 0;
        MissCount = 0;
        StarCount = 0;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementCloseWindowAchievements()
    {
        CloseWindowCount++;

    }

    public void IncrementMissWindowAchievements()
    {
        MissCount++;

    }

    public void IncrementNearMissAchievements()
    {
        NearMissCount++;

    }

    public void IncrementStarCountAchievement(int Count)
    {
        StarCount = Count;

    }

    public void ReportToGooglePlayService(AchievementSO achievement)
    {
        if (!IsSignedIn)
            return;
        if (achievement != null)
        {
            
            if (achievement.Type == AchievementType.CloseWindow)
            {
                PlayGamesPlatform.Instance.IncrementAchievement(achievement.GPGSID, CloseWindowCount,
                    (bool success) => { Debug.Log("Achievement Status: " + success); }
                    );
            }
            else if (achievement.Type == AchievementType.Miss)
            {
                PlayGamesPlatform.Instance.IncrementAchievement(achievement.GPGSID, MissCount,
                    (bool success) => { Debug.Log("Achievement Status: " + success); }
                    );
            }
            else if (achievement.Type == AchievementType.NearMiss)
            {
                PlayGamesPlatform.Instance.IncrementAchievement(achievement.GPGSID, NearMissCount,
                    (bool success) => { Debug.Log("Achievement Status: " + success); }
                    );
            }
            else if (achievement.Type == AchievementType.ObtainStars)
            {
                double percentageComplete = (StarCount == achievement.Count) ? 100.0f : 0.0f;
                Debug.Log("Achievement percent: " + percentageComplete);
                PlayGamesPlatform.Instance.ReportProgress(achievement.GPGSID, percentageComplete,
                (bool success) => { Debug.Log("Achievement Status: " + success); }
                );
                
            }
            

            
        }
    }

}
