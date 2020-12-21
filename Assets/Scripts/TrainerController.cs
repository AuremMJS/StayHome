using ColorSnipersU.MonoBehaviors.MainGameScene;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrainerController : MonoBehaviour
{

    public MainGameSceneMonoBehavior mainGameSceneMB;
    public bool TrainerOn { get; set; }
    public bool TrainerSkipped { get; private set; }

    public Text Instruction;
    public GameObject SkipButton;
    public Animator TutorialAnimator;

    public static TrainerController Instance;

    private Action SwitchOffCallback;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //Instance = this;
        Instance.TrainerOn = false;
        Instance.TrainerSkipped = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("WallMidPoint"))
        {
            WallBase wallScript = collision.collider.transform.parent.GetComponent<WallBase>();
            //if (Instance.TrainerOn)
            if (wallScript != null && wallScript.shouldPauseForTrainer)
            {
                mainGameSceneMB.SpeedMultiplier = 0;
                //TutorialUI.SetActive(true);
                TrainerSkipped = false;
                TutorialAnimator.SetInteger("Gesture", (int)wallScript.tutorialGesture);
                Instruction.text = wallScript.TutorialInstruction;
                float startTime = Time.time;
                SkipButton.SetActive(true);
                StartCoroutine(ResumeAfterFiveSeconds(wallScript, startTime));
            }
        }
    }

    public void SwitchTrainerOn(bool switchVal, Action switchOffCallback = null)
    {
        TrainerOn = switchVal;
        if (switchVal)
        {
            SwitchOffCallback = switchOffCallback;
        }


    }

    private IEnumerator ResumeAfterFiveSeconds(WallBase wallScript, float startTime)
    {
        while (((wallScript.IsNonWindowWall && !wallScript.ClosingBegan) || (!wallScript.IsNonWindowWall && !wallScript.Closed)) && !TrainerSkipped)
        {
            if (Time.time - startTime > 5.0f)
                break;
            yield return new WaitForEndOfFrame();
        }

        //yield return new WaitForSeconds(5.0f);
        mainGameSceneMB.SpeedMultiplier = 1;
        Instruction.text = string.Empty;
        TutorialAnimator.SetInteger("Gesture", 0);
        SkipButton.SetActive(false);
        SwitchTrainerOn(false);
        if (SwitchOffCallback != null)
            SwitchOffCallback();
        yield return new WaitForSeconds(1.0f);
    }

    public void SkipTutorial()
    {
        TrainerSkipped = true;
        SkipButton.SetActive(false);
    }

}
