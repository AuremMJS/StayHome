using ColorSnipersU.MonoBehaviors.MainGameScene;
using UnityEngine;

public class NearMissController : MonoBehaviour
{
    public static NearMissController Instance;

    public int NearMissBonus;

    // Use this for initialization
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("WallMidPoint"))
        {
            GameObject wallGO = collision.collider.transform.parent.gameObject;
            if (wallGO != null)
            {
                WallBase wallScript = wallGO.GetComponent<WallBase>();
                if (wallScript != null && !wallScript.IsNonWindowWall)
                {
                    wallScript.ReportNearMiss = true;
                }
            }
        }
    }

    public void WallNearMissed()
    {
        MainGameSceneMonoBehavior.Instance.AddGemToMeter(NearMissBonus, "Near Miss\n+" + NearMissBonus);
    }
}
