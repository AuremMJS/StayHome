using System;
using UnityEngine;

public class GemTouchScript : MonoBehaviour
{

    private TouchScript touchScript;
    public event Action GemCollected;
    public Vector3 ItemPosition;
    public int GemValue = 5;
    public String PrefixText = "+";
    private void Awake()
    {
        touchScript = GetComponent<TouchScript>();
        if (touchScript != null)
        {
            touchScript.Tapped += TouchScript_Tapped;
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    private void TouchScript_Tapped()
    {
        MainGameSceneMonoBehavior.Instance.AddGemToMeter(GemValue,PrefixText+GemValue);
        GetComponent<BoxCollider>().enabled = false;
        SoundController.Instance.GemCollecting.Play();
        if (GemCollected != null)
            GemCollected();
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
