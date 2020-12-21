using ColorSnipersU.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StoreMonoBehavior : MonoBehaviour
{

    public Text GemText;
    public Text PriceText;
    public Image PurchaseItemImage;
    public Image PopUpsBg;
    public GameObject ConfirmationPopUp;
    public GameObject InsufficientFundsPopUp;
    public StorePurchaseItem[] StorePurchaseItems;

    private StoreObjectPurchaseController.StoreObject storeObjectSelected;

    // Use this for initialization
    void Start()
    {
        GemText.text = GemScript.Instance.TotalGems.ToString();
        StoreObjectPurchaseController.Instance.OnPurchaseComplete += OnPurchaseComplete;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ConfirmationPopUp.activeSelf)
            {
                CancelPurchase();
                PopUpsBg.enabled = false;
            }
            else if(InsufficientFundsPopUp.activeSelf)
            {
                InsufficientFundsPopUp.SetActive(false);
                PopUpsBg.enabled = false;
            }
            else
                HelperFunctions.LoadSceneAsync(ColorSnipersU.Utilities.Enumerations.GameScenes.Menu, this, null);
        }
    }

    public void ShowPurchaseConfirmPopUp(int storeObjectIndex)
    {
        StoreObjectPurchaseController.StoreObject storeObject = (StoreObjectPurchaseController.StoreObject)storeObjectIndex;
        if (StoreObjectPurchaseController.Instance.IsSuffientFundsAvailable(storeObject))
        {
            StorePriceScriptableObject storePrice = StoreObjectPurchaseController.Instance.StorePrice;
            StorePriceScriptableObject.StorePriceMap storePriceMap = storePrice.GetStorePriceMap(storeObject);
            PriceText.text = storePriceMap.Price.ToString();
            PurchaseItemImage.sprite = storePriceMap.NoBgImage;
            ConfirmationPopUp.SetActive(true);
            PopUpsBg.enabled = true;
            storeObjectSelected = storeObject;
        }
        else
        {
            InsufficientFundsPopUp.SetActive(true);
            PopUpsBg.enabled = true;
            StartCoroutine(InsufficientFundTimeoutCoroutine());
        }

    }

    public void PurchaseStoreObject()
    {
        if (storeObjectSelected != StoreObjectPurchaseController.StoreObject.None)
        {
            StoreObjectPurchaseController.Instance.PurchaseStoreObject(storeObjectSelected);
            ConfirmationPopUp.SetActive(false);
            PopUpsBg.enabled = false;
            foreach (var storePurchaseItem in StorePurchaseItems)
            {
                if(storePurchaseItem.storeObject == storeObjectSelected)
                {
                    storePurchaseItem.UpdateActiveObjectsText();
                }
            }
        }
    }

    public void CancelPurchase()
    {
        storeObjectSelected = StoreObjectPurchaseController.StoreObject.None;
        ConfirmationPopUp.SetActive(false);
        PopUpsBg.enabled = false;
    }

    private IEnumerator InsufficientFundTimeoutCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        InsufficientFundsPopUp.SetActive(false);
        PopUpsBg.enabled = false;
        StopCoroutine(InsufficientFundTimeoutCoroutine());
    }

    public void OnPurchaseComplete(StoreObjectPurchaseController.StoreObject storeObject)
    {
        GemText.text = GemScript.Instance.TotalGems.ToString();
    }

}
