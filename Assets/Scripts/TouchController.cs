using ColorSnipersU.MonoBehaviors.MainGameScene;
using ColorSnipersU.Utilities.Enumerations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchController : MonoBehaviour
{

    private Vector3 startPosition;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)GameScenes.Levels)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    EvaluateTouchForLevelsScene(Input.GetTouch(i));
                }
            }
        }
        //else if (SceneManager.GetActiveScene().buildIndex == (int)GameScenes.MainGame)
        //{
        //    if (Input.touchCount > 0)
        //    {
        //        for (int i = 0; i < Input.touchCount; i++)
        //        {
        //            EvaluateTouchForMainGame(Input.GetTouch(i));
        //        }
        //    }
        //}
    }

    private void EvaluateTouchForLevelsScene(Touch touch)
    {
        if (GameTicketManager.Instance.PlayGamePopUp.activeSelf || GameErrorDisplayManager.Instance.ErrorDisplaying || GameTicketManager.Instance.LoadingScreen.activeSelf)
            return;
        LevelSceneMonoBehavior levelSceneMonoBehavior = FindObjectOfType<LevelSceneMonoBehavior>();
        if (levelSceneMonoBehavior == null)
            return;
        if (touch.phase == TouchPhase.Began)
        {

            startPosition = new Vector3(touch.position.x, touch.position.y, 10.0f);

        }
        else if (touch.phase == TouchPhase.Moved)
        {

            var rightSwiped = touch.position.x - startPosition.x > 5.0f;
            var leftSwiped = startPosition.x - touch.position.x > 5.0f;
            if (leftSwiped)
            {
                levelSceneMonoBehavior.MoveCameraLeft(touch.position.x - startPosition.x);
            }
            else if (rightSwiped)
            {
                levelSceneMonoBehavior.MoveCameraRight(touch.position.x - startPosition.x);
            }
        }

    }

    public void EvaluateTouchForMainGame(Touch touch)
    {
        Vector3 mPosition = new Vector3(touch.position.x, touch.position.y, 10.0f);
        var rayOrigin = Camera.main.ScreenToWorldPoint(mPosition);
        RaycastHit hitInfo;
        Physics.Raycast(new Vector3(rayOrigin.x, rayOrigin.y, rayOrigin.z), Camera.main.transform.forward, out hitInfo);
        if (hitInfo.collider != null && hitInfo.collider.gameObject.name == "SwipePad")
        {
            if (touch.phase == TouchPhase.Began)
            {
                startPosition = new Vector3(touch.position.x, touch.position.y, 10.0f);
                hitInfo.collider.transform.parent.parent.parent.GetComponent<WallBase>().ClosingBegan = true;

            }
            else if (touch.phase == TouchPhase.Moved)
            {
                WallBase wallScript = hitInfo.collider.transform.parent.parent.parent.GetComponent<WallBase>();
                if (!wallScript.ColorBallTriggered)
                    wallScript.CheckIfWindowClosed(startPosition);
            }

            else if (touch.phase != TouchPhase.Began && touch.phase != TouchPhase.Stationary)
            {
            }
        }

        //if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Began)
        //{
        RaycastHit gemHitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycasted = Physics.Raycast(ray, out gemHitInfo, 100.0f, LayerMask.GetMask("Gem"));
        if (raycasted)
        {
            if (gemHitInfo.collider != null)
            {
                if (gemHitInfo.collider.CompareTag("Gem"))
                {
                    //MainGameSceneMonoBehavior.Instance.AddGemToMeter(5);
                    Destroy(gemHitInfo.collider.gameObject);
                }
            }
        }
        //}
    }
}
