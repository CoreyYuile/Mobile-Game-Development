using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] private string androidAdUnitID;
    [SerializeField] private string iosAdUnitID;

    private string adUnitID;

    private void Awake()
    {
#if UNITY_IOS
        adUnitID = iosAdUnitID;
#elif UNITY_ANDROID
adUnitID = androidAdUnitID;
#endif
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(adUnitID, this);
    }

    public void ShowRewardedAd()
    {
        // Show the ad, load next one so it is ready ASAP.
        Advertisement.Show(adUnitID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    // Check for when ad is complete
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitID && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // Ad is complete, give the player reward
            Debug.Log("Rewarded ad completed");
            MoneyManager.Instance.AddMoney(100);

            LoadRewardedAd();
        }
    }
}
