using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Cinemachine;

public class CameraPan : MonoBehaviour
{

    // !! COMMENT THROUGH ALL OF THIS OTHERWISE I'LL FORGET SOMETHING LIKE A DUMB IDIOT LATER ON !!

    private Vector3 touch;
    //public Camera cam;
    public CinemachineCamera cam;
    public Transform target;
    public float moveSpeed = 0.025f;
    public float groundZ = 0;
    public float zoomOutMin = 50.0f;
    public float zoomOutMax = 90.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Ignore UI touches
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // Get vector3 position of where the player held down a tap
        if (Input.GetMouseButtonDown(0))
        {
            //touch = GetWorldPosition(groundZ);

            touch = Input.mousePosition;
        }
        // Handles zooming
        if (Input.touchCount == 2)
        {
            // Get the two touch IDs
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Get the difference in position between where they originally were and where they are now
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Get magnitudes and calculate the difference between them
            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            // zoom the camera in / out by however much necessary
            Zoom(difference * 0.01f);
        }
        // Handle panning the camera
        else if (Input.GetMouseButton(0))
        {
            Touch fingertouch = Input.GetTouch(0);
            // Ignore UI touches
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingertouch.fingerId))
            {
                return;
            }
            // ughhhhhhhhhhh this is kinda stupid
            // Get the difference in position, fetch camforward and camright because the camera is angled
            //Vector3 direction = touch - GetWorldPosition(groundZ);
            Vector3 deltaMovement = Input.mousePosition - touch;

            // why won't you let me do this in one nice little line...
            Vector3 camForward = cam.transform.forward;
            Vector3 camRight = cam.transform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // Calculate the direction
            Vector3 direction = (-deltaMovement.x * moveSpeed * camRight) + (-deltaMovement.y * moveSpeed * camForward);

            // Gotta clamp the position of the target so that the cam boundaries don't feel like superglue when the target goes wayyyy out of bounds and the player tries to move the cam in opposite direction
            Vector3 newPos = new Vector3(Mathf.Clamp(direction.x + target.position.x, -40.0f, 20.0f), target.position.y, (Mathf.Clamp(direction.z + target.position.z, -40.0f, 20.0f)));
            target.position = newPos;
            touch = Input.mousePosition;
            
            //Camera.main.transform.position += direction;
        }
    }

    // Zoom camera
    void Zoom(float increment)
    {
        //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - increment, zoomOutMin, zoomOutMax);

        // Set FOV to a value between min and max values
        cam.Lens.FieldOfView = Mathf.Clamp(cam.Lens.FieldOfView - increment, zoomOutMin, zoomOutMax);
    }

    //private Vector3 GetWorldPosition(float z)
    //{
    //    Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
    //    Plane ground = new Plane(Vector3.down, new Vector3(0, 0, z));
    //    float distance;
    //    ground.Raycast(mousePos, out distance);
    //    return mousePos.GetPoint(distance);
    //}
}