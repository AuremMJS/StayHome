using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreObjectPurchaseController : MonoBehaviour
{
    public StorePriceScriptableObject StorePrice;

    public event Action<StoreObject> OnPurchaseComplete;

    public enum StoreObject
    {
        Rain = 1,
        MagicWand,
        MudGun,
        WindowWitch,
        Shield,
        Gem,
        None = -1
    }

    private Dictionary<StoreObject, int> purchasedStoreObjects;

    public static StoreObjectPurchaseController Instance;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        purchasedStoreObjects = new Dictionary<StoreObject, int>();
        foreach (var item in Enum.GetValues(typeof(StoreObject)).Cast<StoreObject>().ToArray())
        {
            if (PlayerPrefs.HasKey("StoreObject_" + item.ToString()))
            {
                purchasedStoreObjects.Add(item, PlayerPrefs.GetInt("StoreObject_" + item.ToString()));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PurchaseStoreObject(StoreObject storeObject)
    {
        StorePriceScriptableObject.StorePriceMap storePriceMap = StorePrice.GetStorePriceMap(storeObject);
        if (storePriceMap.PurchaseUsingGem)
        {
            if (GemScript.Instance.TotalGems >= storePriceMap.Price)
            {
                AddStoreObject(storeObject);
                GemScript.Instance.TotalGems -= (int)storePriceMap.Price;
                OnPurchaseComplete(storeObject);
            }
        }
        else
        {
            return;
        }


    }

    public void AddStoreObject(StoreObject storeObject)
    {
        if (purchasedStoreObjects.ContainsKey(storeObject))
        {
            purchasedStoreObjects[storeObject]++;
        }
        else
        {
            purchasedStoreObjects.Add(storeObject, 1);
        }
    }

    public int AddRandomStoreObject()
    {
        int rand = UnityEngine.Random.Range(1, 6);
        StoreObject storeObj = (StoreObject)rand;
        AddStoreObject(storeObj);
        return rand;
    }

    public int AddRandomStoreObject(bool AddImmediate)
    {
        int rand = UnityEngine.Random.Range(1, 6);
        StoreObject storeObj = (StoreObject)rand;
        if (AddImmediate)
            AddStoreObject(storeObj);
        return rand;
    }

    public void UseStoreObject(StoreObject storeObject)
    {
        if (purchasedStoreObjects.ContainsKey(storeObject))
        {
            purchasedStoreObjects[storeObject]--;
        }
    }

    public bool IsSuffientFundsAvailable(StoreObject storeObject)
    {
        bool sufficientFundsAvailable = false;
        StorePriceScriptableObject.StorePriceMap storePriceMap = StorePrice.GetStorePriceMap(storeObject);
        if (storePriceMap.PurchaseUsingGem)
        {
            if (GemScript.Instance.TotalGems >= storePriceMap.Price)
            {
                sufficientFundsAvailable = true;
            }
        }
        return sufficientFundsAvailable;
    }

    public int GetNoOfStoreObjectPurchased(StoreObject storeObject)
    {
        if (purchasedStoreObjects.ContainsKey(storeObject))
        {
            return purchasedStoreObjects[storeObject];
        }
        return 0;
    }

    public void ExitGame()
    {
        foreach (var item in purchasedStoreObjects)
        {
            PlayerPrefs.SetInt("StoreObject_" + item.Key.ToString(), item.Value);
            PlayerPrefs.Save();
        }
    }
}
