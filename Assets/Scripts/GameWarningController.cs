using ColorSnipersU.MonoBehaviors.MainGameScene;
using UnityEngine;

public class GameWarningController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

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
                    //SoundController.Instance.Warning.Play();
                }

            }
        }
    }
}