using ColorSnipersU.MonoBehaviors.MainGameScene;
using UnityEngine;

public class SliderLockWindowWall : WallBase
{
    protected SwipeGesture LockGesture;
    bool isLocked;
    bool isSlided;

    public override void Awake()
    {
        base.Awake();
        isLocked = false;
        isSlided = false;
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
        EvaluateSwipe(SwipeGesture.Right, SwipeGesture.Left);
    }

    private void TouchScript_SwipedLeft()
    {
        EvaluateSwipe(SwipeGesture.Left, SwipeGesture.Right);
    }

    private void TouchScript_SwipedUp()
    {
        EvaluateSwipe(SwipeGesture.Up, SwipeGesture.Down);
    }

    private void TouchScript_SwipedDown()
    {
        EvaluateSwipe(SwipeGesture.Down, SwipeGesture.Up);
    }

    public override void SetRotation(int rotationRand)
    {
        base.SetRotation(rotationRand);
        switch (rotationRand)
        {
            case 1:
                LockGesture = SwipeGesture.Right;
                break;
            case 2:
                LockGesture = SwipeGesture.Down;
                break;
            case 3:
                LockGesture = SwipeGesture.Up;
                break;
            default:
                LockGesture = SwipeGesture.Left;
                break;
        }

    }

    private void EvaluateSwipe(SwipeGesture currentGesture, SwipeGesture inverseGesture)
    {
        if (swipeGesture == currentGesture)
        {
            if (!isLocked)
                isSlided = true;
            animator.SetTrigger("Slide");
        }
        if (!isLocked && LockGesture == currentGesture)
        {
            isLocked = true;
            animator.SetBool("Lock", true);
        }
        if (isLocked && LockGesture == inverseGesture)
        {
            isLocked = false;
            animator.SetBool("Lock", false);
        }

        Closed = isSlided && isLocked && !ColorBallTriggered;
        if (!Closed)
            SoundController.Instance.CloseWindow.Play();
    }

    public override void CheckIfWindowClosed(Vector3 startPosition)
    {
        var RightSwiped = Input.mousePosition.x - startPosition.x > 3.0f;
        var LeftSwiped = startPosition.x - Input.mousePosition.x > 3.0f;
        var UpSwiped = Input.mousePosition.y - startPosition.y > 3.0f;
        var DownSwiped = startPosition.y - Input.mousePosition.y > 3.0f;
        if (!isLocked)
        {
            if ((RightSwiped && swipeGesture == WallBase.SwipeGesture.Right) || (LeftSwiped && swipeGesture == WallBase.SwipeGesture.Left) ||
                       (DownSwiped && swipeGesture == WallBase.SwipeGesture.Down) || (UpSwiped && swipeGesture == WallBase.SwipeGesture.Up))
            {
                isSlided = true;
                animator.SetTrigger("Slide");
            }
            if ((RightSwiped && LockGesture == WallBase.SwipeGesture.Right) || (LeftSwiped && LockGesture == WallBase.SwipeGesture.Left) ||
                       (DownSwiped && LockGesture == WallBase.SwipeGesture.Down) || (UpSwiped && LockGesture == WallBase.SwipeGesture.Up))
            {
                isLocked = true;
                animator.SetBool("Lock", true);
            }


        }

        else
        {
            if ((RightSwiped && swipeGesture == WallBase.SwipeGesture.Right) || (LeftSwiped && swipeGesture == WallBase.SwipeGesture.Left) ||
                       (DownSwiped && swipeGesture == WallBase.SwipeGesture.Down) || (UpSwiped && swipeGesture == WallBase.SwipeGesture.Up))
            {
                animator.SetTrigger("Slide");
            }
            if ((RightSwiped && LockGesture == WallBase.SwipeGesture.Left) || (LeftSwiped && LockGesture == WallBase.SwipeGesture.Right) ||
                      (DownSwiped && LockGesture == WallBase.SwipeGesture.Up) || (UpSwiped && LockGesture == WallBase.SwipeGesture.Down))
            {
                isLocked = false;
                animator.SetBool("Lock", false);
            }
        }
        Closed = isSlided && isLocked;
    }
}
