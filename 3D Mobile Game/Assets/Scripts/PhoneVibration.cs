using UnityEngine;
using UnityEngine.UI;
using CandyCoded;
using CandyCoded.HapticFeedback;

public class PhoneVibration : MonoBehaviour
{

    // !! I ONLY HAVE TO ASSUME THIS IS WORKING !!
    // Ok so I made this and then realised that both the hapticfeedback package AND handheld.vibrate don't work on phone is simulator :/
    // Only way to truly check if its working is by making a build, which is gonna SUCK on iOS
    // But hey I guess it works, handheld.vibrate gives a debug log...

    public static PhoneVibration Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Play light haptic vibration
    public void LightVibration()
    {
        Debug.Log("Light vibration performed");
        HapticFeedback.LightFeedback();
    }

    // Play medium haptic vibration
    public void MediumVibration()
    {
        Debug.Log("Medium vibration performed");
        HapticFeedback.MediumFeedback();
    }

    // Play heavy haptic vibration
    public void HeavyVibration()
    {
        Debug.Log("Heavy vibration performed");
        HapticFeedback.HeavyFeedback();
        Handheld.Vibrate();
    }
}
