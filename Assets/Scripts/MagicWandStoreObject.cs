using ColorSnipersU.MonoBehaviors.MainGameScene;
using ColorSnipersU.Utilities;
using System.Collections;
using UnityEngine;

public class MagicWandStoreObject : StoreObject
{
    public GameObject MagicWand;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
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
                    StartCoroutine(MagicWandCoroutine(wallScript));
                }
            }
        }
    }

    private IEnumerator MagicWandCoroutine(WallBase wall)
    {
        MainGameSceneMonoBehavior.Instance.SpeedMultiplier = 0;
        this.GetComponent<Animator>().SetBool("TriggerMagic", true);
        yield return new WaitForSeconds(0.5f);
        GameObject powerUp = wall.GetRandomPowerUp();
        powerUp.SetActive(true);
        wall.Chest.SetActive(true);
        var TargetPosition = new Vector3(-1.15f, 4.0f, 1.46f);
        //var TargetPosition = new Vector3(-1.15f, 5.51f, -3.54f);
        HelperFunctions.MoveObejectTowardsPosition(this, wall.Chest, TargetPosition, 20.0f);
        yield return new WaitForSeconds(3.0f);
        MainGameSceneMonoBehavior.Instance.SpeedMultiplier = 1;
        //yield return new WaitForSeconds(1.0f);
        RemoveStoreObjectFromGamePlay();
        //yield return new WaitForSeconds(0.5f);
        wall.ConvertColorBall = true;
        
        //HelperFunctions.MoveObejectTowardsPosition(this, powerUp, TargetPosition, 20.0f);
        //wall.ColorBallTriggered = true;
        yield return new WaitForSeconds(1.0f);
        //MainGameSceneMonoBehavior.Instance.AddGemToMeter();
    }

    public override void ApplyStoreObjectToGamePlay(StoreObjectsController storeObjectsController)
    {
        base.ApplyStoreObjectToGamePlay(storeObjectsController);
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponent<Animator>().enabled = true;
        MagicWand.SetActive(true);
    }

    public override void RemoveStoreObjectFromGamePlay()
    {
        base.RemoveStoreObjectFromGamePlay();
        this.GetComponent<BoxCollider>().enabled = false;
        this.GetComponent<Animator>().SetBool("TriggerMagic", false);
        this.GetComponent<Animator>().enabled = false;
        MagicWand.SetActive(false);
    }
}
