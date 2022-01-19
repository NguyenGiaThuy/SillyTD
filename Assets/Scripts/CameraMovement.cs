using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera panning")]
    [SerializeField]
    private float panSpeed;
    [SerializeField]
    private float panMinX;
    [SerializeField]
    private float panMaxX;
    [SerializeField]
    private float panMinY;
    [SerializeField]
    private float panMaxY;

    [Header("Camera zooming")]
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float zoomMin;
    [SerializeField]
    private float zoomMax;

    private Transform pivot;
    private float initialPivotY;

    private CinemachineInputProvider inputProvider;

    // Start is called before the first frame update
    void Start()
    {
        inputProvider = GetComponent<CinemachineInputProvider>();
        pivot = GameObject.Find("CameraPivot").GetComponent<Transform>();
        initialPivotY = pivot.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float x = inputProvider.GetAxisValue(0);
        float y = inputProvider.GetAxisValue(1);
        float z = inputProvider.GetAxisValue(2);

        if (x != 0f || y != 0f) PanScreen(x, y);
        if (z != 0f) ZoomScreen(z);
    }

    public Vector3 PanDirection(float x, float y)
    {
        Vector3 direction = Vector3.zero;

        if(y >= Screen.height * 0.95f) direction += Vector3.forward;
        if(y <= Screen.height * 0.05f) direction += Vector3.back;
        if (x >= Screen.width * 0.95f) direction += Vector3.right;
        if (x <= Screen.width * 0.05f) direction += Vector3.left;

        return direction;
    }

    public void PanScreen(float x, float y)
    {
        Vector3 direction = PanDirection(x, y);
        Vector3 newPosition = Vector3.Lerp(transform.position, transform.position + direction, panSpeed * Time.deltaTime);
        newPosition.x = Mathf.Clamp(newPosition.x, panMinX, panMaxX);
        newPosition.z = Mathf.Clamp(newPosition.z, panMinY, panMaxY);
        transform.position = newPosition;
    }

    public Vector3 ZoomDirection(float z)
    {
        Vector3 direction = Vector3.zero;
        direction += new Vector3(direction.x, z, direction.z);
        direction.Normalize();
        return direction;
    }

    public void ZoomScreen(float z)
    {
        Vector3 direction = ZoomDirection(z);
        Vector3 newPosition = Vector3.Lerp(transform.position, transform.position + direction, zoomSpeed * Time.deltaTime);
        newPosition.y = Mathf.Clamp(newPosition.y, zoomMin, zoomMax);
        transform.position = newPosition;

        // Rotate camera after zooming
        Vector3 pivotDirection = new Vector3(pivot.position.x, initialPivotY, pivot.position.z) - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(pivotDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, zoomSpeed * Time.deltaTime);
    }
}
