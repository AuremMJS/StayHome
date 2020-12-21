using UnityEngine;
using UnityEngine.UI;

namespace ColorSnipersU.MonoBehaviors.MainGameScene
{
    public class WallBase : MonoBehaviour
    {
        public bool IsNonWindowWall;

        public bool IsTrainerControllable;

        public Text ScoresMeter;

        public int ScorePerClose;

        public Animator animator;

        private bool mClosingBegan;

        private bool mClosed;

        public GameObject WindowScaler;

        public GameObject[] WallTranslator;

        public string TutorialInstruction;

        public bool shouldPauseForTrainer;

        public GameObject[] ColorBalls;

        public GameObject[] PowerUps;

        public GameObject Chest;

        public GameObject HeadTarget;

        public GameObject FluidOverHead;

        protected int randMin;

        protected int randMax;
        private Vector3 startPosition;

        public TouchScript touchScript;

        public bool ReportNearMiss;

        public virtual bool ClosingBegan
        {
            get
            {
                return mClosingBegan;
            }
            set
            {
                if (value == true)
                {
                    if (animator != null)
                        animator.SetTrigger("Touched");
                }
                mClosingBegan = value;
            }
        }

        public virtual bool Closed
        {
            get
            {
                return mClosed;
            }
            set
            {
                if (value == true)
                {
                    if (!mClosed)
                    {
                        MainGameSceneMonoBehavior.Instance.UpdateWindowMeters();
                        if (ReportNearMiss)
                        {
                            NearMissController.Instance.WallNearMissed();
                            //AchievementRecordController.Instance.IncrementNearMissAchievements();
                        }
                        else
                        {
                            //AchievementRecordController.Instance.IncrementCloseWindowAchievements();
;                        }
                    }
                    animator.SetTrigger("Closed");
                    SoundController.Instance.CloseWindow.Play();
                }
                mClosed = value;
            }
        }

        public enum SwipeGesture
        {
            Up,
            Down,
            Right,
            Left
        }

        public enum TutorialGesture
        {
            Tap = 1,
            Up,
            Down,
            Right,
            Left
        }

        public SwipeGesture swipeGesture { get; set; }

        public TutorialGesture tutorialGesture { get; set; }

        public bool ColorBallTriggered { get; set; }

        public bool ConvertColorBall { get; set; }

        public virtual void Awake()
        {
            randMin = 1;
            randMax = 5;
        }
        public virtual void Start()
        {
            ClosingBegan = false;
            Closed = false;
            ColorBallTriggered = false;
            ConvertColorBall = false;
            ReportNearMiss = false;
            InitTouchEvents();
        }

        protected virtual void InitTouchEvents()
        {
            if (touchScript != null)
            {
                touchScript.SwipedDown += TouchScript_SwipedDown;
                touchScript.SwipedUp += TouchScript_SwipedUp;
                touchScript.SwipedLeft += TouchScript_SwipedLeft;
                touchScript.SwipedRight += TouchScript_SwipedRight;
                touchScript.Tapped += TouchScript_Tapped;
            }
        }

        public void TouchScript_Tapped()
        {
            ClosingBegan = true;
        }

        private void TouchScript_SwipedRight()
        {
            if (swipeGesture == SwipeGesture.Right && !ColorBallTriggered)
                Closed = true;
        }

        private void TouchScript_SwipedLeft()
        {
            if (swipeGesture == SwipeGesture.Left && !ColorBallTriggered)
                Closed = true;
        }

        private void TouchScript_SwipedUp()
        {
            if (swipeGesture == SwipeGesture.Up && !ColorBallTriggered)
                Closed = true;
        }

        private void TouchScript_SwipedDown()
        {
            if (swipeGesture == SwipeGesture.Down && !ColorBallTriggered)
                Closed = true;
        }

        public void RandomizeWallRotation()
        {
            int rotationRand = UnityEngine.Random.Range(randMin, randMax);
            SetRotation(rotationRand);

        }

        public void RandomizeWindowPosition()
        {
            if (WallTranslator != null)
            {
                float yPositionRand = UnityEngine.Random.Range(-0.25f, 0.26f);

                foreach (var item in WallTranslator)
                {
                    Vector3 WallPos = item.transform.localPosition;
                    item.transform.localPosition = new Vector3(WallPos.x, yPositionRand, WallPos.z);
                }


            }
        }
        public virtual void SetRotation(int rotationRand)
        {
            float rotation = 0;
            string instruction = string.Empty;

            switch (rotationRand)
            {
                case 1:
                    swipeGesture = SwipeGesture.Left;
                    tutorialGesture = TutorialGesture.Left;
                    instruction = "Swipe Left to close the Window";
                    rotation = 180.0f;
                    break;
                case 2:
                    swipeGesture = SwipeGesture.Up;
                    tutorialGesture = TutorialGesture.Up;
                    instruction = "Swipe Up to close the Window";
                    rotation = 90.0f;
                    break;
                case 3:
                    swipeGesture = SwipeGesture.Down;
                    tutorialGesture = TutorialGesture.Down;
                    instruction = "Swipe Down to close the Window";
                    rotation = 270.0f;
                    break;
                default:
                    swipeGesture = SwipeGesture.Right;
                    tutorialGesture = TutorialGesture.Right;
                    instruction = "Swipe Right to close the Window";
                    rotation = 0.0f;
                    break;
            }
            if (IsNonWindowWall)
            {
                tutorialGesture = TutorialGesture.Tap;
                instruction = "Tap to collect the gem";
            }
            if (WindowScaler != null)
                WindowScaler.transform.Rotate(Vector3.back, rotation);
            if (TutorialInstruction != null)
            {
                TutorialInstruction= shouldPauseForTrainer ? instruction : string.Empty;
                //TutorialInstruction.rectTransform.anchoredPosition = rotation == 90.0f ? new Vector2(TutorialInstruction.rectTransform.anchoredPosition.x, TutorialInstruction.rectTransform.anchoredPosition.y + 40.0f) : TutorialInstruction.rectTransform.anchoredPosition;
            }

        }

        public virtual void CheckIfWindowClosed(Vector3 startPosition)
        {
            Closed = (Input.mousePosition.x - startPosition.x > 5.0f && swipeGesture == WallBase.SwipeGesture.Right) || (startPosition.x - Input.mousePosition.x > 5.0f && swipeGesture == WallBase.SwipeGesture.Left) ||
                   (startPosition.y - Input.mousePosition.y > 5.0f && swipeGesture == WallBase.SwipeGesture.Down) || (Input.mousePosition.y - startPosition.y > 5.0f && swipeGesture == WallBase.SwipeGesture.Up);
        }

        public GameObject GetRandomPowerUp()
        {
            int randIndex = UnityEngine.Random.Range(0, PowerUps.Length);
            return PowerUps[randIndex];
        }

        public GameObject GetRandomColorBall()
        {
            int randomNo = UnityEngine.Random.Range(0, ColorBalls.Length);
            return ColorBalls[randomNo];
        }

    }
}