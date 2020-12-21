using UnityEngine;
using UnityEngine.UI;

public class StorePurchaseItem : MonoBehaviour {

    public Text Title;
    public Image Image;
    public Text Description;
    public Text Price;
    public GameObject Gem;
    public Text ActiveObjects;
    public StoreObjectPurchaseController.StoreObject storeObject;

	// Use this for initialization
	void Start () {
        StorePriceScriptableObject storePrice = StoreObjectPurchaseController.Instance.StorePrice;
        StorePriceScriptableObject.StorePriceMap storePriceMap = storePrice.GetStorePriceMap(storeObject);
        Title.text = storeObject.ToString();
        Description.text = storePriceMap.Description;
        Gem.SetActive(storePriceMap.PurchaseUsingGem);
        string PriceText = storePriceMap.PurchaseUsingGem ? storePriceMap.Price.ToString() : "$ " + storePriceMap.Price.ToString();
        Price.text = PriceText;
        ActiveObjects.text = "Active Items : " + StoreObjectPurchaseController.Instance.GetNoOfStoreObjectPurchased(storeObject);
        Image.sprite = storePriceMap.Image;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateActiveObjectsText()
    {
        ActiveObjects.text = "Active Items : " + StoreObjectPurchaseController.Instance.GetNoOfStoreObjectPurchased(storeObject);
    }
}
