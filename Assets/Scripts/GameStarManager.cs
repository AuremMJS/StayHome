using ColorSnipersU.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

public class GameStarManager : MonoBehaviour
{
    public static GameStarManager Instance;
    public Dictionary<string, int> LevelStarCountDictionary;
    public int TotalStars
    {
        get
        {
            int totalStars = 0;
            foreach (var item in LevelStarCountDictionary)
            {
                totalStars += item.Value;
            }
            return totalStars;
        }
    }
    public LevelsSO LevelsInstance;

    void Awake()
    {
        Instance = this;
        LevelStarCountDictionary = new Dictionary<string, int>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitGame()
    {
        UploadStarDataToSavedGames();
    }

    public void IncrementStarsToLevel(string levelName, int count)
    {
        if (LevelStarCountDictionary.ContainsKey(levelName))
        {
            int existingStars = 0;
            LevelStarCountDictionary.TryGetValue(levelName, out existingStars);
            LevelStarCountDictionary.Remove(levelName);
        }
        LevelStarCountDictionary.Add(levelName, count);
    }

    public void UploadStarDataToSavedGames()
    {
        foreach (var item in LevelStarCountDictionary)
        {
            string Key = "StarCount_" + item.Key;
            PlayerPrefs.SetInt(Key, item.Value);
        }
        PlayerPrefs.Save();
    }

    public void GetStarDataFromSavedGames()
    {
        for (int i = 0; i < LevelsInstance.Levels.Length; i++)
        {
            var levelName = LevelsInstance.Levels[i].LevelName;
            var key = "StarCount_" + levelName;
            if (PlayerPrefs.HasKey(key))
            {
                if (!LevelStarCountDictionary.ContainsKey(levelName))
                {
                    int count = PlayerPrefs.GetInt(key);
                    LevelStarCountDictionary.Add(levelName, count);
                }

            }
        }
    }

    public int GetTotalStars()
    {
        return TotalStars;
    }

    public int GetStarsForLevel(string LevelName)
    {
        int stars = 0;
        if (LevelStarCountDictionary.ContainsKey(LevelName))
            LevelStarCountDictionary.TryGetValue(LevelName, out stars);
        return stars;
    }
}
