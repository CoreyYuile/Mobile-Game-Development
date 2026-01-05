using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
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

    // Load the banner ad
    public void LoadBannerAd()
    {
        // Set a position for the banner
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

        // Options for specifying what callbacks should be used
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = BannerLoaded,
            errorCallback = BannerLoadedError
        };

        // Load the banner ad with the correct banner unit ID
        Advertisement.Banner.Load(adUnitID, options);
    }

    public void ShowBannerAd()
    {

        BannerOptions options = new BannerOptions
        {
            showCallback = BannerShown,
            clickCallback = BannerClicked,
            hideCallback = BannerHidden
        };

        Advertisement.Banner.Show(adUnitID, options);
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    private void BannerHidden()
    {

    }

    private void BannerClicked()
    {

    }

    private void BannerShown()
    {

    }

    private void BannerLoadedError(string message)
    {
        Debug.Log(message);
    }

    private void BannerLoaded()
    {
        Debug.Log("Banner ad loaded!");
        //ShowBannerAd();
    }
}
