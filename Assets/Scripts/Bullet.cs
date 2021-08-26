using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject explosionHitBox;

    private  Vector3 direction;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        initialPosition = transform.position;
        Destroy(gameObject, 5f);
    }

    private void Start()
    {
        targetPosition = target.position;
        direction = targetPosition - transform.position;
        direction.y = 0.5f; // Offset
    }

    private void Update()
    {
        Vector3 currentDirection = transform.position - initialPosition;

        // Prevent flying passes target
        if (currentDirection.magnitude >= direction.magnitude)
        {
            TargetHit();
            return;
        }

        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    private void TargetHit()
    {
        Instantiate(explosionHitBox, targetPosition, explosionHitBox.transform.rotation); 
        Destroy(gameObject);
    }
}
