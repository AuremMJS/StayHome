using UnityEngine;


namespace ColorSnipersU.MonoBehaviors.MainGameScene
{
    public class HiddenWallRevealController : MonoBehaviour
    {

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Window")
            {
                GameObject wallGO = collision.collider.transform.parent.parent.parent.parent.parent.gameObject;
                if (wallGO != null)
                {
                    Animator wallAnimator = wallGO.GetComponent<Animator>();
                    if(wallAnimator!=null)
                    {
                        wallAnimator.SetTrigger("RevealWindow");
                    }
                }
            }
        }
    }
}