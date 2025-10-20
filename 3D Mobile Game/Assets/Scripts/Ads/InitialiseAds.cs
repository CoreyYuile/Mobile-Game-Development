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
gameID = iosGameID;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameID, isTesting, this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ads initialised!");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }
}
