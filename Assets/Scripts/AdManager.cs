using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

public class AdManager : MonoBehaviour
{

    private RewardBasedVideoAd rewardBasedVideoAd;

    public GameObject RewardPresentation;

    public GameObject[] Rewards;

    void Awake()
    {
        rewardBasedVideoAd = RewardBasedVideoAd.Instance;

        rewardBasedVideoAd.OnAdLoaded += HandleOnAdLoaded;

        rewardBasedVideoAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        rewardBasedVideoAd.OnAdOpening += HandleOnAdOpening;
        rewardBasedVideoAd.OnAdStarted += HandleOnAdStarted;
        rewardBasedVideoAd.OnAdClosed += HandleOnAdClosed;
        rewardBasedVideoAd.OnAdRewarded += HandleOnAdRewarded;
        rewardBasedVideoAd.OnAdLeavingApplication += OnAdLeavingApplication;
        rewardBasedVideoAd.OnAdCompleted += HandleOnAdCompleted;
    }

    private void HandleOnAdCompleted(object sender, EventArgs e)
    {

    }

    private void OnAdLeavingApplication(object sender, EventArgs e)
    {

    }

    private void HandleOnAdRewarded(object sender, Reward e)
    {
        if (GameTicketManager.Instance.NoOfTickets < 1)
        {
            GameTicketManager.Instance.IncrementTickets();
            Rewards[5].SetActive(true);
            GameTicketManager.Instance.WatchAdPopUp.SetActive(false);
            StartCoroutine(RevealReward());
            Debug.Log("CSU Ad: " + GameTicketManager.Instance.NoOfTickets);
        }
        else
        {
            int rewardedSO = StoreObjectPurchaseController.Instance.AddRandomStoreObject();
            Rewards[rewardedSO - 1].SetActive(true);
            StartCoroutine(RevealReward());
            GameTicketManager.Instance.ShowStoreObjectIcons();
        }
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        // Load next Ad
        LoadRewardBasedAd();
    }

    private void HandleOnAdStarted(object sender, EventArgs e)
    {

    }

    private void HandleOnAdOpening(object sender, EventArgs e)
    {

    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        // Retry
        LoadRewardBasedAd();
    }

    private void HandleOnAdLoaded(object sender, EventArgs e)
    {
        //ShowRewardBasedAd();
    }

    // Use this for initialization
    void Start()
    {
        LoadRewardBasedAd();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadRewardBasedAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";

#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-5019187389656355/5028259383";
#else
        string adUnitId = "unexpected_platform";
#endif
        AdRequest adRequest = new AdRequest.Builder()
            .AddTestDevice("37AD81FDCADE8F75020206B099863822") // RM
            .AddTestDevice("3B40082CB7FBC1BA7DBDA1919DA723F2") // BALA ANNA
            .AddTestDevice("F3177A6A023442B55DD9260015E11858") // AAKAASH
            .AddTestDevice("F093BA6F4C72F1D97A594283E80C3D65") // ABI
            .AddTestDevice("6C6C74582FF3414E7E05DC8F696C18D7") // GOWTHAM
            .AddTestDevice("A2558CA0089069CCC2C6AA013D4368E7") // SURESH BABU
            .Build();
        rewardBasedVideoAd.LoadAd(adRequest, adUnitId);
    }

    public void ShowRewardBasedAd()
    {
#if UNITY_EDITOR
        Rewards[0].SetActive(true);
        StartCoroutine(RevealReward());
#endif
        if (rewardBasedVideoAd.IsLoaded())
        {
            rewardBasedVideoAd.Show();
        }
        else
        {
            Debug.Log("Ads: Error");
        }
    }

    private IEnumerator RevealReward()
    {
        RewardPresentation.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        foreach (var item in Rewards)
        {
            item.SetActive(false);
        }
        RewardPresentation.SetActive(false);
    }

    public void LoadAndShowRewardBasedVideoAd()
    {
        //LoadRewardBasedAd();
        ShowRewardBasedAd();
    }


}
