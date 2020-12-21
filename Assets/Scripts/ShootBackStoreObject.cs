using ColorSnipersU.MonoBehaviors.MainGameScene;
using ColorSnipersU.Utilities;
using System.Collections;
using UnityEngine;

public class ShootBackStoreObject : StoreObject
{

    public GameObject Gun;

    public GameObject Ball;

    private Vector3 BallOriginalPosition;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        BallOriginalPosition = Ball.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("WallMidPoint"))
        {
            GameObject wallGO = collider.transform.parent.gameObject;
            if (wallGO != null)
            {
                WallBase wallScript = wallGO.GetComponent<WallBase>();
                if (wallScript != null && !wallScript.IsNonWindowWall)
                {
                    StartCoroutine(ShootBackCoroutine(wallScript));
                }
            }
        }
    }

    private IEnumerator ShootBackCoroutine(WallBase wall)
    {
        this.GetComponent<Animator>().SetBool("TriggerBall", true);
        SoundController.Instance.GunShot.Play();
        MainGameSceneMonoBehavior.Instance.SpeedMultiplier = 0;
        Ball.SetActive(true);
        wall.HeadTarget.SetActive(true);
        var TargetPosition = wall.HeadTarget.transform.position;
        HelperFunctions.MoveObejectTowardsPosition(this, Ball, TargetPosition, 20.0f);
        yield return new WaitForSeconds(1.0f);
        wall.FluidOverHead.SetActive(true);
        wall.HeadTarget.SetActive(false);
        SoundController.Instance.Splash.Play();
        MainGameSceneMonoBehavior.Instance.SpeedMultiplier = 1;
        RemoveStoreObjectFromGamePlay();
        wall.ColorBallTriggered = true;
    }

    public override void ApplyStoreObjectToGamePlay(StoreObjectsController storeObjectsController)
    {
        base.ApplyStoreObjectToGamePlay(storeObjectsController);
        this.GetComponent<BoxCollider>().enabled = true;
        Gun.SetActive(true);
    }

    public override void RemoveStoreObjectFromGamePlay()
    {
        base.RemoveStoreObjectFromGamePlay();
        this.GetComponent<Animator>().SetBool("TriggerBall", false);
        this.GetComponent<BoxCollider>().enabled = false;
        Gun.SetActive(false);
        Ball.SetActive(false);
        Ball.transform.position = BallOriginalPosition;
    }
}
