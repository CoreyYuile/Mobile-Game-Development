using UnityEngine;

public class AdsManager : MonoBehaviour
{

    public InitialiseAds initialiseAds;
    public InterstitialAds interstitialAds;
    public RewardedAds rewardedAds;
    public BannerAds bannerAds;

    public static AdsManager instance { get; private set; }

    public bool isRewarded;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Load ads ahead of time
        //interstitialAds.LoadInterstitialAd();
        //rewardedAds.LoadRewardedAd();
        //bannerAds.LoadBannerAd();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Load ads ahead of time
    public void OnAdsInitialised()
    {
        interstitialAds.LoadInterstitialAd();
        rewardedAds.LoadRewardedAd();
        bannerAds.LoadBannerAd();

        //bannerAds.ShowBannerAd();
    }
}
