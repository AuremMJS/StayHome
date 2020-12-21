using ColorSnipersU.MonoBehaviors.MainGameScene;
using System.Collections;
using UnityEngine;

public class WindowWitchStoreObject : StoreObject {

    public GameObject SmokeObject;

    public GameObject Witch;
	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("WallMidPoint"))
        {
            GameObject wallGO = collider.transform.parent.gameObject;
            if (wallGO != null)
            {
                WallBase wallScript = wallGO.GetComponent<WallBase>();
                if (wallScript != null)
                {
                    StartCoroutine(CloseWindowCoroutine(wallScript));
                }
            }
        }
    }

    private IEnumerator CloseWindowCoroutine(WallBase wall)
    {
        Witch.GetComponent<Animator>().SetTrigger("CloseWindow");
        yield return new WaitForSeconds(0.5f);
        wall.ClosingBegan = true;
        wall.Closed = true;
    }

    // Method to apply window witch store object to game play
    public override void ApplyStoreObjectToGamePlay(StoreObjectsController storeObjectsController)
    {
        base.ApplyStoreObjectToGamePlay(storeObjectsController);
        StartCoroutine(WitchAppearCoroutine());
    }

    // Coroutine to start witch appearance effect
    private IEnumerator WitchAppearCoroutine()
    {
        SmokeObject.SetActive(false);   
        SmokeObject.SetActive(true);
        MainGameSceneMonoBehavior.Instance.SpeedMultiplier = 0;
        yield return new WaitForSeconds(1.0f);
        MainGameSceneMonoBehavior.Instance.SpeedMultiplier = 2f;
        Witch.SetActive(true);
        Witch.GetComponent<Animator>().SetFloat("SpeedMult", MainGameSceneMonoBehavior.Instance.TotalSpeed / 6.0f);
        this.GetComponent<BoxCollider>().enabled = true;
    }

    public override void RemoveStoreObjectFromGamePlay()
    {
        base.RemoveStoreObjectFromGamePlay();
        this.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(WitchDisappearCoroutine());
    }

    private IEnumerator WitchDisappearCoroutine()
    {
        SmokeObject.SetActive(false);
        SmokeObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        MainGameSceneMonoBehavior.Instance.SpeedMultiplier = 1;
        Witch.SetActive(false);
    }
}
