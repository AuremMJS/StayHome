using System;
using UnityEngine;

public class RainStoreObject : StoreObject
{

    public GameObject Rain;

    public event Action RainStoreObjectApplied;
    public event Action RainStoreObjectRemoved;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Initialize(Action[] appliedEventActions, Action[] removedEventActions)
    {
        base.Initialize(appliedEventActions, removedEventActions);
        if (appliedEventActions != null)
            foreach (var item in appliedEventActions)
            {
                RainStoreObjectApplied += item;
            }

        if (removedEventActions != null)
            foreach (var item in removedEventActions)
            {
                RainStoreObjectRemoved += item;
            }


    }

    public override void ApplyStoreObjectToGamePlay(StoreObjectsController storeObjectsController)
    {
        base.ApplyStoreObjectToGamePlay(storeObjectsController);
        RainStoreObjectApplied();
        Rain.SetActive(true);
    }

    public override void RemoveStoreObjectFromGamePlay()
    {
        base.RemoveStoreObjectFromGamePlay();
        RainStoreObjectRemoved();
        Rain.SetActive(false);
        
    }
}
