using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StoreObject : MonoBehaviour
{
    public StoreObjectPurchaseController.StoreObject storeObjectID;
    public Button StoreObjectButton;
    public Text CountText;
    public float Time = -1;
    public Text StoreObjectTimer;
    public GameObject StoreObjectTimerGO;

    // Use this for initialization
    protected virtual void Start()
    {
        int NoOfStoreObjectPurchased = StoreObjectPurchaseController.Instance.GetNoOfStoreObjectPurchased(storeObjectID);
        bool DoesThisStoreObjectExist = NoOfStoreObjectPurchased > 0;
        StoreObjectButton.interactable = DoesThisStoreObjectExist;
        CountText.text = DoesThisStoreObjectExist ? NoOfStoreObjectPurchased.ToString() : string.Empty;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Initialize(Action[] appliedEventActions, Action[] removedEventActions)
    {

    }

    public virtual void ApplyStoreObjectToGamePlay(StoreObjectsController storeObjectsController)
    {
        StoreObjectButton.interactable = false;
        if (Time != -1)
        {
            StoreObjectTimerGO.SetActive(true);
            StartCoroutine(StoreObjectCoroutine());
        }
        StoreObjectPurchaseController.Instance.UseStoreObject(storeObjectID);
        int NoOfStoreObjectPurchased = StoreObjectPurchaseController.Instance.GetNoOfStoreObjectPurchased(storeObjectID);
        bool DoesThisStoreObjectExist = NoOfStoreObjectPurchased > 0;
        StoreObjectButton.interactable = DoesThisStoreObjectExist;
        CountText.text = DoesThisStoreObjectExist ? NoOfStoreObjectPurchased.ToString() : string.Empty;
    }

    private IEnumerator StoreObjectCoroutine()
    {
        for (int i = 0; i < Time; i++)
        {
            StoreObjectTimer.text = (Time - i) + " Seconds";
            yield return new WaitForSeconds(1.0f);
        }
        //yield return new WaitForSeconds(Time);
        RemoveStoreObjectFromGamePlay();
    }

    public virtual void RemoveStoreObjectFromGamePlay()
    {
        if (StoreObjectPurchaseController.Instance.GetNoOfStoreObjectPurchased(storeObjectID) > 0)
        {
            StoreObjectButton.interactable = true;
        }
        else
            StoreObjectButton.interactable = false;
        StoreObjectTimer.text = "";
        StoreObjectTimerGO.SetActive(false);
    }
}
