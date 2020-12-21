using UnityEngine;

namespace ColorSnipersU.MonoBehaviors.MainGameScene
{
    public class DoubleWindowWall : WallBase
    {
        private int NoOfWindowsClosed;

        private enum WindowState
        {
            LeftClosed,
            RightClosed,
            AllOpen,
            AllClosed
        }

        private WindowState currentState;
        //public override bool Closed
        //{
        //    set
        //    {
        //        Closed = value;
        //    }
        //}
        public override void Awake()
        {
            base.Awake();
            randMax = 3;
            NoOfWindowsClosed = 0;
            currentState = WindowState.AllOpen;
        }

        protected override void InitTouchEvents()
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

        private void TouchScript_Tapped()
        {
            ClosingBegan = true;
        }

        private void TouchScript_SwipedRight()
        {
            if (swipeGesture == SwipeGesture.Left && !ColorBallTriggered)
            {
                animator.SetTrigger("Left_Closed");
                if (currentState == WindowState.AllOpen)
                {
                    SoundController.Instance.CloseWindow.Play();
                    currentState = WindowState.LeftClosed;
                }
                else if (currentState == WindowState.RightClosed)
                {
                    currentState = WindowState.AllClosed;
                    Closed = true;
                }
            }
        }

        private void TouchScript_SwipedLeft()
        {
            if (swipeGesture == SwipeGesture.Left && !ColorBallTriggered)
            {
                animator.SetTrigger("Right_Closed");
                if (currentState == WindowState.AllOpen)
                {
                    SoundController.Instance.CloseWindow.Play();
                    currentState = WindowState.RightClosed;
                }
                else if (currentState == WindowState.LeftClosed)
                {
                    currentState = WindowState.AllClosed;
                    Closed = true;
                }
            }
        }

        private void TouchScript_SwipedUp()
        {
            if (swipeGesture == SwipeGesture.Up && !ColorBallTriggered)
            {
                animator.SetTrigger("Left_Closed");
                if (currentState == WindowState.AllOpen)
                {
                    SoundController.Instance.CloseWindow.Play();
                    currentState = WindowState.LeftClosed;
                }
                else if (currentState == WindowState.RightClosed)
                {
                    currentState = WindowState.AllClosed;
                    Closed = true;
                }
            }
        }

        private void TouchScript_SwipedDown()
        {
            if (swipeGesture == SwipeGesture.Up && !ColorBallTriggered)
            {
                animator.SetTrigger("Right_Closed");
                if (currentState == WindowState.AllOpen)
                {
                    SoundController.Instance.CloseWindow.Play();
                    currentState = WindowState.RightClosed;
                }
                else if (currentState == WindowState.LeftClosed)
                {
                    currentState = WindowState.AllClosed;
                    Closed = true;
                }
            }
        }

        public override void SetRotation(int rotationRand)
        {
            float rotation = 0;
            string instruction = string.Empty;

            switch (rotationRand)
            {
                case 1:
                    swipeGesture = SwipeGesture.Left;
                    instruction = "Swipe Left to close the Window";
                    rotation = 0.0f;
                    break;
                case 2:
                    swipeGesture = SwipeGesture.Up;
                    instruction = "Swipe Up to close the Window";
                    rotation = 90.0f;
                    break;

                default:
                    swipeGesture = SwipeGesture.Left;
                    instruction = "Swipe Right to close the Window";
                    rotation = 0.0f;
                    break;
            }
            if (WindowScaler != null)
                WindowScaler.transform.Rotate(Vector3.back, rotation);
            if (TutorialInstruction != null)
            {
                TutorialInstruction = shouldPauseForTrainer ? instruction : string.Empty;
                //TutorialInstruction.rectTransform.anchoredPosition = rotation == 90.0f ? new Vector2(TutorialInstruction.rectTransform.anchoredPosition.x, TutorialInstruction.rectTransform.anchoredPosition.y + 40.0f) : TutorialInstruction.rectTransform.anchoredPosition;
            }

        }

        public override void CheckIfWindowClosed(Vector3 startPosition)
        {
            var leftClosed = (Input.mousePosition.x - startPosition.x > 5.0f && swipeGesture == WallBase.SwipeGesture.Left) || (Input.mousePosition.y - startPosition.y > 5.0f && swipeGesture == WallBase.SwipeGesture.Up);
            var rightClosed = (startPosition.x - Input.mousePosition.x > 5.0f && swipeGesture == WallBase.SwipeGesture.Left) || (startPosition.y - Input.mousePosition.y > 5.0f && swipeGesture == WallBase.SwipeGesture.Up);
            if (leftClosed)
                animator.SetTrigger("Left_Closed");
            else if (rightClosed)
                animator.SetTrigger("Right_Closed");
            if ((leftClosed || rightClosed) && currentState == WindowState.AllOpen)
            {
                if (leftClosed)
                    currentState = WindowState.LeftClosed;
                else if (rightClosed)
                    currentState = WindowState.RightClosed;
            }

            else if ((currentState == WindowState.LeftClosed && rightClosed) || (currentState == WindowState.RightClosed && leftClosed))
            {
                Closed = leftClosed || rightClosed;
                currentState = WindowState.AllClosed;
            }
        }
    }
}
