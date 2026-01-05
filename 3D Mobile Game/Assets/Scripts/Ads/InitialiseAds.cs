using UnityEngine;
using UnityEngine.Advertisements;

public class InitialiseAds : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameID;
    [SerializeField] private string iosGameID;
    [SerializeField] private bool isTesting;

    private string gameID;

    private void Awake()
    {
        // Check what device is currently in use
#if UNITY_IOS
gameID = iosGameID;
#elif UNITY_ANDROID
gameID = androidGameID;
#elif UNITY_EDITOR
gameID = androidGameID;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameID, isTesting, this);
        }
    }

    // Call adsmanager to load all available ad types
    public void OnInitializationComplete()
    {
        Debug.Log("Ads initialised!");
        AdsManager.Instance.OnAdsInitialised();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }
}
