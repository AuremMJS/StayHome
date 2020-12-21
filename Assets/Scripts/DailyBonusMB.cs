using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonusMB : MonoBehaviour
{
    public GameObject DailyBonusPresentation;

    public GameObject[] Bonuses;

    public Text GemText;

    int index;

    // Use this for initialization
    void Start()
    {
        DateTime LastBonusAwardedDateTime = PlayerPrefs.HasKey("LastBonusAwardedDateTime") ? Convert.ToDateTime(PlayerPrefs.GetString("LastBonusAwardedDateTime")) : DateTime.MinValue;
        if (LastBonusAwardedDateTime.CompareTo(DateTime.Today) != 0)
        {
            int rand = UnityEngine.Random.Range(0, 3);
            index = -1;
            if (rand == 1)
            {
                index = 5;
            }
            else if (rand == 2)
            {
                index = 6;
            }
            else
            {
                index = StoreObjectPurchaseController.Instance.AddRandomStoreObject(false) - 1;
            }
            Bonuses[index].SetActive(true);
            DailyBonusPresentation.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClaimBonus()
    {
        if (index < 5)
        {
            StoreObjectPurchaseController.StoreObject storeObject = (StoreObjectPurchaseController.StoreObject)(index + 1);
            StoreObjectPurchaseController.Instance.AddStoreObject(storeObject);
        }
        else if (index == 5)
        {
            GameTicketManager.Instance.IncrementTickets();
        }
        else if (index == 6)
        {
            GemScript.Instance.TotalGems += 50;
            GemText.text = GemScript.Instance.TotalGems.ToString();
        }
        DailyBonusPresentation.SetActive(false);
        PlayerPrefs.SetString("LastBonusAwardedDateTime", DateTime.Today.ToString());
        PlayerPrefs.Save();

    }
}
