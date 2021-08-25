using UnityEngine;

public class Destination : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Mob")
        {
            Destroy(other.gameObject);
        }
    }
}
