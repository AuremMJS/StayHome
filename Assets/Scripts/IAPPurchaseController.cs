using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace ColorSnipersU
{
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class IAPPurchaseController : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        public static string productID_50Gems = "com.auremgamestudio.colorsnipersu3d.50gems";
        public static string productID_100Gems = "com.auremgamestudio.colorsnipersu3d.100gems";
        public static string productID_200Gems = "com.auremgamestudio.colorsnipersu3d.200gems";
        public static string productID_500Gems = "com.auremgamestudio.colorsnipersu3d.500gems";
        public static string productID_1000Gems = "com.auremgamestudio.colorsnipersu3d.1000gems";

        public Text product_50Gems_Text;
        public Text product_100Gems_Text;
        public Text product_200Gems_Text;
        public Text product_500Gems_Text;
        public Text product_1000Gems_Text;
        public Text GemText;

        void Awake()
        {
            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
                InitializePurchasing();

            }
        }
            void Start()
        {
            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
         //       InitializePurchasing();
                if (IsInitialized())
                {
                    product_50Gems_Text.text = m_StoreController.products.WithID(productID_50Gems).metadata.localizedPrice.ToString();
                    product_100Gems_Text.text = m_StoreController.products.WithID(productID_100Gems).metadata.localizedPrice.ToString();
                    product_200Gems_Text.text = m_StoreController.products.WithID(productID_200Gems).metadata.localizedPrice.ToString();
                    product_500Gems_Text.text = m_StoreController.products.WithID(productID_500Gems).metadata.localizedPrice.ToString();
                    product_1000Gems_Text.text = m_StoreController.products.WithID(productID_1000Gems).metadata.localizedPrice.ToString();
                }
            }
        }

        public void InitializePurchasing()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.
            builder.AddProduct(productID_50Gems, ProductType.Consumable);
            builder.AddProduct(productID_100Gems, ProductType.Consumable);
            builder.AddProduct(productID_200Gems, ProductType.Consumable);
            builder.AddProduct(productID_500Gems, ProductType.Consumable);
            builder.AddProduct(productID_1000Gems, ProductType.Consumable);

            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }


        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        public void BuyGems(int noOfGems)
        {
            string productId = string.Empty;
            switch (noOfGems)
            {
                case 50:
                    productId = productID_50Gems;
                    break;
                case 100:
                    productId = productID_100Gems;
                    break;
                case 200:
                    productId = productID_200Gems;
                    break;
                case 500:
                    productId = productID_500Gems;
                    break;
                case 1000:
                    productId = productID_1000Gems;
                    break;
                default:
                    productId = string.Empty;
                    break;
            }
            if (!string.IsNullOrEmpty(productId))
                BuyProductID(productId);
        }

        void BuyProductID(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = m_StoreController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }




        //  
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;

            product_50Gems_Text.text = m_StoreController.products.WithID(productID_50Gems).metadata.localizedPriceString.ToString();
            product_100Gems_Text.text = m_StoreController.products.WithID(productID_100Gems).metadata.localizedPriceString.ToString();
            product_200Gems_Text.text = m_StoreController.products.WithID(productID_200Gems).metadata.localizedPriceString.ToString();
            product_500Gems_Text.text = m_StoreController.products.WithID(productID_500Gems).metadata.localizedPriceString.ToString();
            product_1000Gems_Text.text = m_StoreController.products.WithID(productID_1000Gems).metadata.localizedPriceString.ToString();
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            int GemsToAdd = 0;
            // A consumable product has been purchased by this user.
            if (String.Equals(args.purchasedProduct.definition.id, productID_50Gems, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                GemsToAdd = 50;
            }
            else if (String.Equals(args.purchasedProduct.definition.id, productID_100Gems, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                GemsToAdd = 100;
            }
            else if (String.Equals(args.purchasedProduct.definition.id, productID_200Gems, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                GemsToAdd = 200;
            }
            else if (String.Equals(args.purchasedProduct.definition.id, productID_500Gems, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                GemsToAdd = 500;
            }
            else if (String.Equals(args.purchasedProduct.definition.id, productID_1000Gems, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                GemsToAdd = 1000;
            }
            // Or ... an unknown product has been purchased by this user. Fill in additional products here....
            else
            {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }
            GemScript.Instance.TotalGems += GemsToAdd;
            GemText.text = GemScript.Instance.TotalGems.ToString();

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
    }
}