using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StorePriceSO", menuName = "StorePrice", order = 1)]
public class StorePriceScriptableObject : ScriptableObject
{
    [Serializable]
    public struct StorePriceMap
    {
        public StoreObjectPurchaseController.StoreObject storeObject;
        public bool PurchaseUsingGem;
        public double Price;
        public string Description;
        public Sprite Image;
        public Sprite NoBgImage;
    }

    public StorePriceMap[] StorePrices; 
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public StorePriceMap GetStorePriceMap(StoreObjectPurchaseController.StoreObject storeObject)
    {
        var storePriceMap = from p in StorePrices
                    where p.storeObject == storeObject
                    select p;
        return storePriceMap.FirstOrDefault();
    }
}
