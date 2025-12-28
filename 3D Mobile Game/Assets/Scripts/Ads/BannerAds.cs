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

    public void LoadBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = BannerLoaded,
            errorCallback = BannerLoadedError
        };

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
