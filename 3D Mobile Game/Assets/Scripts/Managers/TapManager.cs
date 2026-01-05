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

    [SerializeField] private float shakeThreshold = 1.5f;
    [SerializeField] private float shakeCooldown = 1.0f;
    private float shakeTimer = 0.0f;

    public bool isHarvesting = true;

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
                        SelectPlot(touch.position);
                    }
                    isTapping = false;
                    break;
            }
        }
    }

    // Check if the player is shaking for harvest / planting
    private void DetectShake()
    {
        Vector3 acceleration = Input.acceleration;

        float magnitude = acceleration.magnitude;

        // Check if the magnitude of the player's shake surpasses the threshold (so it doesn't accidentally trigger)
        if (magnitude > shakeThreshold && shakeTimer > shakeCooldown)
        {
            Debug.Log("Shake Detected");
            shakeTimer = Time.deltaTime;

            // Depends on what is currently selected from the AutoHarvest toggle
            if (isHarvesting)
            {
                AutoHarvest();
            }
            else
            {
                AutoPlant();
            }
        }

        shakeTimer += Time.deltaTime;
    }

    // Harvest all plots with crops that are fully grown
    private void AutoHarvest()
    {
        // FindObjectsOfType is obsolete, apparently this is faster??
        FarmPlot[] plots = FindObjectsByType<FarmPlot>(FindObjectsSortMode.None);

        // Loop through all plots, find what ones are able to be harvested, and harvest them
        foreach (var plot in plots)
        {
            if (plot.state == FarmPlot.PlotState.ReadyToHarvest)
            {
                plot.HarvestCrop();
            }
        }
    }

    // Plant in all available empty plots
    private void AutoPlant()
    {
        // FindObjectsOfType is obsolete, apparently this is faster??
        FarmPlot[] plots = FindObjectsByType<FarmPlot>(FindObjectsSortMode.None);

        // Loop through all plots, find what ones are empty, and plant the selected crop in them
        foreach(var plot in plots)
        {
            if (plot.state == FarmPlot.PlotState.Empty && plot.isOwned == true)
            {
                plot.PlantSeed(MenuManager.Instance.selectedCrop);
            }
        }
    }

    // Cast a ray from the camera / tap position and try to find the selected farm plot
    private void SelectPlot(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if hit object is a farm plot
            FarmPlot plot = hit.collider.GetComponentInParent<FarmPlot>();
            if (plot != null)
            {
                plot.HandleTap();
            }
        }
    }
}
