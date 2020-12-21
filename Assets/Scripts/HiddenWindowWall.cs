using UnityEngine;


namespace ColorSnipersU.MonoBehaviors.MainGameScene
{
    public class HiddenWindowWall : WallBase
    {
        public GameObject[] TempWalls;
        private bool isWindowHidden;

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
            if (isWindowHidden)
                return;
            ClosingBegan = true;
        }

        private void TouchScript_SwipedRight()
        {
            if (isWindowHidden)
                return;
            if (swipeGesture == SwipeGesture.Right && !ColorBallTriggered)
                Closed = true;
        }

        private void TouchScript_SwipedLeft()
        {
            if (isWindowHidden)
                return;
            if (swipeGesture == SwipeGesture.Left && !ColorBallTriggered)
                Closed = true;
        }

        private void TouchScript_SwipedUp()
        {
            if (isWindowHidden)
                return;
            if (swipeGesture == SwipeGesture.Up && !ColorBallTriggered)
                Closed = true;
        }

        private void TouchScript_SwipedDown()
        {
            if (isWindowHidden)
                return;
            if (swipeGesture == SwipeGesture.Down && !ColorBallTriggered)
                Closed = true;
        }

        public void Start()
        {
            isWindowHidden = true;
            InitTouchEvents(); 
        }
        public void EnableWindow()
        {
            isWindowHidden = false;
        }

        public void HideTempWalls()
        {
            foreach (var item in TempWalls)
            {
                item.SetActive(false);
            }
        }

        public override void CheckIfWindowClosed(Vector3 startPosition)
        {
            if (isWindowHidden)
                return;
            base.CheckIfWindowClosed(startPosition);
        }

    }
}