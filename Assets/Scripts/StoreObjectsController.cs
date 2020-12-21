using System;
using UnityEngine;

public class StoreObjectsController : MonoBehaviour
{

    public MainGameSceneMonoBehavior mainGameSceneMonoBehavior;
    public StoreObject[] storeObjects;

    // Use this for initialization
    void Start()
    {
        storeObjects[0].Initialize(new Action[] { mainGameSceneMonoBehavior.FadeColorBallsOnShoot }, new Action[] { mainGameSceneMonoBehavior.DisableFadeColorBallsOnShoot });
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ActivateStoreObject(int storeObject)
    {
        storeObjects[storeObject].ApplyStoreObjectToGamePlay(this);
    }
}
