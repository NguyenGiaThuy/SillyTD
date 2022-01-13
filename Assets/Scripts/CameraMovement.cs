using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera panning")]
    [SerializeField]
    private float panSpeed;
    [SerializeField]
    private float panPadding;
    [SerializeField]
    private float panMinX;
    [SerializeField]
    private float panMaxX;
    [SerializeField]
    private float panMinZ;
    [SerializeField]
    private float panMaxZ;

    [Header("Camera scrolling")]
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private float scrollMin;
    [SerializeField]
    private float scrollMax;

    // Scrolling velocity
    private Vector3 velocity;

    // Track scrolling
    private float newPositionY;

    private Transform pivot;
    private float initialPivotY;

    private void Start()
    {
        velocity = Vector3.zero;
        newPositionY = transform.position.y;
        pivot = GameObject.Find("CameraPivot").GetComponent<Transform>();
        initialPivotY = pivot.position.y;
    }

    private void Update()
    {
        
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panPadding) 
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey("s") || Input.mousePosition.y <= panPadding) 
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panPadding) 
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey("a") || Input.mousePosition.x <= panPadding) 
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);

        // Clamp panning
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        newPosition.x = Mathf.Clamp(newPosition.x, panMinX, panMaxX);
        newPosition.z = Mathf.Clamp(newPosition.z, panMinZ, panMaxZ);
        transform.position = newPosition;

        // Smooth scrolling
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * 1000f * Time.deltaTime;
        newPositionY -= scroll;
        newPositionY = Mathf.Clamp(newPositionY, scrollMin, scrollMax);
        newPosition = new Vector3(transform.position.x, newPositionY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, 0.5f);

        // Rotate camera after scrolling
        Vector3 direction = new Vector3(pivot.position.x, initialPivotY, pivot.position.z) - newPosition;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, scrollSpeed / 5f * Time.deltaTime);
    }
}
