using System.Collections.Generic;
using UnityEngine;

public class BackgroundCollisionScript : MonoBehaviour
{

    public MainGameSceneMonoBehavior sceneMonoBehavior;
    public bool isDestroyer;

    public GameObject GemRef;
    public Transform GemParent;

    private List<GameObject> gameObjectsToDestroy;
    // Use this for initialization
    void Start()
    {
        gameObjectsToDestroy = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter(Collider collision)
    {
        //if (sceneMonoBehavior.CurrentLevel.isLoadStatic)
        //    return;
        if (collision.CompareTag("Wall") || collision.CompareTag("FillingWall"))
        {
            if (!isDestroyer && !sceneMonoBehavior.CurrentLevel.isLoadStatic)
            {
                sceneMonoBehavior.GenerateWall(collision.transform.position.x);
                collision.enabled = false;
                MainGameSceneMonoBehavior.Instance.TotalWallsIncludingNonWindowWalls++;
                //bool isNoWindowWallGenerated = true ;
                //GameObject nextWall = sceneMonoBehavior.InstantitateNextBackground(collision.transform.position.x,out isNoWindowWallGenerated);
                //collision.enabled = false;
                //if (isNoWindowWallGenerated)
                //{
                //    GameObject gem = Instantiate(GemRef);
                //    var yPos = Random.Range(-0.3f, 0.4f);
                //    gem.transform.SetParent(nextWall.transform);
                //    gem.transform.localPosition = new Vector3(0, yPos, 0);
                //    gem.transform.SetParent(nextWall.transform);
                //}
            }
            else
            {
                if (!sceneMonoBehavior.CurrentLevel.isLoadStatic)
                    Destroy(collision.gameObject);
                else
                {
                    gameObjectsToDestroy.Add(collision.gameObject);
                }
            }
        }

        else if ((collision.name == "Background" || collision.CompareTag("FillingWall")) && isDestroyer)
            Destroy(collision.gameObject);
    }

    public void DestroyAllWallsInList()
    {
        foreach (var item in gameObjectsToDestroy)
        {
            Destroy(item);
        }
        gameObjectsToDestroy.Clear();
    }
}
