using System;
using UnityEngine;

public class TouchScript : MonoBehaviour
{

    Vector3 startPos;
    Vector3 touchDelta;

    public event Action SwipedLeft;
    public event Action SwipedRight;
    public event Action SwipedUp;
    public event Action SwipedDown;
    public event Action Tapped;
    // Use this for initialization
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        startPos = Input.mousePosition;
        Tapped();
    }

    public void OnMouseUp()
    {
        touchDelta = Input.mousePosition - startPos;
        float requiredMagnitude = 0.5f / GameSettingsManager.Instance.TouchSensitivity;
        if (touchDelta.magnitude > requiredMagnitude)
        {
            if (Math.Abs(touchDelta.x) > Math.Abs(touchDelta.y)) // Horizontal Swipe
            {
                if (touchDelta.x > 0)
                {
                    // Swipe Right
                    Debug.Log("Swipe Right");
                    SwipedRight();
                }
                else
                {
                    // Swipe Left
                    Debug.Log("Swipe Left");
                    SwipedLeft();
                }
            }
            else // Vertical Swipe 
            {
                if (touchDelta.y > 0)
                {
                    // Swipe Up
                    Debug.Log("Swipe Up");
                    SwipedUp();
                }
                else
                {
                    // Swipe Down
                    Debug.Log("Swipe Down");
                    SwipedDown();
                }
            }
        }
        Reset();
    }

    private void Reset()
    {
        startPos = Vector3.zero;
        touchDelta = Vector3.zero;
    }
}
