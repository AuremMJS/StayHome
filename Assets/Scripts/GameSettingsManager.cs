using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{

    public static GameSettingsManager Instance;
    public float TouchSensitivity;

    public float MusicVolume;

    public float SFXVolume;

    public void Awake()
    {
        Instance = this;
        TouchSensitivity = 1.0f;
        MusicVolume = 0.75f;
        SFXVolume = 0.75f;
    }
}
