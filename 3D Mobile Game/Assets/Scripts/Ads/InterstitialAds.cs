using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] private string androidAdUnitID;
    [SerializeField] private string iosAdUnitID;

    private string adUnitID;

    private void Awake()
    {
        // Check what device is currently in use
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

    // Loads the ad
    public void LoadInterstitialAd()
    {
        Advertisement.Load(adUnitID, this);
    }

    // Show the ad, load next one
    public void ShowIntersitialAd()
    {
        Advertisement.Show(adUnitID, this);
        LoadInterstitialAd();
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

    // Check for if ad has finished
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Interstitial ad completed");
    }
}
