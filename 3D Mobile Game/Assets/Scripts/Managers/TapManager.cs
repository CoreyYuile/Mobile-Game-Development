using UnityEngine;
using UnityEngine.EventSystems;

public class TapManager : MonoBehaviour
{

    public static TapManager Instance {  get; private set; }

    [Header("Tap Settings")]

    [SerializeField] private float tapThreshold = 15f;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool isTapping = false;

    [Header("Shake Settings")]

    [SerializeField] private float shakeThreshold = 2.5f;
    [SerializeField] private float shakeCooldown = 1.0f;
    public bool isHarvesting = true;

    private float lastTimeShaked;

    [Header("References")]

    [SerializeField] private Camera mainCamera;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        DetectTap();
        DetectShake();
    }

    private void DetectTap()
    {
        // Check if only one finger is held on the screen
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Ignore UI touches
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                isTapping = false ;
                return;
            }

            switch (touch.phase)
            {
                // Assume the player is tapping, get the start position of the tap
                case TouchPhase.Began:
                    isTapping = true;
                    touchStartPos = touch.position;
                    break;

                //case TouchPhase.Moved:
                //    if (Vector2.Distance(touchStartPos, touch.position) > tapThreshold)
                //        isTapping = false;
                //    break;

                // Player has let go of finger on phone
                case TouchPhase.Ended:
                    // Get the end position of the tap, check if the start and end are the same
                    touchEndPos = touch.position;
                    if (Vector2.Distance(touchStartPos, touchEndPos) > tapThreshold)
                    {
                        isTapping = false;
                    }
                    // If they are, go ahead with checking what the player has tapped on
                    if (isTapping)
                    {
                        TrySelectPlot(touch.position);
                    }
                    isTapping = false;
                    break;
            }
        }
    }

    private void DetectShake()
    {
        Vector3 acceleration = Input.acceleration;

        float magnitude = acceleration.magnitude;

        if (magnitude > shakeThreshold && Time.time > shakeCooldown)
        {
            Debug.Log("Shake Detected");
            shakeCooldown = Time.time;
            if (isHarvesting)
            {
                AutoHarvest();
            }
            else
            {
                AutoPlant();
            }
        }
    }

    private void AutoHarvest()
    {
        FarmPlot[] plots = FindObjectsOfType<FarmPlot>();

        foreach (var plot in plots)
        {
            if (plot.state == FarmPlot.PlotState.ReadyToHarvest)
            {
                plot.HarvestCrop();
            }
        }
    }

    private void AutoPlant()
    {
        FarmPlot[] plots = FindObjectsOfType<FarmPlot>();

        foreach(var plot in plots)
        {
            if (plot.state == FarmPlot.PlotState.Empty && plot.isOwned == true)
            {
                plot.PlantSeed(MenuManager.Instance.availableCrops[0]);
            }
        }
    }

    private void TrySelectPlot(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            FarmPlot plot = hit.collider.GetComponentInParent<FarmPlot>();
            if (plot != null)
            {
                plot.HandleTap();
            }
        }
    }
}
